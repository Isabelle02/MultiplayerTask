using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerView : MonoBehaviourPun
{
    private CharacterView _characterView;
    private int _hpValue;
    private int _coinReward;
    private float _passTime;

    private Dictionary<string, CharacterView> CharacterViews = new();

    private const float CoinSpawnInterval = 2f;
    private const float CoinRadius = 0.5f;
    
    public static event Action<int> HpChanged;
    
    private int HpValue
    {
        get => _hpValue;
        set
        {
            _hpValue = value;
            HpChanged?.Invoke(value);
        }
    }
    
    private static Bounds CameraBounds => Camera.main.Bounds();
    private string Id => PhotonNetwork.LocalPlayer.UserId;

    private void Awake()
    {
        HpValue = 100;
    }

    public void Init()
    {
        photonView.RPC("CreateCharacter", RpcTarget.AllBuffered, Id);

        _characterView = CharacterViews[Id];
        _characterView.Damaged += OnCharacterDamaged;

        CurrencyManager.CoinCollect += OnCoinCollected;
    }

    private void Update()
    {
        _passTime += Time.deltaTime;

        if (_passTime > CoinSpawnInterval)
        {
            var chance = Random.Range(0, 100);
            if (chance < 70)
                return;
            
            _passTime = 0;
            var position = new Vector3(Random.Range(CameraBounds.min.x, CameraBounds.max.x),
                Random.Range(CameraBounds.min.y, CameraBounds.max.y));
            photonView.RPC("SpawnCoin", RpcTarget.AllBuffered, position);
        }
    }

    private void OnCoinCollected(int amount)
    {
        _coinReward += amount - CurrencyManager.Coins;
    }

    private void OnCharacterDamaged(int damage)
    {
        HpValue -= damage;
        if (HpValue <= 0)
        {
            PopupManager.Open<MatchCompletionPopup>(new MatchCompletionPopup.Param(false, _coinReward));
        }
    }

    public void SendMove(Vector3 vector)
    {
        photonView.RPC("Move", RpcTarget.AllBuffered, Id, vector);
    }

    public void SendShoot()
    {
        var characterTransform = CharacterViews[Id].transform;
        photonView.RPC("Shoot", RpcTarget.AllBuffered, characterTransform.position, characterTransform.rotation);
    }

    [PunRPC]
    private void SpawnCoin(Vector3 position)
    {
        var coin = Pool.Get<CoinView>();
        coin.transform.position = position;
    }
    
    [PunRPC]
    private void CreateCharacter(string id)
    {
        var character = Pool.Get<CharacterView>(transform);
        Debug.LogError($"adding character {Id}");
        CharacterViews.Add(id, character);
    }
    
    [PunRPC]
    private void Shoot(Vector3 position, Quaternion rotation)
    {
        var bullet = Pool.Get<BulletView>(transform);
        bullet.Run(position, rotation);
    }

    [PunRPC]
    private void Move(string id, Vector3 vector)
    {
        var character = CharacterViews[id];
        var size = character.Collider2D.bounds.size;
        var characterTransform = character.transform;
        characterTransform.Translate(vector, Space.World);
        if (vector != Vector3.zero)
            characterTransform.LookAt2D(vector);
        
        var min = new Vector2(-CameraBounds.size.x + size.x / 2,
            -CameraBounds.size.y + size.y / 2);
        var max = new Vector2(CameraBounds.size.x - size.x / 2,
            CameraBounds.size.y - size.y / 2);
                
        var targetX = Mathf.Clamp(characterTransform.position.x, min.x, max.x);
        var targetY = Mathf.Clamp(characterTransform.position.y, min.y, max.y);
        characterTransform.SetPositionXY(targetX, targetY);
    }
}
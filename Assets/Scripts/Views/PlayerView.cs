using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerView : MonoBehaviourPun
{
    [SerializeField] private Color[] _colors;

    private readonly List<Color> _leftColors = new();

    private int _hpValue;
    private float _passTime;

    private static readonly Dictionary<string, CharacterView> CharacterViews = new();

    private const float CoinSpawnInterval = 2f;

    public static event Action Inited;
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
    
    private static Bounds CameraBounds => CameraManager.GameCamera.Bounds();
    private static Vector3 RandomPosition => new (Random.Range(CameraBounds.min.x, CameraBounds.max.x),
        Random.Range(CameraBounds.min.y, CameraBounds.max.y));

    private static string Id => PhotonNetwork.LocalPlayer.UserId;
    public int CoinReward { get; private set; }
    public static CharacterView CharacterView { get; private set; }
    public static Color Color { get; private set; }

    private void Awake()
    {
        HpValue = 100;
        _leftColors.AddRange(_colors);
    }

    public void Init()
    {
        var colorIndex = Random.Range(0, _leftColors.Count);
        photonView.RPC("CreateCharacter", RpcTarget.AllBuffered, Id, RandomPosition, colorIndex);

        CharacterView = CharacterViews[Id];
        CharacterView.Damaged += OnCharacterDamaged;

        CurrencyManager.CoinCollect += OnCoinCollected;
        
        Inited?.Invoke();
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
            photonView.RPC("SpawnCoin", RpcTarget.AllBuffered, RandomPosition);
        }
    }

    private void OnCoinCollected(int amount)
    {
        CoinReward += amount - CurrencyManager.Coins;
    }

    private void OnCharacterDamaged(int damage)
    {
        HpValue -= damage;
        if (HpValue <= 0)
        {
            PopupManager.Open<MatchCompletionPopup>(new MatchCompletionPopup.Param(false, CoinReward));
        }
    }

    public void SendMove(Vector3 vector)
    {
        photonView.RPC("Move", RpcTarget.AllBuffered, Id, vector);
    }

    public void SendShoot()
    {
        photonView.RPC("Shoot", RpcTarget.AllBuffered, Id);
    }

    public void Disconnect()
    {
        photonView.RPC("ReleaseCharacter", RpcTarget.AllBuffered, Id);
        CharacterViews.Clear();
        CharacterView = null;
        Pool.Release(this);
    }

    [PunRPC]
    private void SpawnCoin(Vector3 position)
    {
        var coin = Pool.Get<CoinView>();
        coin.transform.position = position;
    }
    
    [PunRPC]
    private void CreateCharacter(string id, Vector3 position, int colorIndex)
    {
        var character = Pool.Get<CharacterView>(transform);
        character.transform.position = position;
        Color = _leftColors[colorIndex];
        character.Init(id, Color);
        _leftColors.RemoveAt(colorIndex);
        CharacterViews.Add(id, character);
    }
    
    [PunRPC]
    private void Shoot(string id)
    {
        var characterTransform = CharacterViews[id].transform;
        var bullet = Pool.Get<BulletView>();
        bullet.Run(id, characterTransform.position, characterTransform.rotation);
    }

    [PunRPC]
    private void Move(string id, Vector3 vector)
    {
        var character = CharacterViews[id];
        var size = character.Size;
        var characterTransform = character.transform;
        characterTransform.Translate(vector, Space.World);
        if (vector != Vector3.zero)
            characterTransform.LookAt2D(vector);
        
        var min = new Vector2(-CameraBounds.size.x / 2 + size.x / 2,
            -CameraBounds.size.y / 2 + size.y / 2);
        var max = new Vector2(CameraBounds.size.x / 2 - size.x / 2,
            CameraBounds.size.y / 2 - size.y / 2);
                
        var targetX = Mathf.Clamp(characterTransform.position.x, min.x, max.x);
        var targetY = Mathf.Clamp(characterTransform.position.y, min.y, max.y);
        characterTransform.SetPositionXY(targetX, targetY);
    }

    [PunRPC]
    private void ReleaseCharacter(string id)
    {
        Pool.Release(CharacterViews[id]);
        CharacterViews.Remove(id);
    }
}
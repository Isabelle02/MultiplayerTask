using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviourPun
{
    [SerializeField] private Image _hpFillImage;
    [SerializeField] private PlayerHandlerView _handler;
    [SerializeField] private Button _shootButton;

    public Canvas Canvas { get; private set; }
    private int _hpValue;
    
    private int HpValue
    {
        get => _hpValue;
        set
        {
            _hpValue = value;
            _hpFillImage.fillAmount = value / 100f;
        }
    }
    private string Id => PhotonNetwork.LocalPlayer.UserId;

    private void Awake()
    {
        Canvas = GetComponent<Canvas>();
        Canvas.worldCamera = Camera.main;
        
        _shootButton.onClick.AddListener(SendShoot);
        HpValue = 100;
    }

    private void Update()
    {
        SendMove(_handler.MovingOffset);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BulletView bullet))
        {
            HpValue--;
            if (HpValue <= 0)
            {
                PopupManager.Open<MatchCompletionPopup>(new MatchCompletionPopup.Param(false, 0));
            }
        }
    }

    private void SendMove(Vector3 vector)
    {
        photonView.RPC("Move", RpcTarget.AllBuffered, Id, vector);
    }

    private void SendShoot()
    {
        var characterTransform = NetworkController.CharacterViews[Id].transform;
        photonView.RPC("Shoot", RpcTarget.AllBuffered, characterTransform.position, characterTransform.rotation);
    }
    
    [PunRPC]
    private void Shoot(Vector3 position, Quaternion rotation)
    {
        var bullet = Pool.Get<BulletView>(Canvas.transform);
        bullet.Run(position, rotation);
    }

    [PunRPC]
    private void Move(string id, Vector3 vector)
    {
        var character = NetworkController.CharacterViews[id].transform;
        character.Translate(vector, Space.World);
        character.SetRotationZ(-Mathf.Asin(vector.y / vector.magnitude));
    }
}
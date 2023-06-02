using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{
    private static NetworkController _instance;
    
    private string _lobbyName;
    public static PlayerView PlayerView { get; private set; }

    public static event Action Disconnected;

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public static bool CreateRoom(string roomName)
    {
        Debug.LogWarning("[NetworkController] Creating Room");

        return PhotonNetwork.CreateRoom(roomName);
    }

    public static bool JoinRoom(string roomName)
    {
        Debug.LogWarning("[NetworkController] Joining Room");

        return PhotonNetwork.JoinRoom(roomName);
    }

    public static void TryToConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected due to: {cause}");
        Disconnected?.Invoke();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.LogWarning("YOU JOINED ROOM");
        if (PhotonNetwork.CurrentRoom.PlayerCount <= 1) 
            return;
        
        PlayerView = Pool.GetPun<PlayerView>();
        PlayerView.Init();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        
        Debug.LogWarning("ANOTHER PLAYER JOINED ROOM");
        PlayerView = Pool.GetPun<PlayerView>();
        PlayerView.Init();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            PopupManager.Open<MatchCompletionPopup>(new MatchCompletionPopup.Param(true, PlayerView.CoinReward));
    }

    public static void Disconnect()
    {
        if (PlayerView)
        {
            PlayerView.Disconnect();
            PlayerView = null;
        }
        
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }
}
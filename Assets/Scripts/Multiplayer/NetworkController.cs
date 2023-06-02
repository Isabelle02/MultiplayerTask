using System;
using ExitGames.Client.Photon;
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

        Debug.LogWarning("JOINED ROOM");
        PlayerView = Pool.GetPun<PlayerView>();
        PlayerView.Init();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    }

    public static void Disconnect()
    {
        PlayerView.Disconnect();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        PlayerView = null;
    }
}
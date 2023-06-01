using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{
    private static NetworkController _instance;
    
    private string _lobbyName;
    public static PlayerView PlayerView;

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

    public static bool JoinRoom(string roomName)
    {
        Debug.LogWarning("Try join room");

        return PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions(), TypedLobby.Default);
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
}
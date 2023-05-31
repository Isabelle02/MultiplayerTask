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
    private PlayerView _playerView;

    public static event Action Disconnected;

    public static Dictionary<string, CharacterView> CharacterViews = new();

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

    public static bool JoinLobby(string lobbyName)
    {
        return PhotonNetwork.JoinLobby(new TypedLobby(lobbyName, LobbyType.SqlLobby));
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

    public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        base.OnLobbyStatisticsUpdate(lobbyStatistics);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        _playerView = Pool.Get<PlayerView>();
        
        var playerId = PhotonNetwork.LocalPlayer.UserId;
        photonView.RPC("CreateCharacter", RpcTarget.AllBuffered, playerId);
    }

    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    }

    [PunRPC]
    private void CreateCharacter(string id)
    {
        var character = Pool.Get<CharacterView>(_playerView.Canvas.transform);
        CharacterViews.Add(id, character);
    }
}
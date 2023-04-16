using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using SimpleEventBus.Disposables;
using UnityEngine;

public class GameManager : NetworkBehaviour, IDisposable
{
    [SerializeField] private GeneralScoreScreenController _generalScoreScreenController;
    [SerializeField] private PlayerNameScreenController _playerNameScreenController;
    [SerializeField] private WinScreenController _winScreenController;

    [SerializeField] private float _restartTime;

    private List<Player> _players = new ();
    private CompositeDisposable _subscriptions;

    public void AddNewPlayer(Player player)
    {
        _players.Add(player);
        
        RpcTransferPlayersListToClients(_players);
        RpcChangeCountPlayersForAllPlayers(_players);
        RpcInitializePlayerNameInputController(player);
    }

    private void Awake()
    {
        _subscriptions = new CompositeDisposable
        {
            EventStreams.Game.Subscribe<PlayerWonEvent>(InitializeEndGame)
        };
    }

    [ClientRpc]
    private void RpcInitializePlayerNameInputController(Player player)
    {
        _playerNameScreenController.Initialize(player, ActivatePlayer);
    }
    
    [ClientRpc]
    private void RpcChangeCountPlayersForAllPlayers(List<Player> players)
    {
        _generalScoreScreenController.ChangeCountPlayers(players);
    }

    [ClientRpc]
    private void RpcTransferPlayersListToClients(List<Player> players)
    {
        _players = players;
    }
    
    private void ActivatePlayer(Player player, string playerName)
    {
        player.SetName(playerName);
        _playerNameScreenController.gameObject.SetActive(false);
    }
    
    private void InitializeEndGame(PlayerWonEvent eventData)
    {

        EndGame(eventData.Player);
    }
    
    private void EndGame(Player player)
    {
        RpcEndGame(player);
    }

    [ClientRpc]
    private void RpcEndGame(Player player)
    {
        _winScreenController.gameObject.SetActive(true);
        
        for (var i = 0; i < _players.Count; i++)
        {
            _players[i].RestScore();
            _players[i].gameObject.SetActive(false);
        }
        
        _winScreenController.Initialize(player);
        StartCoroutine(RespawnPlayers());
    }
    
    private IEnumerator RespawnPlayers()
    {
        yield return new WaitForSeconds(_restartTime);
        
        foreach (var player in _players)
        {
            player.transform.position = player.StartPosition;
            player.gameObject.SetActive(true);
        }
        
        _winScreenController.gameObject.SetActive(false);
    }

    public void Dispose()
    {
        _subscriptions?.Dispose();
    }
}
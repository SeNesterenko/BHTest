using System;
using System.Collections.Generic;
using Mirror;
using SimpleEventBus.Disposables;
using UnityEngine;

public class GameManager : NetworkBehaviour, IDisposable
{
    [SerializeField] private GeneralScoreScreenController _generalScoreScreenController;
    [SerializeField] private PlayerNameScreenController _playerNameScreenController;
    [SerializeField] private WinScreenController _winScreenController;

    private readonly List<Player> _players = new ();
    private CompositeDisposable _subscriptions;

    public void AddNewPlayer(Player player)
    {
        _players.Add(player);
        
        ChangeCountPlayersForAllPlayers(_players);
        InitializePlayerNameInputController(player);
    }

    private void Awake()
    {
        _subscriptions = new CompositeDisposable
        {
            EventStreams.Game.Subscribe<PlayerWonEvent>(InitializeEndGame)
        };
    }

    [ClientRpc]
    private void InitializePlayerNameInputController(Player player)
    {
        _playerNameScreenController.Initialize(player, ActivatePlayer);
    }
    
    [ClientRpc]
    private void ChangeCountPlayersForAllPlayers(List<Player> players)
    {
        _generalScoreScreenController.ChangeCountPlayers(players);
    }
    
    private void ActivatePlayer(Player player, string playerName)
    {
        player.SetName(playerName);
        _playerNameScreenController.gameObject.SetActive(false);
    }
    
    private void InitializeEndGame(PlayerWonEvent eventData)
    {
        if (!isServer || !NetworkServer.active)
        {
            return;
        }
        
        EndGame(eventData.Player);
    }

    private void EndGame(Player player)
    {
        _generalScoreScreenController.gameObject.SetActive(false);
        _winScreenController.Initialize(player);

        RpcEndGame(player);
    }

    [ClientRpc]
    private void RpcEndGame(Player player)
    {
        _generalScoreScreenController.gameObject.SetActive(false);
        _winScreenController.gameObject.SetActive(true);
        _winScreenController.Initialize(player);
    }

    public void Dispose()
    {
        _subscriptions?.Dispose();
    }
}
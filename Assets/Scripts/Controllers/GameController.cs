using System;
using System.Collections.Generic;
using Events;
using Mirror;
using SimpleEventBus.Disposables;
using UI.Controllers;
using UnityEngine;

namespace Controllers
{
    public class GameController : NetworkBehaviour, IDisposable
    {
        [SerializeField] private GeneralScoreScreenController _generalScoreScreenController;
        [SerializeField] private PlayerNameScreenController _playerNameScreenController;
        [SerializeField] private WinScreenController _winScreenController;
        [SerializeField] private PlayerRespawnController _playerRespawnController;

        private List<Player.Player> _players = new ();
        private CompositeDisposable _subscriptions;

        public void AddNewPlayer(Player.Player player)
        {
            _players.Add(player);
        
            RpcTransferPlayersListToClients(_players);
            RpcChangeCountPlayersForAllPlayers(_players);
            RpcInitializePlayerNameInputController(player);
        }

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Confined;
            
            _subscriptions = new CompositeDisposable
            {
                EventStreams.Game.Subscribe<PlayerWonEvent>(InitializeEndGame)
            };
        }

        [ClientRpc]
        private void RpcInitializePlayerNameInputController(Player.Player player)
        {
            _playerNameScreenController.Initialize(player, ActivatePlayer);
        }
    
        [ClientRpc]
        private void RpcChangeCountPlayersForAllPlayers(List<Player.Player> players)
        {
            _generalScoreScreenController.ChangeCountPlayers(players);
        }

        [ClientRpc]
        private void RpcTransferPlayersListToClients(List<Player.Player> players)
        {
            _players = players;
        }
    
        private void ActivatePlayer(Player.Player player, string playerName)
        {
            player.SetName(playerName);
            _playerNameScreenController.gameObject.SetActive(false);
        }
    
        private void InitializeEndGame(PlayerWonEvent eventData)
        {
            EndGame(eventData.Player);
        }
    
        private void EndGame(Player.Player player)
        {
            RpcEndGame(player);
        }

        [ClientRpc]
        private void RpcEndGame(Player.Player player)
        {
            _winScreenController.gameObject.SetActive(true);
        
            for (var i = 0; i < _players.Count; i++)
            {
                _players[i].ResetPlayer();
                _players[i].gameObject.SetActive(false);
            }
        
            _winScreenController.Initialize(player);
            _playerRespawnController.InitializeRespawnPlayers(_players, DisableWinScreen);
        }

        private void DisableWinScreen()
        {
            _winScreenController.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _subscriptions?.Dispose();
        }
    }
}
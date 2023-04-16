using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class PlayerRespawnController : MonoBehaviour
    {
        [SerializeField] private float _restartTime;
    
        private List<Player.Player> _players;
        private Action _playersRespawned;

        public void InitializeRespawnPlayers(List<Player.Player> players, Action playersRespawned)
        {
            _playersRespawned = playersRespawned;
            _players = players;
            StartCoroutine(RespawnPlayers());
        }
    
        private IEnumerator RespawnPlayers()
        {
            yield return new WaitForSeconds(_restartTime);
        
            foreach (var player in _players)
            {
                player.gameObject.SetActive(true);
            }
        
            _playersRespawned.Invoke();
        }
    }
}
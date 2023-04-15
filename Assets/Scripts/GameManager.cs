using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GeneralScoreController _generalScoreController;
    
    private List<Player> _players = new ();

    public void AddNewPlayer(Player player)
    {
        _players.Add(player);
        ChangeCountPlayersForAllPlayers(_players);
    }

    [ClientRpc]
    private void ChangeCountPlayersForAllPlayers(List<Player> players)
    {
        if (!isServer)
        {
            _players = players;
        }
        
        _generalScoreController.ChangeCountPlayers(players);
    }
}
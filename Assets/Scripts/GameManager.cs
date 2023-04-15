using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GeneralScoreController _generalScoreController;
    [SerializeField] private PlayerNameController _playerNameController;

    private readonly List<Player> _players = new ();

    public void AddNewPlayer(Player player)
    {
        _players.Add(player);
        
        ChangeCountPlayersForAllPlayers(_players);
        InitializePlayerNameInputController(player);
    }

    [ClientRpc]
    private void InitializePlayerNameInputController(Player player)
    {
        _playerNameController.Initialize(player);
    }
    
    [ClientRpc]
    private void ChangeCountPlayersForAllPlayers(List<Player> players)
    {
        _generalScoreController.ChangeCountPlayers(players);
    }
}
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GeneralScoreScreenView))]
public class GeneralScoreScreenController : MonoBehaviour
{
    [SerializeField] private GeneralScoreScreenView _generalScoreScreenView;
    
    private List<Player> _players = new();

    public void ChangeCountPlayers(List<Player> players)
    {
        _generalScoreScreenView.ResetScoreViews();
        _players = players;
        
        var playerNames = new List<string>();
        var playerScores = new List<string>();
        
        foreach (var player in _players)
        {
            playerNames.Add(player.GetName());
            playerScores.Add(player.GetScore().ToString());
        }
        
        _generalScoreScreenView.ActivateNewScoreView(playerScores, playerNames);
    }

    private void Update()
    {
        for (var i = 0; i < _players.Count; i++)
        {
            var player = _players[i];
            _generalScoreScreenView.Display(player.GetScore().ToString(), i, player.GetName());
        }
    }
}
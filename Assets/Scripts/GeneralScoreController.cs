using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GeneralScoreView))]
public class GeneralScoreController : MonoBehaviour
{
    [SerializeField] private GeneralScoreView _generalScoreView;
    
    private List<Player> _players = new();

    public void ChangeCountPlayers(List<Player> players)
    {
        _generalScoreView.ResetScoreViews();
        _players = players;
        
        var playerNames = new List<string>();
        var playerScores = new List<string>();
        
        foreach (var player in _players)
        {
            playerNames.Add(player.GetName());
            playerScores.Add(player.GetScore().ToString());
        }
        
        _generalScoreView.ActivateNewScoreView(playerScores, playerNames);
    }

    private void Update()
    {
        for (var i = 0; i < _players.Count; i++)
        {
            var player = _players[i];
            _generalScoreView.Display(player.GetScore().ToString(), i, player.GetName());
        }
    }
}
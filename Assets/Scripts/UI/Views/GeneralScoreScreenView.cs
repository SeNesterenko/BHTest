using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace UI.Views
{
    public class GeneralScoreScreenView : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] _playersScore;
    
        public void Display(string playerScore, int indexPlayer, string playerName)
        {
            var text = new StringBuilder();
            text.AppendFormat($"{playerName}: {playerScore}");
            _playersScore[indexPlayer].text = text.ToString();
        }

        public void ActivateNewScoreView(List<string> scores, List<string> names)
        {
            for (var i = 0; i < scores.Count; i++)
            {
                _playersScore[i].gameObject.SetActive(true);
                Display(scores[i], i, names[i]);
            }
        }

        public void ResetScoreViews()
        {
            foreach (var scoreView in _playersScore)
            {
                scoreView.gameObject.SetActive(false);
            }
        }
    }
}
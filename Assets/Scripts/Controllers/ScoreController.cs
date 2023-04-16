using Mirror;
using UnityEngine;

namespace Controllers
{
    public class ScoreController : NetworkBehaviour
    {
        [SerializeField] private int _countScoreToWin;
    
        [SyncVar(hook = nameof(OnScoreChanged))]
        private int _score;
    
        public int GetScore()
        {
            return _score;
        }
    
        public void ResetScore()
        {
            if (isServer)
            {
                _score = 0;
            }
            else
            {
                CmdResetScore();
            }
        }
    
        public void IncreaseScore(int value)
        {
            if (isServer)
            {
                _score += value;
            }
            else
            {
                CmdIncreaseScore(value);
            }
        }

        public bool CheckWinScore()
        {
            return _score >= _countScoreToWin;
        }
    
        private void OnScoreChanged(int oldValue, int newValue)
        {
            _score = newValue;
        }
    
        [Command]
        private void CmdIncreaseScore(int value)
        {
            _score += value;
        }
    
        [Command]
        private void CmdResetScore()
        {
            _score = 0;
        }
    }
}
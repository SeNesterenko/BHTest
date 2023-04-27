using Cinemachine;
using Controllers;
using Events;
using JetBrains.Annotations;
using Mirror;
using Skills;
using UnityEngine;

namespace PlayerEntity
{
    public class Player : NetworkBehaviour
    {
        public Renderer Renderer => _renderer;

        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Skill _skill;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Collider _collider;
        [SerializeField] private ScoreController _scoreController;

        private CinemachineFreeLook _playerCamera;
        private bool _isInvincible;
        private Vector3 _startPosition;

        [SyncVar(hook = nameof(OnNameChanged))]
        private string _name;

        [ClientRpc]
        public void RpcChangeState()
        {
            _isInvincible = !_isInvincible;
            _collider.enabled = !_isInvincible;
            _renderer.material.color = _defaultColor;
        }
    
        [UsedImplicitly]
        public void ActivateSkill()
        {
            _skill.ActivateSkill();
        }

        public void IncreaseScore(int value)
        {
            _scoreController.IncreaseScore(value);

            if (isServer && _scoreController.CheckWinScore())
            {
                EventStreams.Game.Publish(new PlayerWonEvent(this));
            }
            else 
            {
                CmdInitializePlayerWonEvent();
            }
        }

        public int GetScore()
        {
            return _scoreController.GetScore();
        }
    
        public string GetName()
        {
            return _name;
        }

        public void SetName(string playerName)
        {
            if (isServer)
            {
                _name = playerName;
            }
            else
            {
                CmdSetName(playerName);
            }
        }

        public void ResetPlayer()
        {
            _scoreController.ResetScore();
            if (_isInvincible && isServer)
            {
                RpcChangeState();
            }
            else if (_isInvincible)
            {
                ChangeState();
            }
        
            if (!isLocalPlayer) return;
            transform.position = _startPosition;
        }

        private void Start()
        {
            if (isLocalPlayer)
            {
                _playerCamera = FindObjectOfType<CinemachineFreeLook>();
                _playerCamera.Follow = transform;
                _playerCamera.LookAt = transform;
                _startPosition = transform.position;
            }
        }
    
        private void ChangeState()
        {
            _isInvincible = !_isInvincible;
            _collider.enabled = !_isInvincible;
            _renderer.material.color = _defaultColor;
        }

        private void OnNameChanged(string oldValue, string newValue)
        {
            _name = newValue;
        }

        [Command]
        private void CmdInitializePlayerWonEvent()
        {
            if (_scoreController.CheckWinScore())
            {
                EventStreams.Game.Publish(new PlayerWonEvent(this));
            }
        }

        [Command]
        private void CmdSetName(string playerName)
        {
            _name = playerName;
        }
    }
}
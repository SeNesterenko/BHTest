using Cinemachine;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public Vector3 StartPosition { get; private set; }

    public Renderer Renderer => _renderer;

    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Skill _skill;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Collider _collider;
    [SerializeField] private int _countScoreToWin;

    private CinemachineFreeLook _playerCamera;
    private bool _isInvincible;

    [SyncVar(hook = nameof(OnScoreChanged))]
    private int _score;

    [SyncVar(hook = nameof(OnNameChanged))]
    private string _name;

    [UsedImplicitly]
    public void ActivateSkill()
    {
        _skill.ActivateSkill();
    }
    
    [ClientRpc]
    public void RpcChangeState()
    {
        _isInvincible = !_isInvincible;
        _collider.enabled = !_isInvincible;
        _renderer.material.color = _defaultColor;
    }
    
    public void ChangeState()
    {
        _isInvincible = !_isInvincible;
        _collider.enabled = !_isInvincible;
        _renderer.material.color = _defaultColor;
    }
    
    public int GetScore()
    {
        return _score;
    }
    
    public void IncreaseScore(int value)
    {
        if (isServer)
        {
            _score += value;
            if (_score >= _countScoreToWin)
            {
                EventStreams.Game.Publish(new PlayerWonEvent(this));
            }
        }
        else
        {
            CmdIncreaseScore(value);
        }
    }

    public void RestScore()
    {
        if (isServer)
        {
            _score = 0;
            if (_isInvincible)
            {
                ChangeState();
            }
        }
        else
        {
            CmdResetScore();
        }
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

    private void Start()
    {
        if (isLocalPlayer)
        {
            _playerCamera = FindObjectOfType<CinemachineFreeLook>();
            _playerCamera.Follow = transform;
            _playerCamera.LookAt = transform;
            StartPosition = transform.position;
        }
    }
    
    private void OnScoreChanged(int oldValue, int newValue)
    {
        _score = newValue;
    }
    
    [Command]
    private void CmdIncreaseScore(int value)
    {
        _score += value;
        if (_score >= _countScoreToWin)
        {
            EventStreams.Game.Publish(new PlayerWonEvent(this));
        }
    }

    [Command]
    private void CmdResetScore()
    {
        _score = 0;
        if (_isInvincible)
        {
            RpcChangeState();
        }
    }

    private void OnNameChanged(string oldValue, string newValue)
    {
        _name = newValue;
    }

    [Command]
    private void CmdSetName(string playerName)
    {
        _name = playerName;
    }
}
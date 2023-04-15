using Cinemachine;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public Renderer Renderer => _renderer;

    [SerializeField] private Skill _skill;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Collider _collider;

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
    public void ChangeState()
    {
        _isInvincible = !_isInvincible;

        _collider.enabled = !_isInvincible;
    }
    
    public int GetScore()
    {
        return _score;
    }
    
    public void SetScore(int value)
    {
        if (isServer)
        {
            _score += value;
        }
        else
        {
            CmdSetScore(value);
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
        }
    }
    
    private void OnScoreChanged(int oldValue, int newValue)
    {
        _score = newValue;
    }
    
    [Command]
    private void CmdSetScore(int value)
    {
        _score += value;
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
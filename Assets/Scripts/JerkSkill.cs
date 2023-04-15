using System.Collections;
using UnityEngine;
using DG.Tweening;
using Mirror;

public class JerkSkill : Skill
{
    [SerializeField] private Player _player;
    
    [SerializeField] private float _distanceJerk = 10f;
    [SerializeField] private float _durationJerk = 1f;
    
    [SerializeField] private float _colorDelay = 3f;
    [SerializeField] private Color _specialColor = Color.black;
    [SerializeField] private Color _defaultColor = Color.white;

    private bool _isJerkState;

    public override void ActivateSkill()
    {
        var direction = transform.forward;
        var destination = transform.position + direction * _distanceJerk;

        _isJerkState = true;
        transform.DOMove(destination, _durationJerk).OnComplete(() => _isJerkState = false);
    }

    private void OnTriggerExit(Collider otherCollider)
    {
        if(!_isJerkState) return;
        if (!isLocalPlayer) return;
        
        if (otherCollider.gameObject.CompareTag(Tags.PLAYER))
        {
            CmdChangePlayerColor(otherCollider.gameObject.GetComponent<NetworkIdentity>().netId);
            _player.IncreaseScore(1);
        }
    }

    [Command]
    private void CmdChangePlayerColor(uint playerId)
    {
        var player = NetworkServer.spawned[playerId].gameObject.GetComponent<Player>();

        RpcChangePlayerColor(player, _specialColor);
        player.ChangeState();
        StartCoroutine(ResetPlayerColor(player));
    }
    
    
    [ClientRpc]
    private void RpcChangePlayerColor(Player player, Color color)
    {
        var playerRenderer = player.Renderer;
        playerRenderer.material.color = color;
    }
    
    private IEnumerator ResetPlayerColor(Player player)
    {
        yield return new WaitForSeconds(_colorDelay);
        
        RpcChangePlayerColor(player, _defaultColor);
        player.ChangeState();
    }
}
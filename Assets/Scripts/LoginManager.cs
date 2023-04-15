using Mirror;
using UnityEngine;

public class LoginManager : NetworkManager
{
    [SerializeField] private GameManager _gameManager;
    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    { 
        base.OnServerAddPlayer(conn);
        var player = conn.identity.gameObject.GetComponent<Player>();
        _gameManager.AddNewPlayer(player);
    }
}
using Controllers;
using Mirror;
using UnityEngine;

public class LoginManager : NetworkManager
{
    [SerializeField] private GameController _gameController;
    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    { 
        base.OnServerAddPlayer(conn);
        var player = conn.identity.gameObject.GetComponent<Player.Player>();
        _gameController.AddNewPlayer(player);
    }
}
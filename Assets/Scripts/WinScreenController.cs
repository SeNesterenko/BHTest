using UnityEngine;

[RequireComponent(typeof(WinScreenView))]
public class WinScreenController : MonoBehaviour
{
    [SerializeField] private WinScreenView _winScreenView;

    public void Initialize(Player player)
    {
        _winScreenView.Display(player.GetName());
    }
}
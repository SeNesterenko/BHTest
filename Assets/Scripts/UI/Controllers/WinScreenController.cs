using UI.Views;
using UnityEngine;

namespace UI.Controllers
{
    [RequireComponent(typeof(WinScreenView))]
    public class WinScreenController : MonoBehaviour
    {
        [SerializeField] private WinScreenView _winScreenView;

        public void Initialize(Player.Player player)
        {
            _winScreenView.Display(player.GetName());
        }
    }
}
using UI.Views;
using UnityEngine;
using PlayerEntity;

namespace UI.Controllers
{
    [RequireComponent(typeof(WinScreenView))]
    public class WinScreenController : MonoBehaviour
    {
        [SerializeField] private WinScreenView _winScreenView;

        public void Initialize(Player player)
        {
            _winScreenView.Display(player.GetName());
        }
    }
}
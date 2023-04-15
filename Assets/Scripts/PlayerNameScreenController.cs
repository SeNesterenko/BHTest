
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameScreenController : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _activatePlayerButton;

    private Player _player;
    private Action<Player, string> _activatePlayerButtonClicked;

    public void Initialize(Player player, Action<Player, string> activatePlayerButtonClicked)
    {
        _activatePlayerButtonClicked = activatePlayerButtonClicked;
        _player = player;
        _activatePlayerButton.onClick.AddListener(ActivatePlayer);
    }

    private void ActivatePlayer()
    {
        _activatePlayerButtonClicked?.Invoke(_player, _inputField.text);
    }
}
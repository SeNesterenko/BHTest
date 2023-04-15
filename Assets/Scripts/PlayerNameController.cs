using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameController : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _activatePlayerButton;

    private Player _player;

    public void Initialize(Player player)
    {
        _player = player;
        _activatePlayerButton.onClick.AddListener(ActivatePlayer);
    }

    private void ActivatePlayer()
    {
        _player.SetName(_inputField.text);
        gameObject.SetActive(false);
    }
}
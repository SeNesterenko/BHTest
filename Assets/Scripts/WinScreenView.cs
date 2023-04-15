using TMPro;
using UnityEngine;

public class WinScreenView : MonoBehaviour
{
    [SerializeField] private TMP_Text _winnerName;

    public void Display(string playerName)
    {
        _winnerName.text = "Winner: " + playerName;
    }
}
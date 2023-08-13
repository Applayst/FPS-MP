using TMPro;
using UnityEngine;

public class TeamTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void SetTextTeam(bool value)
    {
        _text.color = Color.red;
        _text.text = "Team A";
        if (value)
        {
            _text.color = Color.blue;
            _text.text = "Team B";
        }
    }
}

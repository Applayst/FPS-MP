using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _textA;
    [SerializeField] private TMP_Text _textB;

    public void SetTeamsScore(int valueA, int valueB)
    {     
        _textA.text = $"Team A: {valueA}";
  
        _textB.text = $"Team B: {valueB}";
    }    
}

using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;    

    public void UpdateHealth(int current, float max)
    {        
        _slider.value = current / max;
    }
}

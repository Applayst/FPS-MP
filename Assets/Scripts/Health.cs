using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private HealthUI _healthUI;
    private int _max;
    private int _current;

    public void SetMax(int value)
    {        
        _max = value;
        UpdateHP();
    }

    public void SetCurrent(int value)
    {
        _current = value;
        UpdateHP();
    }

    public void ApplyDamage(int value)
    {        
        _current -= value;
        UpdateHP();     
    }

    private void UpdateHP()
    {
        _healthUI.UpdateHealth(_current, _max);
    }
}

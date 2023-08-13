using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [field: SerializeField] public int MaxHealth { get; protected set; } = 10;
    [field: SerializeField] public bool Team { get; protected set; }
    [field: SerializeField] public float Speed { get; protected set; }
    [field: SerializeField, Range(0.5f, 0.99f)] public float SitMultiplier { get; protected set; }
    public Vector3 Velocity { get; protected set; }
}

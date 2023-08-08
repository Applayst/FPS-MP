using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; protected set; }
    [field: SerializeField, Range(0.5f, 0.99f)] public float SitMultiplier { get; protected set; }
    public Vector3 Velocity { get; protected set; }
}

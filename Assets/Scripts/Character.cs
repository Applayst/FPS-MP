using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; protected set; }
    public Vector3 Velocity { get; protected set; }
}

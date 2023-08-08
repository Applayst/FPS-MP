using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPartPosition : MonoBehaviour
{
    [SerializeField] private Transform _positionPoint;

    void Update()
    {
        transform.position = _positionPoint.position;    
    }
}

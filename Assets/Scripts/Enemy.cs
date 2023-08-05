using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //private Vector3 _targetPosition = Vector3.zero;
    public Vector3 TargetPosition { get; private set; } = Vector3.zero;

    private float _velocitiMagnitude = 0;

    private void Start()
    {
        TargetPosition = transform.position;    
    }

    private void Update()
    {
        if (_velocitiMagnitude > 0.1f)
        {
            float maxDistance = _velocitiMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, maxDistance);
        }
        else
        {
            transform.position = TargetPosition;
        }
        
    }
    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageIntervalTime)
    {
        TargetPosition = position + (velocity * averageIntervalTime);
        _velocitiMagnitude = velocity.magnitude;
    }
}

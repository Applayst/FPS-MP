using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] private Transform _head;

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
    public void SetSpeed(float value) => Speed = value;
    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageIntervalTime)
    {
        TargetPosition = position + (velocity * averageIntervalTime);
        _velocitiMagnitude = velocity.magnitude;

        this.Velocity = velocity;
    }

    public void SetRotateX(float rotateX)
    {
        _head.localEulerAngles = new Vector3(rotateX, 0, 0);
    }

    public void SetRotateY(float rotateY)
    {
        transform.localEulerAngles = new Vector3(0, rotateY, 0);
    }
}
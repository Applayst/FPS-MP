using System;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _partPoints;
    [SerializeField] private Transform _collider;

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

    public void SetSitMultiplier(float sitMultiplier) => SitMultiplier = sitMultiplier;
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
    public void SitDown()
    {
        Vector3 localScale = new Vector3(1f, SitMultiplier, 1f);
        _partPoints.localScale = localScale;
        _body.localScale = localScale;
        _collider.localScale = new Vector3(1f, SitMultiplier * 0.6f + 0.4f, 1f);        
    }

    public void StandUp()
    {
        _partPoints.localScale = Vector3.one;
        _body.localScale = Vector3.one;
        _collider.localScale = Vector3.one;        
    }

}

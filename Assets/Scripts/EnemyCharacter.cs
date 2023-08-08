using System;
using System.Collections;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _partPoints;
    [SerializeField] private Transform _collider;

    public Vector3 TargetPosition { get; private set; } = Vector3.zero;

    private Quaternion _rotationY;
    private Quaternion _rotationX;

    private float _deltaRY = 0;
    private float _deltaRX = 0;

    private float _velocitiMagnitude = 0;
    private bool _isActiveCoroutine;

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
        
        if (Math.Abs(_deltaRY) > 0.1f)
        {
            float maxRY = 25 * Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, _rotationY, maxRY);           
        }
        else
        {
            transform.rotation = _rotationY;            
        }

        if (Math.Abs(_deltaRX) > 0.1f)
        {
            float maxRX = 25 * Time.deltaTime;
            _head.localRotation = Quaternion.Lerp(_head.localRotation, _rotationX, maxRX);            
        }
        else
        {
            _head.rotation = _rotationX;            
        }        
    }

    public void SetSitMultiplier(float sitMultiplier) => SitMultiplier = sitMultiplier;
    public void SetSpeed(float value) => Speed = value;
    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageIntervalTime)
    {
        TargetPosition = position;// + (velocity * averageIntervalTime);
        _velocitiMagnitude = velocity.magnitude;

        this.Velocity = velocity;
    }

    public void SetRotateX(float rotateX)
    {
        _deltaRX = _head.localEulerAngles.x - rotateX;
        _rotationX = Quaternion.Euler(rotateX, 0, 0);
    }

    public void SetRotateY(float rotateY)
    {
        _deltaRY = transform.localEulerAngles.y - rotateY;
        _rotationY = Quaternion.Euler(0, rotateY, 0);        
    }
    public void SitDown()
    {        
        if (_isActiveCoroutine)
            StopCoroutine(SitAndStand(1f));
        StartCoroutine(SitAndStand(SitMultiplier));
    }

    public void StandUp()
    {
        if (_isActiveCoroutine)
            StopCoroutine(SitAndStand(SitMultiplier));
        StartCoroutine(SitAndStand(1f));
    }

    private IEnumerator SitAndStand(float scaleMultiplier)
    {
        _isActiveCoroutine = true;

        Vector3 localScale = new Vector3(1f, scaleMultiplier, 1f);
        Vector3 colliderLcalScale = new Vector3(1f, scaleMultiplier * 0.6f + 0.4f, 1f);

        for (float t = 0; t < 0.1f; t += Time.deltaTime)
        {
            _partPoints.localScale = Vector3.Lerp(_partPoints.localScale, localScale, t);
            _body.localScale = Vector3.Lerp(_body.localScale, localScale, t);
            _collider.localScale = Vector3.Lerp(_collider.localScale, colliderLcalScale, t);

            yield return null;
        }

        _partPoints.localScale = localScale;
        _body.localScale = localScale;
        _collider.localScale = colliderLcalScale;

        _isActiveCoroutine = false;
    }

}

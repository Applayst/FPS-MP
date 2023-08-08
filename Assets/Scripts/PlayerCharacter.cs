using System;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _partPoints;
    [SerializeField] private Transform _collider;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private CheckFly _checkFly;
        
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpTimeDelay;
    [SerializeField] private float _minVerticalAngles;
    [SerializeField] private float _maxVerticalAngles;
    
    private float _inputH;
    private float _inputV;
    private float _rotateY;
    private bool _isSit;
    private float _currentRotateX;
    private float _jumpTime  = 0;
    

    private void Start()
    {
        Transform camera = Camera.main.transform;
        camera.parent = _cameraPoint;
        camera.localPosition = Vector3.zero;
        camera.localRotation = Quaternion.identity;        
    }
    public void SetInput(float h, float v, float rotateY)
    {
        _inputH = h;
        _inputV = v;
        _rotateY += rotateY;
    }

    public void RotateX(float value)
    {
        _currentRotateX = Math.Clamp(_currentRotateX + value, _minVerticalAngles, _maxVerticalAngles);
        _head.localEulerAngles = new Vector3(_currentRotateX, 0, 0);
    }

    public void SitDown()
    {
        Vector3 localScale = new Vector3(1f, SitMultiplier, 1f);
        _partPoints.localScale = localScale;
        _body.localScale = localScale;
        _collider.localScale = new Vector3(1f, SitMultiplier * 0.6f + 0.4f, 1f);
        _isSit = true;
    }

    public void StandUp()
    {
        _partPoints.localScale = Vector3.one;
        _body.localScale = Vector3.one;
        _collider.localScale = Vector3.one;
        _isSit = false;
    }

    public void Jump()
    {
        if (_checkFly.IsFly || ((Time.time - _jumpTime) < _jumpTimeDelay))
            return;

        _jumpTime = Time.time;
        _rigidBody.AddForce(0, _jumpForce, 0, ForceMode.VelocityChange);
    }

    void FixedUpdate()
    {
        Move();
        RotateY();
    }

    private void RotateY()
    {
        _rigidBody.angularVelocity = new Vector3(0, _rotateY, 0);
        _rotateY = 0;
    }

    private void Move()
    {
        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * Speed;
        velocity.y = _rigidBody.velocity.y;
        Velocity = velocity;
        _rigidBody.velocity = Velocity;
    }

    public void GetPosition(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY, out bool isSit)
    {
        position = transform.position;
        velocity = _rigidBody.velocity;

        rotateY = transform.eulerAngles.y;
        rotateX = _head.localEulerAngles.x;

        isSit = _isSit;
    }

}

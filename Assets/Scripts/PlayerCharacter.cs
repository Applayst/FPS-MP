using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private float _speed = 1f;

    private float _inputH;
    private float _inputV;

    public void SetInput(float h, float v)
    {
        _inputH = h;
        _inputV = v;
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        //Vector3 direction = new Vector3(_inputH, 0,_inputV).normalized;
        //transform.position += direction * Time.deltaTime * _speed;

        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * _speed;
        _rigidBody.velocity = velocity;

    }

    public void GetPosition(out Vector3 position, out Vector3 velocity)
    {
        position = transform.position;
        velocity = _rigidBody.velocity;
    }

}

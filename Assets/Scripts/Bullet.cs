using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _lifeTime;
    public void Init(Vector3 direction)
    {
        _rigidbody.velocity = direction;

        StartCoroutine(DestroyDelay());
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSecondsRealtime(_lifeTime);
        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy();
    }
}

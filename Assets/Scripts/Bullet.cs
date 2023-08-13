using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _lifeTime;

    private int _damage;
    public void Init(Vector3 direction, int damageValue = 0)
    {
        _rigidbody.velocity = direction;
        _damage = damageValue;

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
        if (collision.collider.transform.parent.TryGetComponent(out EnemyCharacter enemyCharacter))
        {
            enemyCharacter.ApplyDamage(_damage);
        }
        
        Destroy();
    }
}

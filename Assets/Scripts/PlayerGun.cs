using UnityEngine;

public class PlayerGun : Gun
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _shootDelay;
    
    private float _lastShotTime;

    public bool TryShoot(out ShootInfo info)
    {
        info = new ShootInfo();
        if ((Time.time - _lastShotTime) < _shootDelay) return false;

        _lastShotTime = Time.time;

        Vector3 position = _spawnPoint.position;
        Vector3 velocity = _spawnPoint.forward * _bulletSpeed;
        Instantiate(_bulletPrefab, position, _spawnPoint.rotation).Init(velocity);

        OnShoot?.Invoke();

        info.pX = position.x;
        info.pY = position.y;
        info.pZ = position.z;
        info.dX = velocity.x;
        info.dY = velocity.y;
        info.dZ = velocity.z;

        return true;
    }
}

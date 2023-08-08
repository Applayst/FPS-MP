using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyCharacter _enemy;
    [SerializeField] private EnemyGun _enemyGun;

    private List<float> _lastTimes = new List<float> { 0, 0, 0, 0, 0 };
    private float _lastRecivedTime = 0;

    private Player _player;

    public void Init(Player player)
    {
        _player = player;
        _enemy.SetSpeed(_player.speed);
        _enemy.SetSitMultiplier(_player.sitM);
        _player.OnChange += OnChange;
    }

    public void Shoot(in ShootInfo shootInfo)
    {
        Vector3 position = new Vector3(shootInfo.pX, shootInfo.pY, shootInfo.pZ);
        Vector3 velocity = new Vector3(shootInfo.dX, shootInfo.dY, shootInfo.dZ);
        _enemyGun.Shoot(position, velocity);
    }
    public void Destroy()
    {
        _player.OnChange -= OnChange;
        Destroy(gameObject);
    }

    private float AverageInterval
    {
        get
        {            
            float summ = 0;
            for (int i = 0; i < _lastTimes.Count; i++)
            {
                summ += _lastTimes[i];
            }
            return summ / _lastTimes.Count;
        }
    }

    private void SaveTime()
    {
        float interval = Time.time - _lastRecivedTime;
        _lastRecivedTime = Time.time;

        _lastTimes.Add(interval);
        _lastTimes.Remove(0);
    }

    internal void OnChange(List<DataChange> changes)
    {
        SaveTime();

        Vector3 position = _enemy.TargetPosition;
        Vector3 velocity = _enemy.Velocity;

        foreach (var dataChange in changes)
        {
            switch (dataChange.Field)
            {
                case "pX":
                    position.x = (float)dataChange.Value;
                    break;

                case "pY":
                    position.y = (float)dataChange.Value;
                    break;

                case "pZ":
                    position.z = (float)dataChange.Value;
                    break;

                case "vX":
                    velocity.x = (float)dataChange.Value;
                    break;

                case "vY":
                    velocity.y = (float)dataChange.Value;
                    break;

                case "vZ":
                    velocity.z = (float)dataChange.Value;
                    break;

                case "rX":
                    _enemy.SetRotateX((float)dataChange.Value);
                    break;

                case "rY":
                    _enemy.SetRotateY((float)dataChange.Value);
                    break;

                case "s":
                    if ((bool)dataChange.Value)
                    {
                        _enemy.SitDown();
                    }
                    else
                    {
                        _enemy.StandUp();
                    }
                    break;

                default:
                    Debug.LogWarning("Changes not apply:" + dataChange.Field);
                    break;
            }
        }

        _enemy.SetMovement(position, velocity, AverageInterval);
    }
}

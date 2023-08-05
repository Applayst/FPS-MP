using Colyseus.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    private List<float> _lastTimes = new List<float> { 0, 0, 0, 0, 0 };
    private float _lastRecivedTime = 0;

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

        Vector3 position = _enemy.TargetPosition; //transform.position;
        Vector3 velocity = Vector3.zero;

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

                default:
                    Debug.LogWarning("Changes not apply:" + dataChange.Field);
                    break;
            }
        }

        _enemy.SetMovement(position, velocity, AverageInterval);
    }
}

using Colyseus.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    internal void OnChange(List<DataChange> changes)
    {
        Vector3 position = transform.position;

        foreach (var dataChange in changes)
        {
            switch (dataChange.Field)
            {
                case "x":
                    position.x = (float)dataChange.Value;
                    break;

                case "y":
                    position.z = (float)dataChange.Value;
                    break;

                default:
                    Debug.LogWarning("Changes not apply:" + dataChange.Field);
                    break;
            }
        }

        _enemy.SetPosition(position);
    }
}

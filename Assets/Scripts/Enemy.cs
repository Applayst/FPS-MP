using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}

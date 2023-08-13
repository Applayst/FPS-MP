using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _camera;

    void Start()
    {
        _camera = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(_camera);
    }
}

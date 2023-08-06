using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    private const string shoot = "Shoot";

    [SerializeField] private Gun _playerGun;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _playerGun.OnShoot += Shoot;
    }

    private void Shoot()
    {
        _animator.SetTrigger(shoot);
    }

    private void OnDestroy()
    {
        _playerGun.OnShoot -= Shoot;
    }
}

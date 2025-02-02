using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Fire : InputBase
{
    [SerializeField] private Aim _aim;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _span = 0.05f;
    [SerializeField] private float _lifeTime = 5f;

    private Camera _camera;
    private float _fireTimer;
    private bool _isFire;

    protected override void Start()
    {
        base.Start();

        _camera = Camera.main;
        _inputReader.PrimaryAttackEvent += OnPrimaryFireHandle;
   
    }

    private void Update()
    {
        if(_span > 0) _fireTimer -= Time.deltaTime;

        if (_isFire && _aim.IsAiming.Value)
        {
            if (_fireTimer <= 0)
            {
                _fireTimer = _span;
                var bullet = Instantiate(_bulletPrefab);
                bullet.transform.rotation = _camera.transform.rotation;
                bullet.transform.position = _muzzle.position;
                Destroy(bullet, _lifeTime);
            }
        }
    }
    private void OnPrimaryFireHandle(bool input)
    {
        _isFire = input;
    }
}
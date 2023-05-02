using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Overpass : MonoBehaviour
{
    private int _numDestr = 0;
    private Animator _animator;
    [SerializeField] private GameObject _makeshiftCollider;
    private CinemachineImpulseSource _source;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _source = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            _source.GenerateImpulse();
        }
    }

    public void ShakeScreen()
    {
        
        _source.GenerateImpulse();
        Taptic.Heavy();
    }
    public void SupportDestroyed()
    {
        _numDestr++;
        if (_numDestr == 2)
        {
            _animator.SetTrigger("Fall");
            _makeshiftCollider.SetActive(true);
            Taptic.Warning();
        }
    }
    
}
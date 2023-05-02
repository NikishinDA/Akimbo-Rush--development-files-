using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletMS : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private ForceMode _forceMode;
    [SerializeField] private float _lifeTime;
    [HideInInspector] public float inheritedSpeed = 0;
    [HideInInspector] public bool targetHit = false;

    [SerializeField] private GameObject _bulletDecal;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody.AddForce(transform.forward*(_speed + inheritedSpeed), _forceMode);
        StartCoroutine(LifeTimer(_lifeTime));
    }


    private IEnumerator LifeTimer(float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            yield return null;
        }
        Destroy(gameObject);
    }

    //temp
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 8)//enemy
            Instantiate(_bulletDecal, transform.position, Quaternion.identity).transform.forward = other.transform.forward;
        Destroy(gameObject);
    }
}


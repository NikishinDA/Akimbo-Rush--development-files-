using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BridgeButton : MonoBehaviour
{
    [SerializeField] private Transform _bridge;
    [SerializeField] private float _lowerTime;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private Material _pressedMaterial;
    private CinemachineImpulseSource _source;

    private void Awake()
    {
        _source = GetComponent<CinemachineImpulseSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        StartCoroutine(LowerBridge());
        GetComponent<Collider>().enabled = false;
        _effect.Play();
        transform.localPosition += Vector3.back * 0.1f;
        GetComponent<Renderer>().material = _pressedMaterial;
    }

    private IEnumerator LowerBridge()
    {
        Quaternion initRotation = _bridge.rotation;
        Quaternion endRot = Quaternion.Euler(-90, initRotation.eulerAngles.y, initRotation.eulerAngles.z);
        for (float t = 0; t < _lowerTime; t += Time.deltaTime)
        {
            _bridge.rotation = Quaternion.Lerp(initRotation, endRot, t/_lowerTime);
            yield return null;
        }
        _bridge.rotation = endRot;
        Taptic.Heavy();
        _source.GenerateImpulse();
    }
}

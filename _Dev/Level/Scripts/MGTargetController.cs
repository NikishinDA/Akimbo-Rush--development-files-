using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MGTargetController : MonoBehaviour
{
    [SerializeField] private int _multiplier;
    [SerializeField] private GameObject[] _pieces;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _scatterForce;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            BulletMS bms = other.GetComponent<BulletMS>();
            if (!bms.targetHit)
            {
                var evt = GameEventsHandler.TargetDestroyedEvent;
                evt.Multiplier = _multiplier;
                EventManager.Broadcast(evt);
                DestroyTarget();
                bms.targetHit = true;
            }
        }
    }

    private void DestroyTarget()
    {
        GetComponent<Collider>().enabled = false;
        _target.SetActive(false);
        foreach (var piece in _pieces)
        {
            piece.SetActive(true);
            Rigidbody pieceRB = piece.GetComponent<Rigidbody>();
            pieceRB.isKinematic = false;
            pieceRB.useGravity = true;
            pieceRB.AddForce(_scatterForce * Random.insideUnitSphere, ForceMode.Impulse);
        }

        StartCoroutine(LifeTime(3f));
    }

    private IEnumerator LifeTime(float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            yield return null;
        }
        foreach (var piece in _pieces)
        {
            Destroy(piece);
        }
    }
}

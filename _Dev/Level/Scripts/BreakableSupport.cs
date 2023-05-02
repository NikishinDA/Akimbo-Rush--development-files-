using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableSupport : MonoBehaviour
{
    [SerializeField] private Overpass _overpass;
    [SerializeField] private GameObject _brokenColumn;
    [SerializeField] private GameObject[] _columnPieces;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private float _scatterForce;
    private void OnCollisionEnter(Collision other)
    {
        _overpass.SupportDestroyed();
        GetComponent<Collider>().enabled = false;
        //_brokenColumn.SetActive(false);
        foreach (var piece in _columnPieces)
        {
            Rigidbody rb = piece.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Random.insideUnitSphere * _scatterForce, ForceMode.Impulse);
        }
        _effect.Play();
        Taptic.Medium();
        StartCoroutine(ShardsLifeTime(1f));
    }
    private IEnumerator ShardsLifeTime(float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            yield return null;
        }

        foreach (var piece in _columnPieces)
        {
            Destroy(piece);
        }
    }
}

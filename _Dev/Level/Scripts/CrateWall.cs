using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateWall : Wall
{
    [SerializeField] private GameObject[] _shards;
    [SerializeField] private GameObject[] _crates;
    [SerializeField] private Collider _punchCollider;
    [SerializeField] private float _scatterForce = 1f;
    protected override void DestroyEffect()
    {
        Taptic.Heavy();
        foreach (var crate in _crates)
        {
            crate.SetActive(false);
        }
        foreach (var shard in _shards)
        {
            shard.SetActive(true);
            shard.transform.SetParent(null);
            shard.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * _scatterForce, ForceMode.Impulse);
        }

        StartCoroutine(LiftCor(_liftTime, _liftHeight));
        StartCoroutine(ShardsLifeTime(_liftTime));
    }

    protected override void AffectPlayer()
    {
        _punchCollider.enabled = true;
        EventManager.Broadcast(GameEventsHandler.PlayerInstantDeathEvent);
    }

    private IEnumerator ShardsLifeTime(float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            yield return null;
        }

        foreach (var shard in _shards)
        {
            Destroy(shard);
        }
    }
}

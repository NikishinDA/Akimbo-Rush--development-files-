using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassWall : Wall
{
    [SerializeField] private GameObject[] _shards; //0,1 - corners
    [SerializeField] private GameObject _wall;

    [SerializeField] private float _scatterForce = 1f;
    protected override void DestroyEffect()
    {
        Taptic.Heavy();
        _wall.SetActive(false);
        foreach (var shard in _shards)
        {
            shard.SetActive(true);
        }

        for (int i = 2; i < _shards.Length; i++)
        {
            GameObject shard = _shards[i];
            //shard.transform.SetParent(null);
            shard.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * _scatterForce, ForceMode.Impulse);
        }

        StartCoroutine(LiftCor(_liftTime, _liftHeight));
    }
}

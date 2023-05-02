using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Drone : MonoBehaviour
{
    [SerializeField] private float _highHeight = 6f;
    [SerializeField] private float _medHeight = 5f;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private CoinController _coinPrefab;
    private void OnCollisionEnter(Collision other)
    {
        Explode();
    }

    public void Explode()
    {
        var evt = GameEventsHandler.EnemyKilledEvent;
        evt.Type = ObstacleType.Drone;
        EventManager.Broadcast(evt);
        gameObject.SetActive(false);
        Instantiate(_effect, transform.position, Quaternion.identity);
        CoinController go = Instantiate(_coinPrefab, transform.position + Vector3.up, Quaternion.identity);
        go.gameObject.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * 5f,ForceMode.Impulse);
        Taptic.Heavy();
    }

    public void Initialize(PositionEnum pos)
    {
        float newHeight = 0;
        if (pos <= PositionEnum.FrontRight)
        {
            newHeight = _highHeight;
        }
        else if (pos <= PositionEnum.CenterRight)
        {
            newHeight = _medHeight;
        }
        else
        {
            newHeight = _medHeight;
        }
        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);

        
    }
}

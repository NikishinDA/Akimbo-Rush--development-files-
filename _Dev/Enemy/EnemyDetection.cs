using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private EnemyController _enemyController;
    [SerializeField] private bool _closeDetection;

    private void Awake()
    {
        if (!_enemyController)
        {
            transform.root.gameObject.GetComponent<EnemyController>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_closeDetection)
        {
            //_enemyController.Activate(false);
            _enemyController.FixPosition();
            _enemyController.gameObject.GetComponent<Animator>().SetTrigger("Hit");
            //EventManager.Broadcast(GameEventsHandler.PlayerTakeDamageEvent);
        }
        else
        {
            _enemyController.SetTarget(other.transform);
            _enemyController.Activate();
        }
    }
}

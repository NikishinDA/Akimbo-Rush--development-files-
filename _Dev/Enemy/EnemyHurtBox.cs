using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtBox : MonoBehaviour
{
    [SerializeField] private EnemyController _enemyController;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 6)//bullet
            _enemyController.OnShot();
        Taptic.Medium();
    }

}

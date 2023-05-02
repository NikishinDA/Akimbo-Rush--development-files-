using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDetection : MonoBehaviour
{
    [SerializeField] private CoinController _coinController;

    private void OnTriggerEnter(Collider other)
    {
        _coinController.SetTarget(other.transform);
    }
}

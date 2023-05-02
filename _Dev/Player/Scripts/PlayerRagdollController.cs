using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdollController : MonoBehaviour
{
    [SerializeField] private GameObject[] _parts;
    public void EnableRagdoll()
    {
        foreach (var part in _parts)
        {
            Rigidbody rb = part.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}

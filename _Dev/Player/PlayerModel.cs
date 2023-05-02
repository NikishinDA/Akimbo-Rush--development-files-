using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    
    [HideInInspector] public bool stopped;
    public BulletMS bulletPrefab;
    public float firingRate;
    public float movementSpeed;
    

    public float gravityModifier;
    public float jumpForce;

    public float boostSpeed;

    public int health;
    public int damagePerHit;
    
    public GameObject[] transports;

    public float animationOffset;

    public IKController ikController;
    public GameObject actionCamera;
}

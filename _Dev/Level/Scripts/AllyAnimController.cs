using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AllyAnimController : MonoBehaviour
{
    private void OnEnable()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                GetComponent<Animator>().SetTrigger("Clap");
                break;
            case 1:
                GetComponent<Animator>().SetTrigger("Chicken");
                break;
            case 2:
                GetComponent<Animator>().SetTrigger("Cheer");
                break;
        }
    }
}

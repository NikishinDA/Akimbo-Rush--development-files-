using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Wall : MonoBehaviour
{

    [SerializeField] protected float _liftTime = 3f;
    [SerializeField] protected float _liftHeight = 10f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           AffectPlayer();
        }
        DestroyWall();
    }

    protected virtual void AffectPlayer()
    {
        EventManager.Broadcast(GameEventsHandler.PlayerTakeDamageEvent);
    }
    private void DestroyWall()
    {
        GetComponent<Collider>().enabled = false;
        DestroyEffect();
    }

    protected abstract void DestroyEffect();

    protected IEnumerator LiftCor(float time, float height)
    {
        Vector3 initPos = transform.position;
        Vector3 endPos = initPos + Vector3.up * height;
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(initPos, endPos, t / time);
            yield return null;
        }
        Destroy(gameObject);
    }
}

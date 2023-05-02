using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{

    [HideInInspector] public Transform target;

    [SerializeField] private float _timeBeforePickUp = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        if (target)
            StartCoroutine(Timer(_timeBeforePickUp));
    }


    private IEnumerator Timer(float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            yield return null;
        }

        StartCoroutine(MoveCoin(1f));
    }

    public void SetTarget(Transform newTarget)
    {
        if (!newTarget) return;
        this.target = newTarget;
        StartCoroutine(Timer(_timeBeforePickUp));
    }
    private IEnumerator MoveCoin(float time)
    {
        Vector3 initPos = transform.position;
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(initPos, target.position + Vector3.up, t / time);
            yield return null;
        }
        EventManager.Broadcast(GameEventsHandler.CoinPickUpEvent);
        Destroy(gameObject);
    }
}

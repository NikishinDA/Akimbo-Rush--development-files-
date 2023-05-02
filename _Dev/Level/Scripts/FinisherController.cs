using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinisherController : MonoBehaviour
{
    [SerializeField] GameObject[] _targets;
    private IEnumerator _coroutine;
    [SerializeField] private int _tries = 3;
    private int _numShots = 0;
    private void Awake()
    {
        EventManager.AddListener<MinigameStartEvent>(OnMinigameStart);
        EventManager.AddListener<TargetDestroyedEvent>(OnTargetDestroyed);
        EventManager.AddListener<MinigamePlayerShotEvent>(OnPlayerShot);

        VarSaver.CoinMultiplier = 1;
        _numShots = 0;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<MinigameStartEvent>(OnMinigameStart);
        EventManager.RemoveListener<TargetDestroyedEvent>(OnTargetDestroyed);
        EventManager.RemoveListener<TargetDestroyedEvent>(OnTargetDestroyed);  
        EventManager.RemoveListener<MinigamePlayerShotEvent>(OnPlayerShot);

    }

    private void OnPlayerShot(MinigamePlayerShotEvent obj)
    {
        _numShots++;
        if (_numShots == _tries)
        {
            _coroutine = Timer(1f);
            StartCoroutine(_coroutine);
        }
    }

    private void OnMinigameStart(MinigameStartEvent obj)
    {
        foreach (var target in _targets)
        {
            target.SetActive(true);
        }
    }

    private void OnTargetDestroyed(TargetDestroyedEvent obj)
    {
        if (VarSaver.CoinMultiplier < obj.Multiplier)
        {
            VarSaver.CoinMultiplier = obj.Multiplier;
        }
        
        /*if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }*/
    }

    private IEnumerator Timer(float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            yield return null;
        }
        EventManager.Broadcast(GameEventsHandler.ShowWinPopUp);
    }
}

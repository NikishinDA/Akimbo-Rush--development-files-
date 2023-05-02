using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _loseScreen;
    [SerializeField] GameObject _winScreen;
    [SerializeField] GameObject _startScreen;
    [SerializeField] GameObject _overlay;
    [SerializeField] GameObject _transportScreen;
    [SerializeField] private GameObject _minigameUI;

    [SerializeField] private float _timeBeforeLoseScreen;
    private void Awake()
    {
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        EventManager.AddListener<GameStartEvent>(OnGameStart);
        EventManager.AddListener<MinigamePlayerInPositionEvent>(OnPlayerInPosition);
        EventManager.AddListener<ShowWinPopUp>(ShowWinPopUp);
        EventManager.AddListener<TransportUnlockEvent>(OnTransportUnlock);
    }
    private void OnDestroy()
    {
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        EventManager.RemoveListener<GameStartEvent>(OnGameStart);
        EventManager.RemoveListener<ShowWinPopUp>(ShowWinPopUp);
        EventManager.RemoveListener<TransportUnlockEvent>(OnTransportUnlock);
        EventManager.RemoveListener<MinigamePlayerInPositionEvent>(OnPlayerInPosition);


    }

    private void OnPlayerInPosition(MinigamePlayerInPositionEvent obj)
    {
        _overlay.SetActive(false);
        _minigameUI.SetActive(true);
    }

    private void OnTransportUnlock(TransportUnlockEvent obj)
    {
        _winScreen.SetActive(false);
        _transportScreen.SetActive(true);
    }

    private void Start()
    {
        _startScreen.SetActive(true);
    }
    private void OnGameOver(GameOverEvent obj)
    {
        _overlay.SetActive(false);
        if (!obj.IsWin)
        {
            IEnumerator coroutine = Timer(_timeBeforeLoseScreen);
            StartCoroutine(coroutine);
        }
        else
        {
            //_winScreen.SetActive(true);
        }
    }

    private void ShowWinPopUp(ShowWinPopUp obj)
    {
        _minigameUI.SetActive(false);
        _winScreen.SetActive(true);
    }
   /* private void OnMinigameToggle(MiniGameToggleEvent obj)
    {
        if (obj.IsStart)
        {
            _minigameUI.SetActive(true);
        }
        else
        {
            _minigameUI.SetActive(false);
            _winScreen.SetActive(true);
        }
    }*/
    
    private void OnGameStart(GameStartEvent obj)
    {
        _overlay.SetActive(true);
        
    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        
        _loseScreen.SetActive(true);
    }
}

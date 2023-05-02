using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{

    [SerializeField] private Button _startButton;
    [SerializeField] private Text _coinText;
    [SerializeField] private GameObject _startTutorial;

    private void Awake()
    {
        _startButton.onClick.AddListener(OnStartButtonClick);
        _coinText.text = PlayerPrefs.GetInt("TotalCoins", 0).ToString();
    }

    private void OnStartButtonClick()
    {
        EventManager.Broadcast(GameEventsHandler.GameStartEvent);
        gameObject.SetActive(false);
        _startTutorial.SetActive(true);
        // var evt = GameEventsHandler.DebugEvent;
        //Single.TryParse(_senseInput.text, out evt.Sense);
        // Single.TryParse(_speedInput.text, out evt.Speed);
        // Single.TryParse(_boostInput.text, out evt.Boost);
        // Single.TryParse(_angleInput.text, out evt.Angle);
        // Single.TryParse(_rateInput.text, out evt.Rate);
        // EventManager.Broadcast(evt);
    }

    
}

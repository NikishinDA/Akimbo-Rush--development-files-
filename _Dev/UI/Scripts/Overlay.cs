using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    [SerializeField] private int _levelLength;

    [SerializeField] private Slider _progressBar;
    [SerializeField] private GameObject[] _hearts;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _killText;
    [SerializeField] private Text _coinText;
    private float _progress;
    private int _missingHealth = 0;
    private int _bodyCount = 0;
    private int _coinCount = 0;
    private void Awake()
    {
        EventManager.AddListener<GameProgressEvent>(OnProgressUpdate);
        EventManager.AddListener<GameInitializeEvent>(OnInitialize);
        EventManager.AddListener<PlayerTakeDamageEvent>(OnPlayerTakeDamage);
        EventManager.AddListener<EnemyKilledEvent>(OnEnemyKilled);
        EventManager.AddListener<CoinPickUpEvent>(OnCoinPickUp);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<GameProgressEvent>(OnProgressUpdate);
        EventManager.RemoveListener<GameInitializeEvent>(OnInitialize);
        EventManager.RemoveListener<PlayerTakeDamageEvent>(OnPlayerTakeDamage);
        EventManager.RemoveListener<EnemyKilledEvent>(OnEnemyKilled);
        EventManager.RemoveListener<CoinPickUpEvent>(OnCoinPickUp);

    }

    private void OnCoinPickUp(CoinPickUpEvent obj)
    {
        _coinCount++;
        _coinText.text = _coinCount.ToString();
        VarSaver.CoinsCount = _coinCount;
    }

    private void OnEnemyKilled(EnemyKilledEvent obj)
    {
        _bodyCount++;
        _killText.text = _bodyCount.ToString();
       /* if (obj.Type == ObstacleType.Weak || obj.Type == ObstacleType.Drone)
        {
            _coinCount++;
        }
        else if(obj.Type == ObstacleType.Medium)
        {
            _coinCount += 2;
        }
        else if (obj.Type == ObstacleType.Strong)
        {
            _coinCount += 3;
        }*/

        VarSaver.BodyCount = _bodyCount;
    }

    private void OnPlayerTakeDamage(PlayerTakeDamageEvent obj)
    {
        if (_missingHealth < 3)
        {
            _hearts[_missingHealth].SetActive(false);
            _missingHealth++;
        }
    }

    private void OnInitialize(GameInitializeEvent obj)
    {
        _levelLength = obj.LevelLength;
        _levelText.text = "LVL " + PlayerPrefs.GetInt("Level", 1);
        _killText.text = _bodyCount.ToString();
        _coinText.text = _coinCount.ToString();
    }

    private void OnProgressUpdate(GameProgressEvent obj)
    {
        _progress += 1f / _levelLength;
    }

    // Update is called once per frame
    void Update()
    {
        _progressBar.value = Mathf.Lerp(_progressBar.value, _progress, Time.deltaTime);
    }
}

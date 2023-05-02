using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private Button _nextButton;
    [SerializeField] private Text _coinText;
    [SerializeField] private Text _killText;
    [SerializeField] private Slider _transportPB;
    [SerializeField] private float _progressPerLevel;
    [SerializeField] private Sprite[] _multiplierSprites;
    [SerializeField] private Image _multiplierImage;
    private void Awake()
    {
        _nextButton.onClick.AddListener(OnRestartButtonClick);
    }

    private void OnEnable()
    {
        _coinText.text = VarSaver.CoinsCount.ToString();
        _killText.text = VarSaver.BodyCount.ToString();
        if (VarSaver.CoinMultiplier > 1)
        {
            _multiplierImage.sprite = _multiplierSprites[VarSaver.CoinMultiplier - 2];
        }
        else
        {
            _multiplierImage.gameObject.SetActive(false);
        }
        StartCoroutine(CoinTicker());
        StartCoroutine(TransportProgress(2.5f));
    }

    private void OnRestartButtonClick()
    {
        int level = PlayerPrefs.GetInt("Level", 1) + 1;
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins",0) + VarSaver.CoinMultiplier * VarSaver.CoinsCount);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator CoinTicker()
    {
        int coinCount = VarSaver.CoinsCount;
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < VarSaver.CoinsCount * (VarSaver.CoinMultiplier - 1); i++)
        {
            coinCount++;
            _coinText.text = coinCount.ToString();
            yield return null;
        }
    }

    private IEnumerator TransportProgress(float time)
    {
        float progress = PlayerPrefs.GetFloat("Transport", 1);
        PlayerPrefs.SetFloat("Transport", progress + _progressPerLevel);
        if (progress >= 300) yield break;
        progress %= 100;
        float nextValue = progress + _progressPerLevel;
        if (nextValue > 100)
        {
            nextValue = 100;
            _nextButton.onClick.RemoveListener(OnRestartButtonClick);
            _nextButton.onClick.AddListener(ShowTransportUpdate);
        }

        progress /= 100;
        nextValue /= 100;
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            _transportPB.value = Mathf.Lerp(progress, nextValue, t / time);
            yield return null;
        } 
    }

    private void ShowTransportUpdate()
    {
        EventManager.Broadcast(GameEventsHandler.TransportUnlockEvent);
    }
    
}

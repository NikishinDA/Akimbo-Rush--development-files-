using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTutorial : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private GameObject _tutor;

    private void Awake()
    {
        _startButton.onClick.AddListener(OnStartButtonClick);
        
    }

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("Level", 1) == 1)
        {
            StartCoroutine(TutorialScreenWait(1f));
        }
    }

    private void OnStartButtonClick()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    private IEnumerator TutorialScreenWait(float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            yield return null;
        }
        _tutor.SetActive(true);
        _startButton.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}

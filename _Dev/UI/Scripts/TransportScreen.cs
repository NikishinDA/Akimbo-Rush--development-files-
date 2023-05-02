using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransportScreen : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Button _nextButton;
    private void Awake()
    {
        _nextButton.onClick.AddListener(OnNextButtonClick);
    }

    private void OnNextButtonClick()
    {
        int level = PlayerPrefs.GetInt("Level", 1) + 1;
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        SetImage();
    }

    private void SetImage()
    {
        int progress = (int) (PlayerPrefs.GetFloat("Transport", 1) / 100);
        switch (progress)
        {
            case 1:
            {
                _image.sprite = _sprites[0];
            }
                break;
            case 2:
            {
                _image.sprite = _sprites[1];
            }
                break;
            case 3:
            {
                _image.sprite = _sprites[2];
            }
                break;
        }
    }
    
}

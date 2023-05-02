using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameScreen : MonoBehaviour
{
   [SerializeField] private GameObject[] _bullets;
   [SerializeField] private Text _coinText;
   [SerializeField] private Image _multiplierImage;
   [SerializeField] private Sprite[] _multiplierSprites;
   private int _numShots;
   private int _multiplier = 1;
   private void Awake()
   {
      EventManager.AddListener<MinigamePlayerShotEvent>(OnPlayerShot);
      EventManager.AddListener<TargetDestroyedEvent>(OnTargetDestroyed);
   }

   private void OnDestroy()
   {
      EventManager.RemoveListener<MinigamePlayerShotEvent>(OnPlayerShot);
      EventManager.RemoveListener<TargetDestroyedEvent>(OnTargetDestroyed);

   }

   private void OnTargetDestroyed(TargetDestroyedEvent obj)
   {
      if (VarSaver.CoinMultiplier > _multiplier)
      {
         _multiplierImage.gameObject.SetActive(true);
         _multiplierImage.sprite = _multiplierSprites[VarSaver.CoinMultiplier - 2];
         _multiplierImage.GetComponent<Animator>().SetTrigger("Bob");
         _multiplier = VarSaver.CoinMultiplier;
      }
   }

   private void OnEnable()
   {
      _coinText.text = VarSaver.CoinsCount.ToString();
   }

   private void OnPlayerShot(MinigamePlayerShotEvent obj)
   {
      if (_numShots < _bullets.Length)
         _bullets[_numShots].SetActive(false);
      _numShots++;
   }
}

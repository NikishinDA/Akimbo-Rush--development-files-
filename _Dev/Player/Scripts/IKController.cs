using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKController : MonoBehaviour
{
    [SerializeField] private Transform _rightHand;
    [SerializeField] private Transform _rightGun;
    [SerializeField] private Transform _leftGun;
    [SerializeField] [Range(0, 1)] private float _weight = 0f;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
        EventManager.AddListener<GameStartEvent>(OnGameStart);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<GameStartEvent>(OnGameStart);

    }

    private void OnGameStart(GameStartEvent obj)
    {
        _weight = 1f;
    }

    public void ActivateIK()
    {
        StartCoroutine(Activation(1f));
    }

    private IEnumerator Activation(float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            _weight = Mathf.Lerp(0, 1, t / time);
            yield return null;
        }

        _weight = 1;
    }

    public void DeactivateIK(float time)
    {
        StartCoroutine(Deactivation(time));
    }
    private IEnumerator Deactivation(float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            _weight = Mathf.Lerp(1, 0, t / time);
            yield return null;
        }

        _weight = 0;
    }
    public void SetWeight(float weight)
    {
        _weight = weight;
    }
    
    
    private void OnAnimatorIK(int layerIndex)
    {
       _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _weight);
       _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, _weight);
       _animator.SetIKPosition(AvatarIKGoal.RightHand, _rightGun.position);
       _animator.SetIKRotation(AvatarIKGoal.RightHand, _rightGun.rotation);
       _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _weight);
       _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, _weight);
       _animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftGun.position);
       _animator.SetIKRotation(AvatarIKGoal.LeftHand, _leftGun.rotation);
    }
}

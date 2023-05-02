using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MGTargetMover : MonoBehaviour
{
   [SerializeField] private float _speed;
   [SerializeField] private float _desiredHeight;
   [SerializeField] private float _moveConstraint;
   private bool _ready;
   private Rigidbody _rb;
   private float _curSpeed;
   private void Awake()
   {
      _rb = GetComponent<Rigidbody>();
      _ready = false;
      _curSpeed = _speed;
      transform.position += Vector3.right * Random.Range(-_moveConstraint, _moveConstraint);
   }

   

   private void OnEnable()
   {
      StartCoroutine(Deploy(2f));
      if (Random.value < 0.5)
      {
         _curSpeed *= -1;
      }
   }

   private IEnumerator Deploy(float time)
   {
      Vector3 initPos = transform.localPosition;
      Vector3 endPos = new Vector3(initPos.x,_desiredHeight, initPos.z);
      for (float t = 0; t < time; t += Time.deltaTime)
      {
         transform.localPosition = Vector3.Lerp(initPos, endPos, t / time);
         yield return null;
      }

      _ready = true;
   }

   private void FixedUpdate()
   {
      if (_ready)
      {
         _rb.MovePosition(_rb.position + Vector3.right * (_curSpeed * Time.deltaTime));
         if (_rb.position.x > _moveConstraint)
         {
            _curSpeed = -_speed;
         }

         if (_rb.position.x < -_moveConstraint)
         {
            _curSpeed = _speed;
         }
      }
   }
}

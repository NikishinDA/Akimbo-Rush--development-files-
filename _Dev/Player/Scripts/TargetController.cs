using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField] private float _movementScale;
    [SerializeField] private float _touchScale;
    [SerializeField] private float _movementConstraint;
    private Vector3 _initialTargetPosition;

    private bool _move;
    // Start is called before the first frame update

    private void Awake()
    {
        EventManager.AddListener<DebugEvent>(OnDebugCall);
        EventManager.AddListener<MinigameStartEvent>(OnMinigameStart);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<DebugEvent>(OnDebugCall);
        EventManager.RemoveListener<MinigameStartEvent>(OnMinigameStart);
    }

    private void OnMinigameStart(MinigameStartEvent obj)
    {
        _move = false;
        StartCoroutine(MoveToCenter());
    }
    private void OnDebugCall(DebugEvent obj)
    {
        _touchScale = obj.Sense;
        var transform1 = transform;
        var position = transform1.position;
        position = new Vector3(position.x, obj.Angle, position.z);
        transform1.position = position;
    }

    void Start()
    {
        _initialTargetPosition = transform.localPosition;
        _move = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_move &&Input.GetMouseButton(0))
        {
            MoveTarget();
        }
    }
    private void MoveTarget()
    {
        float newPosX = Input.GetAxis("Mouse X");
        if (Input.touchCount > 0)
        {
            newPosX = (Input.touches[0].deltaPosition.x / Screen.width) * _touchScale;
        }

        newPosX *= _movementScale * Time.deltaTime;
        if (!(Math.Abs(transform.localPosition.x + newPosX - _initialTargetPosition.x) > _movementConstraint))
        {
            transform.Translate(newPosX * (1/(Time.timeScale == 0 ? 1 : Time.timeScale)), 0, 0);
        }
    }

    private IEnumerator MoveToCenter()
    {
        while (transform.localPosition.x > -_movementConstraint + _initialTargetPosition.x)
        {
            transform.Translate(-Time.deltaTime, 0,0);
            yield return null;
        }
        transform.localPosition = new Vector3(-_movementConstraint + _initialTargetPosition.x, transform.localPosition.y, transform.localPosition.z);
    }
}

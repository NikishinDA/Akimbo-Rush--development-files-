using System;
using System.Collections;
using System.Collections.Generic;
using AnimeFX.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerModel _model;
    [SerializeField] private PlayerView _view;
    
    private Vector3 _initialTargetPosition;
    private CharacterController _characterController;
    
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private bool _minigame;
    private float _speed;
    private IEnumerator _boostCoroutine;

    private LineRenderer _leftLineRenderer;
    private LineRenderer _rightLineRenderer;

    private bool _stopped;
    private bool _minigameShot = false;
    private int _minigameTimesShot;
    private GameObject _transport;
    private IEnumerator _fireCor;
    private float _addSpeed = 0f;
    private float _addBoostSpeed = 0f;
    private void Awake()
    {
        _characterController = _view.GetComponent<CharacterController>();
        
        EventManager.AddListener<RampJumpEvent>(OnRampJump);
        EventManager.AddListener<PlayerTakeDamageEvent>(OnPlayerTakeDamage);
        EventManager.AddListener<PlayerBoostEvent>(OnPlayerBoost);
        EventManager.AddListener<PlayerInstantDeathEvent>(OnInstantDeath);
        EventManager.AddListener<GameStartEvent>(OnGameStart);
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        EventManager.AddListener<DebugEvent>(OnDebugCall);
        EventManager.AddListener<MinigameStartEvent>(OnMinigameStart);
        EventManager.AddListener<MinigamePlayerInPositionEvent>(OnPlayerInPositionEvent);


        _leftLineRenderer = _view.leftGun.GetComponent<LineRenderer>();
        _rightLineRenderer = _view.rightGun.GetComponent<LineRenderer>();

        _stopped = true;
        _minigame = false;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<RampJumpEvent>(OnRampJump);
        EventManager.RemoveListener<PlayerTakeDamageEvent>(OnPlayerTakeDamage);
        EventManager.RemoveListener<PlayerBoostEvent>(OnPlayerBoost);
        EventManager.RemoveListener<PlayerInstantDeathEvent>(OnInstantDeath);
        EventManager.RemoveListener<GameStartEvent>(OnGameStart);
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        EventManager.RemoveListener<DebugEvent>(OnDebugCall);
        EventManager.RemoveListener<MinigameStartEvent>(OnMinigameStart);
        EventManager.RemoveListener<MinigamePlayerInPositionEvent>(OnPlayerInPositionEvent);
    }

    private void Start()
    {
        SetTransport();
    }

    private void SetTransport()
    {
        int progress = (int) (PlayerPrefs.GetFloat("Transport", 1) / 100);
        if (progress > 3) progress = Random.Range(0, 4);
        switch (progress)
        {
            case 1:
            {
                _model.transports[0].SetActive(true);
                _transport =  _model.transports[0];
                _view.player3DModel.GetComponent<Animator>().SetTrigger("Gyro");
                _model.movementSpeed += 1;
                _model.boostSpeed += 2;
                _addSpeed = 1;
                _addBoostSpeed = 2;
            }
                break;
            case 2:
            {
                _model.transports[1].SetActive(true);
                _transport =  _model.transports[1];
                _view.player3DModel.GetComponent<Animator>().SetTrigger("Wheel");
                _model.movementSpeed += 2;
                _model.boostSpeed += 4;
                _addSpeed = 2;
                _addBoostSpeed = 4;
            }
                break;
            case 3:
            {
                _model.transports[2].SetActive(true);
                _transport =  _model.transports[2];
                _view.player3DModel.GetComponent<Animator>().SetTrigger("Hover");
                _model.movementSpeed += 3;
                _model.boostSpeed += 6;
                _addSpeed = 3;
                _addBoostSpeed = 6;
            }
                break;
            default:
            {
                _view.player3DModel.GetComponent<Animator>().SetTrigger("Legs");
                _view.player3DModel.transform.position += Vector3.down * 0.15f;
               _view.guns.transform.position += Vector3.down * 0.2f;
               _view.target.transform.position += Vector3.down * 0.2f;
            }
                break;
        }
    }

    private void OnPlayerInPositionEvent(MinigamePlayerInPositionEvent obj)
    {
        _stopped = true;
        _minigame = true;
        _view.player3DModel.GetComponent<Animator>().SetTrigger("Minigame");
    }

    private void OnMinigameStart(MinigameStartEvent obj)
    {
        _minigameShot = false;
        _minigameTimesShot = 0;
        //_view.target.transform.localPosition += Vector3.up * _model.animationOffset;
        //_view.guns.transform.localPosition += Vector3.up * _model.animationOffset;
        FallFx.Instance.SetFxIntensity(0);
        StopAllCoroutines();
    }

    private void OnGameOver(GameOverEvent obj)
    {
        if (!obj.IsWin)
            _stopped = true;
    }

    private void OnDebugCall(DebugEvent obj)
    {
        _model.movementSpeed = obj.Speed;
        _speed = obj.Speed;
        _model.boostSpeed = obj.Boost;
        _model.firingRate = obj.Rate;
    }

    private void OnGameStart(GameStartEvent obj)
    {
        _stopped = false;
        _view.player3DModel.GetComponent<Animator>().SetTrigger("Start");
        StartCoroutine(StartSpeedCor(1f));
        _fireCor = FiringCoroutine(_model.firingRate);
        StartCoroutine(_fireCor);
        _model.ikController.ActivateIK();
    }

    private void OnInstantDeath(PlayerInstantDeathEvent obj)
    {
        PlayerDeath();
    }

    private void OnPlayerBoost(PlayerBoostEvent obj)
    {
        if (_boostCoroutine != null)
        {
            StopCoroutine(_boostCoroutine);
        }

        _boostCoroutine = BoostCor(2f,1f);
        StartCoroutine(_boostCoroutine);
    }

    private void OnPlayerTakeDamage(PlayerTakeDamageEvent obj)
    {
        //Debug.Log("hit");
        _model.health -= _model.damagePerHit;
        Taptic.Heavy();
        _view.hitEffect.Play();
        _view.player3DModel.GetComponent<Animator>().SetTrigger("Hit");
        _view.player3DModel.GetComponent<Animator>().ResetTrigger("Start");
        _view.target.transform.localPosition += Vector3.down * _model.animationOffset;
        _view.guns.transform.localPosition += Vector3.down * _model.animationOffset;
        _model.boostSpeed -= _addBoostSpeed;
        _model.movementSpeed -= _addSpeed;
        _speed -= _addSpeed;
        if (_transport)
        {
            _transport.transform.SetParent(null);
            _view.player3DModel.transform.position += Vector3.down * 0.15f;
        }

        if (_model.health <= 0 && !_stopped)
        {
            Debug.Log("dead");
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        Time.timeScale = 1;
        Taptic.Failure();
        _stopped = true;
        StopAllCoroutines();
        _view.player3DModel.GetComponent<Animator>().enabled = false;
        _view.player3DModel.GetComponent<PlayerRagdollController>().EnableRagdoll();
        _view.gameObject.layer = 0;
        _view.leftColt.transform.SetParent(null);
        _view.leftColt.GetComponent<Collider>().enabled = true;
        _view.leftColt.GetComponent<Rigidbody>().isKinematic = false;
        _view.leftColt.GetComponent<Rigidbody>().useGravity = true;
        _view.leftColt.GetComponent<Animator>().enabled = false;
        _view.leftColt.GetComponent<Rigidbody>().velocity = _speed * Vector3.forward + Random.insideUnitSphere;
        _view.rightColt.transform.SetParent(null);
        _view.rightColt.GetComponent<Collider>().enabled = true;
        _view.rightColt.GetComponent<Rigidbody>().isKinematic = false;
        _view.rightColt.GetComponent<Rigidbody>().useGravity = true;
        _view.rightColt.GetComponent<Animator>().enabled = false;
        _view.rightColt.GetComponent<Rigidbody>().velocity = _speed * Vector3.forward + Random.insideUnitSphere;
        EventManager.Broadcast(GameEventsHandler.PlayerDeadEvent);
        var evt = GameEventsHandler.GameOverEvent;
        evt.IsWin = false;
        EventManager.Broadcast(evt);
        _speed = 0;
    }
    private void OnRampJump(RampJumpEvent obj)
    {
       _view.player3DModel.GetComponent<Animator>().SetTrigger("Dive");
       _model.actionCamera.SetActive(true);
        Jump();
        StartCoroutine(JumpCor());
    }

    private IEnumerator JumpCor()
    {
        //yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0.25f;
        yield return new WaitForSeconds(5f/6);
        StopCoroutine(_fireCor);
        yield return new WaitForSeconds(1f);
        StartCoroutine(_fireCor);
        Time.timeScale = 1;
        _model.actionCamera.SetActive(false);
    }
    private void Update()
    {
        groundedPlayer = _characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

       /* if (Input.GetKey(KeyCode.Alpha1) && groundedPlayer)
        {
            Jump();
        }*/
        
        RotateGuns();
        
        playerVelocity.y += -_model.gravityModifier * Time.deltaTime;
        if(!_stopped)
            _characterController.Move((Vector3.forward * _speed + playerVelocity) * Time.deltaTime);

        if (_minigame && !_minigameShot && Input.GetMouseButtonDown(0))
        {
            BulletMS go = Instantiate(_model.bulletPrefab);
            if (_minigameTimesShot % 2 == 1)
            {
                go.transform.position = _view.leftMuzzle.transform.position;
                go.transform.forward = _view.leftMuzzle.transform.forward;
                _view.leftMuzzleFlash.Play();
                _view.leftColt.GetComponent<Animator>().SetTrigger("Shot");
            }
            else
            {
                go.transform.position = _view.rightMuzzle.transform.position;
                go.transform.forward = _view.rightMuzzle.transform.forward;
                _view.rightMuzzleFlash.Play();                
                _view.rightColt.GetComponent<Animator>().SetTrigger("Shot");


            }
            EventManager.Broadcast(GameEventsHandler.MinigamePlayerShotEvent);
            _minigameTimesShot++;
            _minigameShot = true;
            if (_minigameTimesShot == 3)
            {
                _minigame = false;
            }

        }
        if (_minigame && _minigameShot && Input.GetMouseButtonUp(0))
        {
            _minigameShot = false;
        }
        //_speed += (_model.movementSpeed - _speed) * Time.deltaTime;
        //_characterController.Move((Vector3.forward + new Vector3(0,_verticalSpeed,0)) * (Time.deltaTime * _model.movementSpeed));

        /*if (_jumped && _characterController.isGrounded)
        {
            _jumped = false;
        }*/

        /*if (_characterController.isGrounded)
        {
            _verticalSpeed = 0;
        }
        else
        {
            _verticalSpeed -= _model.gravityModifier;
        }*/

    }

    private void Jump()
    {
        playerVelocity.y += Mathf.Sqrt(_model.jumpForce * 3.0f );
    }
    private void RotateGuns()
    {
        _view.rightGun.transform.LookAt(_view.target.transform);
        Vector3 rotation = _view.rightGun.transform.rotation.eulerAngles;
        _view.leftGun.transform.rotation = Quaternion.Euler(rotation.x, -rotation.y, rotation.z);
        
    }

    private IEnumerator FiringCoroutine(float rate)
    {   
        Animator leftAnimator = _view.leftColt.GetComponent<Animator>();
        Animator rightAnimator = _view.rightColt.GetComponent<Animator>();

        bool left = true;
        while (true)
        {
            float t = _model.firingRate;
            while (t > 0)
            {
                t -= Time.deltaTime;
                yield return null;
            }
            BulletMS go = Instantiate(_model.bulletPrefab);
            go.inheritedSpeed = _speed;
            if (left)
            {
                go.transform.position = _view.leftMuzzle.transform.position;
                go.transform.forward = _view.leftMuzzle.transform.forward;
                _view.leftMuzzleFlash.Play();
                leftAnimator.SetTrigger("Shot");
            }
            else
            {
                
                go.transform.position = _view.rightMuzzle.transform.position;
                go.transform.forward = _view.rightMuzzle.transform.forward;
                _view.rightMuzzleFlash.Play();   
                rightAnimator.SetTrigger("Shot");


            }

            left = !left;

        }
    }

    private IEnumerator BoostCor(float boostTime, float stoppingTime)
    {
        Taptic.Failure();
        FallFx.Instance.SetFxIntensity(100);
        _speed = _model.boostSpeed;
        for (float t = 0; t < boostTime; t += Time.deltaTime)
        {
            yield return null;
        }

        FallFx.Instance.SetFxIntensity(0);
        for (float t = 0; t < stoppingTime; t += Time.deltaTime)
        {
            _speed = Mathf.Lerp(_model.boostSpeed, _model.movementSpeed, t / stoppingTime);
            yield return null;
        }

        _speed = _model.movementSpeed;
    }

    private IEnumerator StartSpeedCor(float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            //_speed = Mathf.Lerp(0, _model.movementSpeed, t / time);
            yield return null;
        }

        _speed = _model.movementSpeed;
    }
}

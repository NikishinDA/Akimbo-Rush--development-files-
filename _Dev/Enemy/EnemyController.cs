using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour//, IShootable
{
    private bool _active;
    [SerializeField] private float _speed;
    [SerializeField] private float _invalidPosX;
    [SerializeField] private float _baseHealth = 100f;
    [SerializeField] private float _damagePerShot = 10f;
    [SerializeField] private Material[] _materials; //0 - weak, 1 - med, 2 - strong
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private CoinController _coinPrefab;
    private ObstacleType _type;
    private float _health;
    private Rigidbody _rb;
    private Animator _animator;
    [SerializeField] private GameObject _detectionObject;
    private Transform _target;
    private bool _dead = false;
    
    private void Awake()
    {
        EventManager.AddListener<PlayerDeadEvent>(OnPlayerDeath);
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _health = _baseHealth;
        //Initialize(_type); //Debug
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<PlayerDeadEvent>(OnPlayerDeath);
    }

    private void OnPlayerDeath(PlayerDeadEvent obj)
    {
        if (_active && !_dead)
        {
            _animator.SetTrigger("Idle");
        }
        Activate(false);
    }

    private void FixedUpdate()
    {
        if (_active && !_dead)
        {
            _rb.MovePosition(_rb.position + transform.forward * (Time.deltaTime * _speed));
            _rb.MoveRotation(Quaternion.LookRotation(_target.transform.position  - transform.position));//, Vector3.up);
        }
    }

    public void Activate(bool isActive = true)
    {
        _active = isActive;
        //transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
        if (isActive)
        {
            _animator.SetTrigger("Run");
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
    public void Initialize(ObstacleType type)
    {
        _renderer.material = _materials[(int) type];
        _health = _baseHealth * ((int) type + 1);
        _type = type;
        switch (type)
        {
            case ObstacleType.Medium:
                transform.localScale *= 1.1f;
                break;
            case ObstacleType.Strong:
                transform.localScale *= 1.25f;
                break;
        }
    }
    public void FixPosition()
    {
        if (Mathf.Abs(transform.position.x) < _invalidPosX)
        {
            StartCoroutine(FixingPosition(0.1f, (transform.position.x < 0) ? -1 : 1));
        }
    }

    public void OnShot()
    {
        _health -= _damagePerShot;
        if (_health <= 0)
            EnemyDeath();
    }

    public void AnimHit()
    {
        EventManager.Broadcast(GameEventsHandler.PlayerTakeDamageEvent);
        Activate(false);
    }
    private void EnemyDeath()
    {
        if (Random.value < 0.5f)
        {
            GetComponent<Animator>().SetTrigger("Kill");
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Kill Fly");
        }

        GetComponent<Collider>().enabled = false;
        _detectionObject.SetActive(false);
        Activate(false);
        _dead = true;
        var evt = GameEventsHandler.EnemyKilledEvent;
        evt.Type = _type;
        EventManager.Broadcast(evt);
        SpawnCoins();
    }

    private void SpawnCoins()
    {
        int num = (int) _type + 1;
        for (int i = 0; i < num; i++)
        {
            CoinController go = Instantiate(_coinPrefab, transform.position + Vector3.up, Quaternion.identity);
            go.gameObject.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * 5f,ForceMode.Impulse);
            go.target = _target;
        }
    }
    private IEnumerator FixingPosition(float time, float direction)
    {
        Vector3 initPos = transform.position;
        Vector3 newPos = new Vector3(_invalidPosX * direction, initPos.y, initPos.z);
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(initPos, newPos, t / time);
            yield return null;
        }
    }
}

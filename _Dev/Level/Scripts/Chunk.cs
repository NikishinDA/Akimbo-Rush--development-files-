using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Chunk : MonoBehaviour
{
    public Transform Begin;
    public Transform End;
    [SerializeField] private bool _isFinal = false;


    [SerializeField] private GameObject[] _effect;

    [SerializeField] private EnemyController[] _enemyPool;
    [SerializeField] private GameObject _glassObject;
    [SerializeField] private GameObject _cratesObject;
    [SerializeField] private Drone _droneObject;
    [SerializeField] private Transform[] _spawnPoints;
    //678
    //345
    //012
    [SerializeField] private Transform _enemyTarget;
    private void Start()
    {
        
    }

    

    protected void OnDestroy()
    {
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
       
    }
    public void SpawnObstacles(TemplateChunk.Line line)
    {
        switch (line.ObstacleType)
        {
            case ObstacleType.None:
            {
            }
                break;
            case ObstacleType.Glass:
            {
                Instantiate(_glassObject, _spawnPoints[(int) line.Position]);
            }
                break;
            case ObstacleType.Crates:
            {
                Instantiate(_cratesObject, _spawnPoints[(int) line.Position]);
            }
                break;
            case ObstacleType.Drone:
            {
                Instantiate(_droneObject, _spawnPoints[(int) line.Position]).Initialize(line.Position);
            }
                break;
            default:
            {
                EnemyController go = Instantiate(_enemyPool[Random.Range(0, _enemyPool.Length)], _spawnPoints[(int) line.Position]);
                go.Initialize(line.ObstacleType);
                //go.SetTarget(_enemyTarget);
            }
                break;
        }
        //Instantiate(_obstaclePool[(int) line.ObstacleType], _spawnPoints[(int) line.Position]);
    }
    private void OnGameOver(GameOverEvent obj)
    {
        if (obj.IsWin && !_isFinal)
        {
            Destroy(gameObject);
        }
    }
    
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPlacer : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Chunk _chunkPrefab;
    [SerializeField] private Chunk[] _firstPrefab;
    [SerializeField] private Chunk _finalPrefab;
    [SerializeField] private float _spawnDistance;
    [SerializeField] private int _concurrentChunkNumber;
    [SerializeField] private int _levelLength;
    private List<Chunk> _spawnedChunks;
    private bool _finishSpawned = false;
    private int _currentLength = 4;

    [SerializeField] private Chunk[] _chunkPool;
    [SerializeField] private LevelTemplate[] _templates;
    private LevelTemplate _currentTemplate;
    private void Awake()
    {
        _spawnedChunks = new List<Chunk>();
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        EventManager.AddListener<GameStartEvent>(OnGameStart);
        int level = (PlayerPrefs.GetInt("Level", 1) - 1) % 10;
       _currentTemplate = _templates[level];
       _levelLength = _currentTemplate.chunks.Length;
    }
    private void OnDestroy()
    {
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        EventManager.RemoveListener<GameStartEvent>(OnGameStart);

    }

    private void OnGameStart(GameStartEvent obj)
    {
        var evt = GameEventsHandler.GameInitializeEvent;
        evt.LevelLength = _currentTemplate.chunks.Length + 1;
        EventManager.Broadcast(evt);
    }

    private void Start()
    {
        _currentLength = 0;//_firstPrefab.Length;
        foreach (Chunk ch in _firstPrefab)
        {
            _spawnedChunks.Add(ch);
        }

    }

    private void Update()
    {
        if ((!_finishSpawned) && (_playerTransform.position.z > _spawnedChunks[_spawnedChunks.Count - 1].End.position.z - _spawnDistance))
        {
            SpawnChunk();
        }

    }
    private void SpawnChunk()
    {
        Chunk newChunk;
        if (_currentLength < _levelLength)
        {
            if (_currentTemplate != null)
            {
                newChunk = Instantiate(GetNextChunk(_currentLength));
                if (_currentTemplate.chunks[_currentLength].Lines.Length > 0)
                {
                    foreach (var line in _currentTemplate.chunks[_currentLength].Lines)
                    {
                        newChunk.SpawnObstacles(line);
                    }
                }
            }
            else
            {
                newChunk = Instantiate(_chunkPrefab);
            }
            
        }
        else
        {
            newChunk = Instantiate(_finalPrefab);
            _finishSpawned = true;
        }
        newChunk.transform.position = _spawnedChunks[_spawnedChunks.Count - 1].End.position - newChunk.Begin.localPosition;
        _spawnedChunks.Add(newChunk);
        
        _currentLength++;
        if (_spawnedChunks.Count > _concurrentChunkNumber)
        {
            Destroy(_spawnedChunks[0].gameObject);
            _spawnedChunks.RemoveAt(0);
        }
    }

    private Chunk GetNextChunk(int n)
    {
        if (n < _currentTemplate.chunks.Length)
        {
            int type = (int) _currentTemplate.chunks[n].ChunkType;
            try
            {
                return _chunkPool[type];
            }
            catch
            {
                Debug.Log("No such chunk exist in pool");
                return _chunkPrefab;
            }
        }
        else
        {
            return _finalPrefab;
        }
    }
    private void OnGameOver(GameOverEvent obj)
    {
        _finishSpawned = true;
    }
}

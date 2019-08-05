using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{

    public static GameController Instance;
    
    public GameState gameState;
    private bool _isPoolCreated;
    
    [SerializeField] private float _gameSpeed = 2f;
    [SerializeField] private Rigidbody _playerRb;
    [SerializeField] private Transform _playerTr;
    [SerializeField] private Renderer _playerRender;
    [SerializeField] private Transform _cameraTr;
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private Transform _floorTr;
    [SerializeField] private float _floorStartScaleZ = 20f;

    [SerializeField] private List<GameObject> _poolObjList;
    [SerializeField] private List<GameObject> _poolObjUsedList;
    [SerializeField] private List<EnemyController> _enemyControllersList;
    [SerializeField] private List<EnemyController> _enemyControllersUsedList;

    [SerializeField] private GameObject _restartWindow;
    [SerializeField] private GameObject _winWindow;
    
    public List<Material> materialsList;
    public List<GameObject> prefabObj;
    public int[] countObjInPool;

    public float score = 0f;
    [SerializeField] private TextMeshProUGUI _scoreTxt;

    private int _stage = 1;
    private int _startLineCount = 3;
    private int _playerMaterialIndex = 0;
    private const int _countStage = 5;

    private Vector3 _startPosPlayer;
    private Vector3 _startPosCam;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
             Destroy(gameObject);
        }
    }

    private void Start()
    {
        _startPosPlayer = _playerTr.position;
        _startPosCam = _cameraTr.position;
       
        SetData();
    }

    private void SetData()
    {
        _poolObjList = new List<GameObject>();
        _poolObjUsedList = new List<GameObject>();
        _enemyControllersList = new List<EnemyController>();
        _enemyControllersUsedList = new List<EnemyController>();
        
        _floorTr.localScale = new Vector3(1,1, _floorStartScaleZ);

        _playerMaterialIndex = Random.Range(0, materialsList.Count);
        _playerRender.material = materialsList[_playerMaterialIndex];

        score = 0f;
       
        // create pool
        CreatePool();
        CreateObstacles();
    }

    public void RestartGame()
    {
        _poolObjList.Clear();
        _enemyControllersList.Clear();
        _enemyControllersUsedList.Clear();

        for (var i = 0; i < _poolObjUsedList.Count; i++)
        {
            _poolObjUsedList[i].SetActive(false);
        }
        _poolObjUsedList.Clear();

        _playerTr.position = _startPosPlayer;
        _cameraTr.position = _startPosCam;
        _stage = 1;
        _playerRb.velocity = Vector3.zero;
        _playerRb.angularVelocity = Vector3.zero; 
       
        SetData();
        StartGame();
    }

    private void CreatePool()
    {
        var countPrefabs = prefabObj.Count;
        for (var i = 0; i < countPrefabs; i++)
        {
            var countObj = countObjInPool[i];
            for (var j = 0; j < countObj; j++)
            {
                var obj = Instantiate(prefabObj[i], _enemyContainer);
                _poolObjList.Add(obj);
                _enemyControllersList.Add(obj.GetComponent<EnemyController>());
            }
        }

        _isPoolCreated = true;
    }

    private const float FirstWallPosZ = 25f;
    private const float IndentationZ = 1.5f;
    private void CreateObstacles()
    {

        for (var i = 0; i < _startLineCount; i++)
        {
            
            var count = Random.Range(0, _poolObjList.Count);
            var obj = _poolObjList[count].gameObject;
            var pos = new Vector3(0, 0, (FirstWallPosZ * _stage) + (i * IndentationZ));
                   
            obj.transform.position = pos;
            obj.SetActive(true);
            
            _enemyControllersList[count].SetMaterials(materialsList, _playerMaterialIndex);
            _enemyControllersUsedList.Add(_enemyControllersList[count]);
            _enemyControllersList.RemoveAt(count);
            
            _poolObjList.RemoveAt(count);
            _poolObjUsedList.Add(obj);

        }
        
    }

    private const float NextWallCheckPoint = 5;
    private void FixedUpdate()
    {

        if (Instance != null && Instance.gameState == GameState.Started)
        {
            var delta = Vector3.forward * _gameSpeed * Time.fixedDeltaTime;
        
            _playerRb.MovePosition(_playerTr.position + delta);
            _cameraTr.position = _cameraTr.position + delta;
        
            // score 
            score += Time.fixedDeltaTime * _gameSpeed;
            var intScore = (int) score;
            if (_scoreTxt != null)
            {
                _scoreTxt.text = $"Score: {intScore}";
            }

            if (intScore > 0 && intScore % (NextWallCheckPoint * (_stage * _stage)) == 0 && _stage < _countStage)
            {
                _stage++;
                score += 1f;
                CreateObstacles();
            }
        }

    }

    public void StartGame()
    {
        gameState = GameState.Started;
    }
    
    public void EndGame()
    {
        gameState = GameState.Ended;
        _restartWindow.SetActive(true);
    }
    
    public void WinGame()
    {
        gameState = GameState.Win;
        _winWindow.SetActive(true);
    }

    public void PauseGame()
    {
        gameState = GameState.Pause;
    }
    
}

public enum GameState
{
    Started = 0,
    Ended = 1,
    Win,
    Pause
}

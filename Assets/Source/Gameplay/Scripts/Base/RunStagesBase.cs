using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class RunStagesBase : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private CameraSwitcher _cameraSwitcher;
    [SerializeField] private PlayerInfo _playerInfo;
    [Header("Start")]
    [SerializeField] private SpeedGameBase _speedGame;
    [Header("End")]
    [SerializeField] private EndGame _endGame;
    [SerializeField] private string _menuSceneName;
    [Header("Characters")]
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private List<EnemyMovement> _enemies;

    private List<CharacterMovement> _placements = new();
    private List<CharacterMovement> _finished = new();
    private bool _isRunning = false;

    public SpeedGameBase SpeedGame => _speedGame;
    public bool IsRunning => _isRunning;
    public int CharactersCount => _enemies.Count + 1;

    protected abstract int GetEnemyUpgrades(CharacteristicType characteristicType);

    protected abstract void GiveReward();

    public int GetPlacement(CharacterMovement characterMovement)
    {
        return _placements.IndexOf(characterMovement) + 1;
    }

    protected virtual void ActiveSpeedGame()
    {
        _speedGame.gameObject.SetActive(true);
    }

    private void Awake()
    {
        _playerInfo.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(StartPrewiev());
    }

    private void LateUpdate()
    {
        if (_isRunning)
        {
            CaclulatePlacements();
            _playerInfo.SetPlace(GetPlacement(_playerMovement));
        }
    }

    private void OnEnable()
    {
        if (_speedGame != null)
        {
            _speedGame.EndedSpeedGame += StartRunning;
        }

        _playerMovement.OnChangeChunk += CheckEndChunk;

        foreach (CharacterMovement enemy in _enemies)
        {
            enemy.OnChangeChunk += CheckEndChunk;
        }
    }

    private void OnDisable()
    {
        if (_speedGame != null)
        {
            _speedGame.EndedSpeedGame -= StartRunning;
        }

        _playerMovement.OnChangeChunk -= CheckEndChunk;

        foreach (CharacterMovement enemy in _enemies)
        {
            enemy.OnChangeChunk -= CheckEndChunk;
        }
    }

    private void CaclulatePlacements()
    {
        _placements.Clear();

        _placements.Add(_playerMovement);
        _placements.AddRange(_enemies);
        _placements.RemoveAll(a => _finished.Contains(a));
        _placements.Sort((a, b) => b.Distance.CompareTo(a.Distance));
        List<CharacterMovement> newFinished = _placements.FindAll(a => a.IsFinished);
        
        if (newFinished.Contains(_playerMovement))
        {
            newFinished.Remove(_playerMovement);
            newFinished.Insert(0, _playerMovement);
        }

        _finished.AddRange(newFinished);

        _placements.RemoveAll(a => newFinished.Contains(a));

        for (int i = 0; i < _finished.Count; i++)
        {
            _placements.Insert(i, _finished[i]);
        }
    }

    private void StartRunning(float multiplier)
    {
        _cameraSwitcher.SwitchToGameplayCamera();
        _speedGame.gameObject.SetActive(false);
        _playerInfo.gameObject.SetActive(true);
        _isRunning = true;

        List<Chunk> chunks = _map.Chunks;

        _playerMovement.StartMove(chunks, multiplier);

        if (_enemies.Count > 0)
        {
            foreach (EnemyMovement enemy in _enemies)
            {
                enemy.SetUpgrades(
                    GetEnemyUpgrades(CharacteristicType.Run),
                    GetEnemyUpgrades(CharacteristicType.Swim),
                    GetEnemyUpgrades(CharacteristicType.Climb),
                    GetEnemyUpgrades(CharacteristicType.Fly));
                enemy.StartMove(chunks, _speedGame.RandomSpeedMultiplier);
            }
        }
    }

    private void CheckEndChunk(Chunk chunk, CharacterMovement characterMovement)
    {
        if (chunk.Type == ChunkType.Finish)
        {
            if (characterMovement == _playerMovement)
            {
                _endGame.AddPlayerFinisher(characterMovement);
                GiveReward();
                StartCoroutine(LoadMenuOnClick());
            }
            else
            {
                _endGame.AddFinisher(characterMovement);
            }
        }
    }

    private bool IsTouchGown()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            return touch.phase == TouchPhase.Began;
        }

        return Input.GetKeyDown(KeyCode.Mouse0);
    }

    private IEnumerator LoadMenuOnClick()
    {
        while (true)
        {
            if (IsTouchGown())
            {
                SceneManager.LoadScene(_menuSceneName);
                break;
            }

            yield return null;
        }
    }

    private IEnumerator StartPrewiev()
    {
        yield return StartCoroutine(_cameraSwitcher.SwitchToPreviewCamera());

        ActiveSpeedGame();
    }
}

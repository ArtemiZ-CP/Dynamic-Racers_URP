using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [Header("Enemy Characteristics")]
    [SerializeField] private float _minCharacteristicsDelta;
    [SerializeField] private float _maxCharacteristicsDelta;

    private List<CharacterMovement> _placements = new();
    private List<CharacterMovement> _finished = new();
    private bool _isRunning = false;

    public SpeedGameBase SpeedGame => _speedGame;
    public bool IsRunning => _isRunning;
    public int CharactersCount => _enemies.Count + 1;

    private void Awake()
    {
        _playerInfo.gameObject.SetActive(false);

        if (RunSettings.EnemyGadgets != null)
        {
            foreach (EnemyMovement enemy in _enemies)
            {
                enemy.GetComponent<CharacterGadgets>().Init(RunSettings.EnemyGadgets.ToList()[_enemies.IndexOf(enemy)]);
            }
        }
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

    public int GetPlacement(CharacterMovement characterMovement)
    {
        return _placements.IndexOf(characterMovement) + 1;
    }

    protected virtual void ActiveSpeedGame()
    {
        _speedGame.gameObject.SetActive(true);
    }

    protected abstract void GiveReward(int placement);

    private float GetEnemyUpgrades()
    {
        return Random.Range(_minCharacteristicsDelta, _maxCharacteristicsDelta);
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

    private void StartRunning(float multiplier, bool goodStart)
    {
        _cameraSwitcher.SwitchToGameplayCamera();
        _speedGame.gameObject.SetActive(false);
        _playerInfo.gameObject.SetActive(true);
        _isRunning = true;

        List<Chunk> chunks = _map.Chunks;

        _playerMovement.StartMove(chunks, multiplier);
        _playerMovement.GetComponent<CharacterAnimation>().LaunchCharacter(goodStart, _speedGame.FullCharge);

        if (_enemies.Count > 0)
        {
            foreach (EnemyMovement enemy in _enemies)
            {
                enemy.SetUpgrades(
                    GetEnemyUpgrades(),
                    GetEnemyUpgrades(),
                    GetEnemyUpgrades(),
                    GetEnemyUpgrades());
                enemy.StartMove(chunks, _speedGame.GetRandomSpeedMultiplier(out goodStart));
                enemy.GetComponent<CharacterAnimation>().LaunchCharacter(goodStart, true);
            }
        }
    }

    private void CheckEndChunk(Chunk chunk, CharacterMovement characterMovement)
    {
        if (characterMovement.TryGetComponent(out CharacterGadgets characterGadgets) == false)
        {
            return;
        }

        if (chunk.Type == ChunkType.Finish)
        {
            int placement = GetPlacement(characterMovement);

            if (characterMovement == _playerMovement)
            {
                _endGame.AddPlayerFinisher(characterGadgets);
                GiveReward(placement);
                StartCoroutine(LoadMenuOnClick());
            }
            else
            {
                _endGame.AddFinisher(characterGadgets);
            }
        }
    }

    private bool IsTouchUp()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            return touch.phase == TouchPhase.Ended;
        }

        return Input.GetKeyUp(KeyCode.Mouse0);
    }

    private IEnumerator LoadMenuOnClick()
    {
        while (true)
        {
            if (IsTouchUp())
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

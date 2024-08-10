using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LoadAnimation))]
public class PreloadLevel : MonoBehaviour
{
    [SerializeField] private string _levelName = "Gameplay";
    [Header("Training")]
    [SerializeField] private string _trainingLevelName = "Training";
    [SerializeField] private int _startTrainingLevelIndex = 1;

    private LoadAnimation _loadAnimation;

    private void Awake()
    {
        _loadAnimation = GetComponent<LoadAnimation>();
    }

    private void Start()
    {
        StartCoroutine(Preload());
    }

    private IEnumerator Preload()
    {
        string levelName;
        int playersCount;

        if (PlayerData.PassedTrainings < GlobalSettings.Instance.TrainingLevelsCount)
        {
            levelName = $"{_trainingLevelName}{PlayerData.PassedTrainings + _startTrainingLevelIndex}";
            playersCount = GlobalSettings.Instance.TrainingPlayersCount;
        }
        else
        {
            levelName = _levelName;
            playersCount = GlobalSettings.Instance.GameplayPlayersCount;
        }

        var levelLoader = SceneManager.LoadSceneAsync(levelName);
        levelLoader.allowSceneActivation = false;

        yield return _loadAnimation.StartAnimation(playersCount);
        
        levelLoader.allowSceneActivation = true;
    }
}

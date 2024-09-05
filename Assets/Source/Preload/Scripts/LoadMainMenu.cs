using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMainMenu : MonoBehaviour
{
    private const float ActivationProgress = 0.9f;

    [SerializeField] private Slider _progressBar;
    [SerializeField] private string _menuScene;
    [SerializeField] private TMP_Text _loadingPercentage;
    [SerializeField] private float _minLoadTime;
    [SerializeField] private string _trainingName = "Training";

    private AsyncOperation _loadingOperation;
    private float _loadTime = 0;

    private void Start()
    {
        LoadSceneAsync();
    }

    private void Update()
    {
        _loadTime += Time.deltaTime;
        float progress = Mathf.Clamp01(Mathf.Min(_loadingOperation.progress / ActivationProgress, _loadTime / _minLoadTime));
        _progressBar.value = progress;
        _loadingPercentage.text = $"{(int)(progress * 100)}%";

        if (_loadingOperation.progress >= ActivationProgress && _loadTime >= _minLoadTime)
        {
            _loadingOperation.allowSceneActivation = true;
        }
    }

    private void LoadSceneAsync()
    {
        if (PlayerData.PassedTrainings < GlobalSettings.Instance.TrainingLevelsCount)
        {
            _loadingOperation = SceneManager.LoadSceneAsync($"{_trainingName}{PlayerData.PassedTrainings + 1}");
        }
        else
        {
            _loadingOperation = SceneManager.LoadSceneAsync(_menuScene);
        }

        _loadingOperation.allowSceneActivation = false;
    }
}

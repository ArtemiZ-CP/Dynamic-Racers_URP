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

    private AsyncOperation _loadingOperation;

    private void Start()
    {
        LoadSceneAsync();
    }

    private void Update()
    {
        float progress = Mathf.Clamp01(_loadingOperation.progress / ActivationProgress);
        _progressBar.value = progress;
        _loadingPercentage.text = $"{(int)(progress * 100)}%";

        if (_loadingOperation.progress >= ActivationProgress)
        {
            _loadingOperation.allowSceneActivation = true;
        }
    }

    private void LoadSceneAsync()
    {
        _loadingOperation = SceneManager.LoadSceneAsync(_menuScene);
        _loadingOperation.allowSceneActivation = false;
    }
}

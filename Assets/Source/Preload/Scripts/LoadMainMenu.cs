using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMainMenu : MonoBehaviour
{
    private const float ActivationProgress = 0.9f;

    [SerializeField] private Slider _progressBar;
    [SerializeField] private string _sceneToLoad;
    [SerializeField] private TMP_Text _loadingPercentage;

    private AsyncOperation loadingOperation;

    private void Start()
    {
        loadingOperation = SceneManager.LoadSceneAsync(_sceneToLoad);
        loadingOperation.allowSceneActivation = false;
    }

    private void Update()
    {
        float progress = Mathf.Clamp01(loadingOperation.progress / ActivationProgress);
        _progressBar.value = progress;
        _loadingPercentage.text = $"{(int)(progress * 100)}%";

        if (loadingOperation.progress >= ActivationProgress)
        {
            loadingOperation.allowSceneActivation = true;
        }
    }
}

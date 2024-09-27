using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LoadAnimation))]
public class PreloadLevel : MonoBehaviour
{
    [SerializeField] private string _levelName = "Gameplay";

    private LoadAnimation _loadAnimation;

    private void Awake()
    {
        _loadAnimation = GetComponent<LoadAnimation>();
    }

    private void OnEnable()
    {
        StartCoroutine(Preload());
    }

    private IEnumerator Preload()
    {
        var levelLoader = SceneManager.LoadSceneAsync(_levelName);
        levelLoader.allowSceneActivation = false;

        yield return _loadAnimation.StartAnimation(GlobalSettings.Instance.GameplayPlayersCount);

        levelLoader.allowSceneActivation = true;
    }
}

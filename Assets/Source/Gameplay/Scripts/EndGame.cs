using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject _endGamePanel;

    public void Run()
    {
        _endGamePanel.SetActive(true);
    }

    private void Awake()
    {
        _endGamePanel.SetActive(false);
    }
}

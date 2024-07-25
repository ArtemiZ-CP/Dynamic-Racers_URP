using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private EndGamePanel _endGamePanel;

    public void AddPlayerFinisher(CharacterMovement characterMovement)
    {
        AddFinisher(characterMovement);
        Show();
    }

    public void AddFinisher(CharacterMovement characterMovement)
    {
        _endGamePanel.AddFinisher(characterMovement.name);
    }

    private void Awake()
    {
        _endGamePanel.gameObject.SetActive(false);
    }

    private void Show()
    {
        _endGamePanel.gameObject.SetActive(true);
    }
}

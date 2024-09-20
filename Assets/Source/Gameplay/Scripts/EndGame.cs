using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private EndGamePanel _endGamePanel;
    [SerializeField] private float _showDelay = 1;

    public void AddPlayerFinisher(CharacterGadgets characterGadgets, int placement)
    {
        PlayerData.SetPlayerPlace(placement);
        AddFinisher(characterGadgets, placement);
        Invoke(nameof(Show), _showDelay);
    }

    public void AddFinisher(CharacterGadgets characterGadgets, int placement)
    {
        Sprite gadgetSprite = null;

        if (characterGadgets.Gadget != null)
        {
            gadgetSprite = characterGadgets.Gadget.ScriptableObject.SmallSprite;
        }

        _endGamePanel.AddFinisher(characterGadgets.name, placement, gadgetSprite, characterGadgets is PlayerGadgets);
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

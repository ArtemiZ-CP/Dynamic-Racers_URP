using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private EndGamePanel _endGamePanel;
    [SerializeField] private float _showDelay = 1;

    private int place = 1;

    public void AddPlayerFinisher(CharacterGadgets characterGadgets)
    {
        AddFinisher(characterGadgets);
        Invoke(nameof(Show), _showDelay);
    }

    public void AddFinisher(CharacterGadgets characterGadgets)
    {
        Sprite gadgetSprite = null;

        if (characterGadgets.Gadget != null)
        {
            gadgetSprite = characterGadgets.Gadget.ScriptableObject.SmallSprite;
        }

        _endGamePanel.AddFinisher(characterGadgets.name, place, gadgetSprite, characterGadgets is PlayerGadgets);
        place++;
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

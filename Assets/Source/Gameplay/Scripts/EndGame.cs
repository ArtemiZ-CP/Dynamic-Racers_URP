using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private EndGamePanel _endGamePanel;
    [SerializeField] private float _showDelay = 1;
    [SerializeField] private GameObject _characterPoint;
    [SerializeField] private MapEditor _mapEditor;

    private bool _particelsActive = false;

    public void AddPlayerFinisher(CharacterGadgets characterGadgets)
    {
        int place = AddFinisher(characterGadgets);
        PlayerData.SetPlayerPlace(place);
        Invoke(nameof(Show), _showDelay);
    }

    public int AddFinisher(CharacterGadgets characterGadgets)
    {
        if (_particelsActive == false)
        {
            _mapEditor.ActiveFinishParticles();
            _particelsActive = true;
        }

        Sprite gadgetSprite = null;

        if (characterGadgets.Gadget != null)
        {
            gadgetSprite = characterGadgets.Gadget.ScriptableObject.SmallSprite;
        }

        return _endGamePanel.AddFinisher(characterGadgets.name, gadgetSprite, characterGadgets is PlayerGadgets);
    }

    private void Awake()
    {
        _endGamePanel.gameObject.SetActive(false);
    }

    private void Show()
    {
        if (_characterPoint != null) _characterPoint.SetActive(false);
        _endGamePanel.gameObject.SetActive(true);
    }
}

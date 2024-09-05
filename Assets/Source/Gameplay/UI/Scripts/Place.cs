using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Place : MonoBehaviour
{

    [SerializeField] private TMP_Text _placeText;
    [SerializeField] private TMP_Text _place;
    [SerializeField] private List<GameObject> _1;
    [SerializeField] private List<GameObject> _2;
    [SerializeField] private List<GameObject> _3;
    [SerializeField] private List<GameObject> _4;
    [SerializeField] private Image _gadget;
    [SerializeField] private GameObject _playerOutline;

    public void SetPlace(string name, int place, Sprite gadgetSprite)
    {
        _placeText.text = name;
        _place.text = place.ToString();
        ActivePlacementObjects(place);

        if (gadgetSprite == null)
        {
            _gadget.gameObject.SetActive(false);
        }
        else
        {
            _gadget.sprite = gadgetSprite;
        }

        _playerOutline.SetActive(false);
    }

    public void SetPlayerOutline()
    {
        _playerOutline.SetActive(true);
    }

    

    private void ActivePlacementObjects(int place)
    {
        SetActiveObjects(_1, false);
        SetActiveObjects(_2, false);
        SetActiveObjects(_3, false);
        SetActiveObjects(_4, false);

        switch (place)
        {
            case 1:
                SetActiveObjects(_1, true);
                break;
            case 2:
                SetActiveObjects(_2, true);
                break;
            case 3:
                SetActiveObjects(_3, true);
                break;
            default:
                SetActiveObjects(_4, true);
                break;
        }
    }

    private void SetActiveObjects(List<GameObject> objects, bool isActive)
    {
        foreach (var item in objects)
        {
            item.SetActive(isActive);
        }
    }
}

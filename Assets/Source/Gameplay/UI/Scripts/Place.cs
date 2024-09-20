using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Place : MonoBehaviour
{
    [Serializable]
    private class PlaceImages
    {
        public Sprite Sprite;
        public GameObject Place;
    }

    [SerializeField] private TMP_Text _placeText;
    [SerializeField] private TMP_Text _place;
    [SerializeField] private PlaceImages _1;
    [SerializeField] private PlaceImages _2;
    [SerializeField] private PlaceImages _3;
    [SerializeField] private PlaceImages _4;
    [SerializeField] private Image _gadget;
    [SerializeField] private Image _background;
    [SerializeField] private BlickAnimation _blickAnimation;
    [SerializeField] private GameObject _playerOutline;

    private PlaceImages _currentPlace;

    public void SetPlace(string name, int place, Sprite gadgetSprite)
    {
        _placeText.text = name;
        _place.text = place.ToString();
        SetPlacementSprite(place);
        _playerOutline.SetActive(false);

        if (gadgetSprite == null)
        {
            _gadget.gameObject.SetActive(false);
        }
        else
        {
            _gadget.sprite = gadgetSprite;
        }
    }

    public void SetPlayerPlace()
    {
        _playerOutline.SetActive(true);
        _background.material = _blickAnimation.BlickMaterial;
        _blickAnimation.Initialize(_currentPlace.Sprite);
        _blickAnimation.enabled = true;
    }

    private void SetPlacementSprite(int place)
    {
        switch (place)
        {
            case 1:
                _currentPlace = _1;
                break;
            case 2:
                _currentPlace = _2;
                break;
            case 3:
                _currentPlace = _3;
                break;
            default:
                _currentPlace = _4;
                break;
        }

        _background.sprite = _currentPlace.Sprite;
        _currentPlace.Place.SetActive(true);
    }
}

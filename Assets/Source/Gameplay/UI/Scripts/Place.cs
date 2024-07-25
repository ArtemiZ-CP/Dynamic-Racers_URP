using TMPro;
using UnityEngine;

public class Place : MonoBehaviour
{
    [SerializeField] private TMP_Text _placeText;

    public void SetPlace(string text)
    {
        _placeText.text = text;
    }
}

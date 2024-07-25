using TMPro;
using UnityEngine;

public class PlayerPlacement : MonoBehaviour
{
    [SerializeField] private TMP_Text _placeText;

    public void SetPlace(int place)
    {
        string placeText = place.ToString();
        placeText += "<sup>-";

        placeText += place switch
        {
            1 => "st",
            2 => "nd",
            3 => "rd",
            _ => "th",
        };

        placeText += "</sup>";
        _placeText.text = placeText;
    }
}

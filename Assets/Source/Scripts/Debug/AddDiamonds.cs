using UnityEngine;
using TMPro;

public class AddDiamonds : MonoBehaviour
{
    [SerializeField] private int _diamondsToGive;

    [ContextMenu("Give Diamonds")]
    public void GiveDiamondsContextMenu()
    {
        PlayerData.AddDiamonds(_diamondsToGive);
    }

    public void GiveDiamondsWithButton(TMP_InputField inputField)
    {
        if (int.TryParse(inputField.text, out int diamonds))
        {
            PlayerData.AddDiamonds(diamonds);
        }
        else
        {
            PlayerData.AddDiamonds(1);
        }
    }
}

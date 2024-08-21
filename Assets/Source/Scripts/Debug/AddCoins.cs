using TMPro;
using UnityEngine;

public class AddCoins : MonoBehaviour
{
    [SerializeField] private int _CoinsToGive;

    [ContextMenu("Give Coins")]
    public void GiveCoinsContextMenu()
    {
        PlayerData.AddCoins(_CoinsToGive);
    }

    public void GivecoinsWithButton(TMP_InputField inputField)
    {
        if (int.TryParse(inputField.text, out int coins))
        {
            PlayerData.AddCoins(coins);
        }
        else
        {
            PlayerData.AddCoins(1);
        }
    }
}

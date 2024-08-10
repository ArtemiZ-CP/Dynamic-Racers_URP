using TMPro;
using UnityEngine;

public class CoinsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinsText;

    private void OnEnable()
    {
        PlayerData.OnCoinsChanged += SetCoins;
        SetCoins();
    }

    private void OnDisable()
    {
        PlayerData.OnCoinsChanged -= SetCoins;
    }

    public void SetCoins()
    {
        _coinsText.text = PlayerData.Coins.ToString();
    }
}

using TMPro;
using UnityEngine;

public class CoinsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinsText;

    private void OnEnable()
    {
        PlayerProgress.OnCoinsChanged += SetCoins;
        SetCoins();
    }

    private void OnDisable()
    {
        PlayerProgress.OnCoinsChanged -= SetCoins;
    }

    public void SetCoins()
    {
        _coinsText.text = PlayerProgress.Coins.ToString();
    }
}

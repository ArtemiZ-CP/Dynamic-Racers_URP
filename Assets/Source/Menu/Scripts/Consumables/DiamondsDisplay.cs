using TMPro;
using UnityEngine;

public class DiamondsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _diamondsText;

    private void OnEnable()
    {
        PlayerData.OnDiamondsChanged += SetDiamonds;
        SetDiamonds();
    }

    private void OnDisable()
    {
        PlayerData.OnDiamondsChanged -= SetDiamonds;
    }

    public void SetDiamonds()
    {
        _diamondsText.text = PlayerData.Diamonds.ToString();
    }
}

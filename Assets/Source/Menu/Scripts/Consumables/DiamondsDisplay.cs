using TMPro;
using UnityEngine;

public class DiamondsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _diamondsText;

    private void OnEnable()
    {
        PlayerProgress.OnDiamondsChanged += SetDiamonds;
        SetDiamonds();
    }

    private void OnDisable()
    {
        PlayerProgress.OnDiamondsChanged -= SetDiamonds;
    }

    public void SetDiamonds()
    {
        _diamondsText.text = PlayerProgress.Diamonds.ToString();
    }
}

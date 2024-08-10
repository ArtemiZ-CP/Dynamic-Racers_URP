using TMPro;
using UnityEngine;

public class TicketsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _ticketsText;

    private int _maxTickets;

    private void Awake()
    {
        _maxTickets = GlobalSettings.Instance.MaxTickets;
    }

    private void OnEnable()
    {
        PlayerData.OnTicketsChanged += SetTickets;
        SetTickets();
    }

    private void OnDisable()
    {
        PlayerData.OnTicketsChanged -= SetTickets;
    }

    public void SetTickets()
    {
        _ticketsText.text = $"{PlayerData.Tickets}/{_maxTickets}";
    }
}

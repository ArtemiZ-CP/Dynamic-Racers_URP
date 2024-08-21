using UnityEngine;
using TMPro;

public class AddTickets : MonoBehaviour
{
    [SerializeField] private int _ticketsToGive;

    [ContextMenu("Give Tickets")]
    public void GiveTicketsContextMenu()
    {
        PlayerData.AddTickets(_ticketsToGive);
    }

    public void GiveTicketsWithButton(TMP_InputField inputField)
    {
        if (int.TryParse(inputField.text, out int tickets))
        {
            PlayerData.AddTickets(tickets);
        }
        else
        {
            PlayerData.AddTickets(1);
        }
    }
}

using TMPro;
using UnityEngine;

public class AddXP : MonoBehaviour
{
    [SerializeField] private int _XPToGive;

    [ContextMenu("Give XP")]
    public void GiveXPContextMenu()
    {
        PlayerData.AddExperience(_XPToGive);
    }

    public void GiveXPWithButton(TMP_InputField inputField)
    {
        if (int.TryParse(inputField.text, out int xp))
        {
            PlayerData.AddExperience(xp);
        }
        else
        {
            PlayerData.AddExperience(1);
        }
    }
}

using UnityEngine;

public class ProfileButton : MonoBehaviour
{
    [SerializeField] private GameObject _profilePanel;

    public void OpenProfilePanel()
    {
        _profilePanel.SetActive(true);
    }

    public void CloseProfilePanel()
    {
        _profilePanel.SetActive(false);
    }

    private void OnEnable()
    {
        _profilePanel.SetActive(false);
    }
}

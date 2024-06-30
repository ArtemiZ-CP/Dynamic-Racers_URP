using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private string _levelName = "Gameplay";

    public void Load()
    {
        if (RunSettings.PlayerGadget == null)
        {
            return;
        }

        SceneManager.LoadScene(_levelName);
    }
}

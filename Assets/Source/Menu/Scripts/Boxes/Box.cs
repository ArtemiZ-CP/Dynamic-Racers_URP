using UnityEngine;

public class Box : MonoBehaviour
{
    public void Init()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

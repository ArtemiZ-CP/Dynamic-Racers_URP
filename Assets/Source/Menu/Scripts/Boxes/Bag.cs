using UnityEngine;

public class Bag : MonoBehaviour
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

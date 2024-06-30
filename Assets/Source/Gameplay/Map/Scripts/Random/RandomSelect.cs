using UnityEngine;

public class RandomSelect : MonoBehaviour
{
    private void Awake()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        
        int index = Random.Range(0, transform.childCount);
        transform.GetChild(index).gameObject.SetActive(true);
    }
}

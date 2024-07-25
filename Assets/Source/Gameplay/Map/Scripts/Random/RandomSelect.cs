using UnityEngine;

[ExecuteAlways]
public class RandomSelect : MonoBehaviour
{
    private void Awake()
    {
        if (transform.childCount == 0)
        {
            return;
        }

        int index = Random.Range(0, transform.childCount);

        if (Application.isPlaying)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                if (i != index)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
                else
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            transform.GetChild(index).gameObject.SetActive(true);
        }
    }
}

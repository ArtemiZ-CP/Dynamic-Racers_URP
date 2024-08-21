using UnityEngine;

public class ResetSaves : MonoBehaviour
{
    public void Reset()
    {
        DataSaver.ResetData();
    }
}

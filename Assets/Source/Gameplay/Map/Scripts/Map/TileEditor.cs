using System.Collections.Generic;
using UnityEngine;

public class TileEditor : MonoBehaviour
{
    [SerializeField] private List<GameObject> _before;
    [SerializeField] private List<GameObject> _after;

    public void SetupTile(bool emptyBefore, bool emptyAfter)
    {
        if (_before != null && emptyBefore == false)
        {
            foreach (var item in _before)
            {
                item.SetActive(false);
            }
        }

        if (_after != null && emptyAfter == false)
        {
            foreach (var item in _after)
            {
                item.SetActive(false);
            }
        }
    }
}

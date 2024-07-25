using UnityEngine;

[ExecuteAlways]
public class SaveCellDirection : MonoBehaviour
{
    [SerializeField] private Transform _cell;

    private void Start()
    {
        if (transform.localScale.z < 0)
        {
            _cell.localScale = new Vector3(_cell.localScale.x, _cell.localScale.y, -_cell.localScale.z);
        }
    }
}

using UnityEngine;

public class Environment : MonoBehaviour
{
    public enum Type
    {
        Ground,
        Wall,
        Outside,
    }

    [SerializeField, Min(1)] private int _lenth;
    [SerializeField, Min(1)] private int _wigth;
    [SerializeField] private bool _isHigh = false;
    [SerializeField] private Type _type;
    [Header("Debug")]
    [SerializeField] private bool _isDebug = false;

    public int Lenth => _lenth;
    public int Wigth => _wigth;
    public bool IsHigh => _isHigh;
    public Type EnvironmentType => _type;

    private void Awake()
    {
        _isDebug = false;
    }

    private void OnDrawGizmos()
    {
        if (_isDebug == false)
        {
            return;
        }

        Gizmos.color = Color.green;
        int offset = GlobalSettings.Instance.RoadsOffset;

        if (_type == Type.Wall)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(_wigth * offset, _lenth * offset, 1));
        }
        else
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(_wigth * offset, 1, _lenth * offset));
        }
    }
}

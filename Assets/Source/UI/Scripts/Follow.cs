using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform _target;
	[SerializeField] private bool _fixX = false;
	[SerializeField] private bool _fixY = false;
	[SerializeField] private bool _fixZ = false;

    private Vector3 _offset;

    private void Awake()
    {
        _offset = transform.position - _target.position;
    }

    private void Update()
    {
        Vector3 position = _target.position + _offset;

        if (_fixX)
            position.x = transform.position.x;
        if (_fixY)
            position.y = transform.position.y;
        if (_fixZ)
            position.z = transform.position.z;

        transform.position = position;
    }
}

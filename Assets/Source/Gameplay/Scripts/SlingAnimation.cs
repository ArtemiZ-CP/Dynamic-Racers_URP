using UnityEngine;

public class SlingAnimation : MonoBehaviour
{
    [SerializeField] private Transform _playerPoint;
    [SerializeField] private Transform _slingPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private float _slingSpeed = 3f;

    private void Update()
    {
        if (_playerPoint.position.z > _endPoint.position.z)
        {
            float z = Mathf.Lerp(_slingPoint.position.z, _startPoint.position.z, Time.deltaTime * _slingSpeed);
            Vector3 newPosition = new(_slingPoint.position.x, _slingPoint.position.y, z);
            _slingPoint.position = newPosition;
        }
        else
        {
            _slingPoint.position = _playerPoint.position;
        }
    }
}

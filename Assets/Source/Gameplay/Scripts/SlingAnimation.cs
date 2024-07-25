using UnityEngine;

public class SlingAnimation : MonoBehaviour
{
    [SerializeField] private Transform _characterPoint;
    [SerializeField] private Transform _slingPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Animation _animation;

    private bool _isSlinging;

    private void Update()
    {
        if (_characterPoint.position.z > _endPoint.position.z)
        {
            if (_isSlinging == false)
            {
                _animation.Play();
                _isSlinging = true;
            }
        }
        else
        {
            _slingPoint.position = new Vector3(_slingPoint.position.x, _slingPoint.position.y, _characterPoint.position.z);
        }
    }
}

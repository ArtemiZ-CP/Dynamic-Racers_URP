using UnityEngine;

[ExecuteAlways]
public class RandomSize : MonoBehaviour
{
    [SerializeField, Min(0)] private float _minSize;
    [SerializeField] private float _maxSize = 1;

    private void Awake()
    {
        transform.localScale = Random.Range(_minSize, _maxSize) * Vector3.one;
    }
}

using UnityEngine;

[ExecuteAlways]
public class RandomPosition : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Vector3 _endPosition;

    private void Awake()
    {
        transform.localPosition = Vector3.Lerp(_startPosition, _endPosition, Random.value);
    }
}

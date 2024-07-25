using System.Collections;
using UnityEngine;

public class StartOffsetEnemy : MonoBehaviour
{
    [SerializeField] private EnemyMovement _enemyMovement;
    [SerializeField] private float _minTimeDelay;
    [SerializeField] private float _maxTimeDelay;
    [SerializeField] private float _minTimeToOffset;
    [SerializeField] private float _maxTimeToOffset;

    private float _startOffset;

    private void Awake()
    {
        _startOffset = GlobalSettings.Instance.CharacterStartOffset;
    }

    private void Start()
    {
        float time = Random.Range(_minTimeToOffset, _maxTimeToOffset);
        float delay = Random.Range(_minTimeDelay, _maxTimeDelay);
        StartCoroutine(Offset(time, delay));
    }

    private IEnumerator Offset(float time, float delay)
    {
        yield return new WaitForSeconds(delay);

        float speed = _startOffset / time;
        float offset = 0;

        while (offset < _startOffset)
        {
            offset += speed * Time.deltaTime;
            _enemyMovement.transform.position += speed * Time.deltaTime * Vector3.back;
            yield return null;
        }
    }
}

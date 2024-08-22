using System.Collections;
using UnityEngine;

public class BlickAnimation : MonoBehaviour
{
    [SerializeField] Material _blickMaterial;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _delay = 0f;
    [SerializeField] private float _fromOffset;
    [SerializeField] private float _toOffset;
    [SerializeField] private float _yOffset;

    private float _offset;

    private void OnEnable()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        while (true)
        {
            _offset = _fromOffset;
            float t = 0;

            while (true)
            {
                _offset = Mathf.Lerp(_fromOffset, _toOffset, t);
                _blickMaterial.SetVector("_Offset", new Vector2(_offset, _yOffset));
                t += Time.deltaTime * _speed;

                if (_offset == _toOffset)
                {
                    break;
                }

                yield return null;
            }

            yield return new WaitForSeconds(_delay);
        }
    }
}

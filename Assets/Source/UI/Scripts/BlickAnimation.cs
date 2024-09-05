using System.Collections;
using UnityEngine;

public class BlickAnimation : MonoBehaviour
{
    [SerializeField] private Material _blickMaterial;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _delay = 0f;
    [SerializeField] private float _fromOffset;
    [SerializeField] private float _toOffset;
    [SerializeField] private float _yOffset;
    [SerializeField] private Sprite _upgradeButtonSprite;
    [SerializeField] private Sprite _disactiveUpgradeButtonSprite;

    private float _offset;

    private void OnEnable()
    {
        ActiveButton();
    }

    public void ActiveButton()
    {
        _blickMaterial.SetTexture("_Texture2D", _upgradeButtonSprite.texture);
        StartCoroutine(Animate());
    }

    public void DisactiveButton()
    {
        _blickMaterial.SetTexture("_Texture2D", _disactiveUpgradeButtonSprite.texture);
        StopAllCoroutines();
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

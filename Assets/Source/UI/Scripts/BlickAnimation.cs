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
    public Material BlickMaterial => _blickMaterial;

    private void OnEnable()
    {
        ActiveBlick();
    }

    public void Initialize(Sprite buttonSprite)
    {
        _upgradeButtonSprite = buttonSprite;
        _disactiveUpgradeButtonSprite = buttonSprite;
        
        if (gameObject.activeInHierarchy) ActiveBlick();
    }

    public void Initialize(Sprite upgradeButtonSprite, Sprite disactiveUpgradeButtonSprite)
    {
        _upgradeButtonSprite = upgradeButtonSprite;
        _disactiveUpgradeButtonSprite = disactiveUpgradeButtonSprite;
    }

    public void ActiveBlick()
    {
        _blickMaterial.SetTexture("_Texture2D", _upgradeButtonSprite.texture);
        StopAllCoroutines();
        StartCoroutine(Animate());
    }

    public void DisactiveBlick()
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

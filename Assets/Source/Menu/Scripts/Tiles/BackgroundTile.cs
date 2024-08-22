using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundTile : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Image _backBlickImage;
    [SerializeField] private Image _blickImage;
    [SerializeField] private Image _image;

    private Coroutine _blickCoroutine;

    private void Start()
    {
        SetRandomSprite();

        _blickImage.color = new Color(1, 1, 1, 0);
        _backBlickImage.color = new Color(1, 1, 1, 0);
    }

    public void SetRandomSprite()
    {
        _image.sprite = _sprites[Random.Range(0, _sprites.Length)];
    }

    public void Blick(float start, float middle, float end, float maxIntesity)
    {
        if (_blickCoroutine != null)
        {
            return;
        }

        _blickCoroutine = StartCoroutine(BlickCoroutine(start, middle, end, maxIntesity));
    }

    private IEnumerator BlickCoroutine(float start, float middle, float end, float maxIntesity)
    {
        float time = 0;

        while (time < start)
        {
            _blickImage.color = new Color(1, 1, 1, Mathf.Lerp(0, maxIntesity, time / start));
            _backBlickImage.color = new Color(1, 1, 1, Mathf.Lerp(0, maxIntesity, time / start));
            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(middle);

        time = 0;

        while (time < end)
        {
            _blickImage.color = new Color(1, 1, 1, Mathf.Lerp(maxIntesity, 0, time / end));
            _backBlickImage.color = new Color(1, 1, 1, Mathf.Lerp(maxIntesity, 0, time / end));
            time += Time.deltaTime;
            yield return null;
        }

        _blickCoroutine = null;
    }
}

using System.Collections;
using TMPro;
using UnityEngine;

public class LoadAnimation : MonoBehaviour
{
    [SerializeField] private float _textAddPointTime;
    [Header("Text")]
    [SerializeField] private int _pointsCount;
    [SerializeField] private TMP_Text _text;
    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _rotationPoint;
    [Header("Players load")]
    [SerializeField] private float _playersLoadDelay;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private RectTransform _viewport;
    [SerializeField] private RectTransform _playersParent;

    public IEnumerator StartAnimation(int playersCount)
    {
        StartCoroutine(AnimateText());
        yield return AddPlayers(playersCount);
    }

    private void Update()
    {
        _rotationPoint.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
        MovePlayersParent();
    }

    private IEnumerator AnimateText()
    {
        string text = _text.text;
        var delay = new WaitForSeconds(_textAddPointTime);

        while (true)
        {
            _text.text = text;
            yield return delay;

            for (int i = 0; i < _pointsCount; i++)
            {
                _text.text += '.';
                yield return delay;
            }
        }
    }

    private IEnumerator AddPlayers(int playersCount)
    {
        ClearPlayers();
        var delay = new WaitForSeconds(_playersLoadDelay);

        for (int i = 0; i < playersCount; i++)
        {
            yield return delay;
            Instantiate(_playerPrefab, _playersParent);
        }

        yield return delay;
    }

    private void ClearPlayers()
    {
        foreach (Transform child in _playersParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void MovePlayersParent()
    {
        float playersY = _playersParent.rect.yMin + _playersParent.position.y;
        float viewportY = _viewport.rect.yMin + _viewport.position.y;

        if (playersY < viewportY)
        {
            _playersParent.anchoredPosition += Vector2.up * (viewportY - playersY);
        }
    }
}

using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class LoadAnimation : MonoBehaviour
{
    [SerializeField] private float _textAddPointTime;
    [Header("Text")]
    [SerializeField] private int _pointsCount;
    [SerializeField] private TMP_Text _text;
    [Header("Players load")]
    [SerializeField] private float _playersLoadDelay;
    [SerializeField] private float _gameLoadDelay;
    [SerializeField] private LoadingPlayer _playerPrefab;
    [SerializeField] private RectTransform _viewport;
    [SerializeField] private RectTransform _playersParent;

    public IEnumerator StartAnimation(int playersCount)
    {
        StartCoroutine(AnimateText());
        yield return AddPlayers(playersCount);
    }

    private void Update()
    {
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
        var lastDelay = new WaitForSeconds(_gameLoadDelay);

        for (int i = 0; i < playersCount; i++)
        {
            yield return delay;
            LoadingPlayer loadingPlayer = Instantiate(_playerPrefab, _playersParent);

            if (i == 0)
            {
                loadingPlayer.Initialize(RunSettings.PlayerGadget, isMainPlayer: true);
            }
            else
            {
                loadingPlayer.Initialize(RunSettings.EnemyGadgets.ToList()[i - 1], isMainPlayer: false);
            }
        }

        yield return lastDelay;
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

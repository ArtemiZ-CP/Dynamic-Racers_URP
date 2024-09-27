using UnityEngine;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private Place _placePrefab;
    [SerializeField] private RectTransform _board;
    [SerializeField] private RectTransform _viewport;

    private int _finishersCount = 0;

    public int AddFinisher(string name, Sprite gadgetSprite, bool isPlayerFinished)
    {
        _finishersCount++;
        Place place = Instantiate(_placePrefab, _board);
        place.SetPlace(name, _finishersCount, gadgetSprite);

        if (isPlayerFinished)
        {
            place.SetPlayerPlace();
        }

        return _finishersCount;
    }

    private void Update()
    {
        MovePanel();
    }

    private void MovePanel()
    {
        float playersY = _board.rect.yMin + _board.position.y;
        float viewportY = _viewport.rect.yMin + _viewport.position.y;

        if (playersY < viewportY)
        {
            _board.anchoredPosition += Vector2.up * (viewportY - playersY);
        }
    }
}

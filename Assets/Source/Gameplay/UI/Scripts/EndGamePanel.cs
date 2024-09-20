using UnityEngine;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private Place _placePrefab;
    [SerializeField] private RectTransform _board;
    [SerializeField] private RectTransform _viewport;

    public void AddFinisher(string name, int placement, Sprite gadgetSprite, bool isPlayerFinished)
    {
        Place place = Instantiate(_placePrefab, _board);
        place.gameObject.transform.SetSiblingIndex(placement - 1);
        place.SetPlace(name, placement, gadgetSprite);

        if (isPlayerFinished)
        {
            place.SetPlayerPlace();
        }
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

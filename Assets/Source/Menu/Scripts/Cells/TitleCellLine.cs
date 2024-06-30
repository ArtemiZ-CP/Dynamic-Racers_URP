using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TitleCellLine : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleText;

    private RectTransform _titleCell;

    public void Init(string title)
    {
        _titleText.text = title;
    }

    private void Awake()
    {
        _titleCell = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        Resize();
    }

    private void Resize()
    {
        float width = transform.parent.GetComponent<RectTransform>().rect.width;
        _titleCell.sizeDelta = new Vector2(width, _titleCell.sizeDelta.y);
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "MarginCellsContainer", menuName = "Containers/MarginCellsContainer")]
public class MarginCellsContainer : ScriptableObject
{
    [SerializeField] private GameObject _startRoadMargin;
    [SerializeField] private GameObject _secondRoadMargin;
    [SerializeField] private GameObject _middleRoadMargin;
    [SerializeField] private GameObject _startMiddleMargin;
    [SerializeField] private GameObject _secondMiddleMargin;
    [SerializeField] private GameObject _middleMiddleMargin;
    [SerializeField] private GameObject _startSideMargin;
    [SerializeField] private GameObject _middleSideMargin;
    [SerializeField] private GameObject _roadCornerMargin;

    public GameObject StartRoadMargin => _startRoadMargin;
    public GameObject SecondRoadMargin => _secondRoadMargin;
    public GameObject MiddleRoadMargin => _middleRoadMargin;
    public GameObject StartMiddleMargin => _startMiddleMargin;
    public GameObject SecondMiddleMargin => _secondMiddleMargin;
    public GameObject MiddleMiddleMargin => _middleMiddleMargin;
    public GameObject StartSideMargin => _startSideMargin;
    public GameObject MiddleSideMargin => _middleSideMargin;
    public GameObject RoadCornerMargin => _roadCornerMargin;
}

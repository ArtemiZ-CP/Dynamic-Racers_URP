using UnityEngine;

[CreateAssetMenu(fileName = "GroundCellsContainer", menuName = "Containers/GroundCellsContainer")]
public class GroundCellsContainer : ScriptableObject
{
    [SerializeField] private GameObject _startMiddle;
    [SerializeField] private GameObject _middleMiddle;
    [SerializeField] private GameObject _startSide;
    [SerializeField] private GameObject _middleSide;
    [SerializeField] private GameObject _roadMargin;
    [SerializeField] private GameObject _middleMargin;
    [SerializeField] private GameObject _sideMargin;
    [SerializeField] private GameObject _startRoadMargin;
    [SerializeField] private GameObject _startMiddleMargin;
    [SerializeField] private GameObject _startSideMargin;

    public GameObject StartMiddle => _startMiddle;
    public GameObject MiddleMiddle => _middleMiddle;
    public GameObject StartSide => _startSide;
    public GameObject MiddleSide => _middleSide;
    public GameObject RoadMargin => _roadMargin;
    public GameObject MiddleMargin => _middleMargin;
    public GameObject SideMargin => _sideMargin;
    public GameObject StartRoadMargin => _startRoadMargin;
    public GameObject StartMiddleMargin => _startMiddleMargin;
    public GameObject StartSideMargin => _startSideMargin;
}

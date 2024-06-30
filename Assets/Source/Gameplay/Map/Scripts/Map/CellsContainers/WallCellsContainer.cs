using UnityEngine;

[CreateAssetMenu(fileName = "WallCellsContainer", menuName = "Containers/WallCellsContainer")]
public class WallCellsContainer : ScriptableObject
{
    [SerializeField] private GameObject _startMiddle;
    [SerializeField] private GameObject _middleMiddle;
    [SerializeField] private GameObject _endMiddle;
    [SerializeField] private GameObject _startSide;
    [SerializeField] private GameObject _middleSide;
    [SerializeField] private GameObject _endSide;
    [SerializeField] private GameObject _startRoadMargin;
    [SerializeField] private GameObject _startMiddleMargin;
    [SerializeField] private GameObject _startSideMargin;
    [SerializeField] private GameObject _roadMargin;
    [SerializeField] private GameObject _middleMargin;
    [SerializeField] private GameObject _sideMargin;
    [SerializeField] private GameObject _endRoadMargin;
    [SerializeField] private GameObject _endMiddleMargin;
    [SerializeField] private GameObject _endSideMargin;

    public GameObject StartMiddle => _startMiddle;
    public GameObject MiddleMiddle => _middleMiddle;
    public GameObject EndMiddle => _endMiddle;
    public GameObject StartSide => _startSide;
    public GameObject MiddleSide => _middleSide;
    public GameObject EndSide => _endSide;
    public GameObject StartRoadMargin => _startRoadMargin;
    public GameObject StartMiddleMargin => _startMiddleMargin;
    public GameObject StartSideMargin => _startSideMargin;
    public GameObject RoadMargin => _roadMargin;
    public GameObject MiddleMargin => _middleMargin;
    public GameObject SideMargin => _sideMargin;
    public GameObject EndRoadMargin => _endRoadMargin;
    public GameObject EndMiddleMargin => _endMiddleMargin;
    public GameObject EndSideMargin => _endSideMargin;
}

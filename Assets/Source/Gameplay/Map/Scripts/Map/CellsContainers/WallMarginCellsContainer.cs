using UnityEngine;

[CreateAssetMenu(fileName = "WallMarginCellsContainer", menuName = "Containers/WallMarginCellsContainer")]
public class WallMarginCellsContainer : ScriptableObject
{
    [SerializeField] private GameObject _startRoadMargin;
    [SerializeField] private GameObject _startMiddleMargin;
    [SerializeField] private GameObject _startSideMargin;
    [SerializeField] private GameObject _middleRoadMargin;
    [SerializeField] private GameObject _middleMiddleMargin;
    [SerializeField] private GameObject _middleSideMargin;
    [SerializeField] private GameObject _endRoadMargin;
    [SerializeField] private GameObject _endMiddleMargin;
    [SerializeField] private GameObject _endSideMargin;
    
    public GameObject StartRoadMargin => _startRoadMargin;
    public GameObject StartMiddleMargin => _startMiddleMargin;
    public GameObject StartSideMargin => _startSideMargin;
    public GameObject MiddleRoadMargin => _middleRoadMargin;
    public GameObject MiddleMiddleMargin => _middleMiddleMargin;
    public GameObject MiddleSideMargin => _middleSideMargin;
    public GameObject EndRoadMargin => _endRoadMargin;
    public GameObject EndMiddleMargin => _endMiddleMargin;
    public GameObject EndSideMargin => _endSideMargin;
}

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

    public GameObject StartMiddle => _startMiddle;
    public GameObject MiddleMiddle => _middleMiddle;
    public GameObject EndMiddle => _endMiddle;
    public GameObject StartSide => _startSide;
    public GameObject MiddleSide => _middleSide;
    public GameObject EndSide => _endSide;
}

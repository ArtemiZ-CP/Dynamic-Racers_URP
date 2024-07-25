using UnityEngine;

[CreateAssetMenu(fileName = "GroundCellsContainer", menuName = "Containers/GroundCellsContainer")]
public class GroundCellsContainer : ScriptableObject
{
    [SerializeField] private GameObject _startMiddle;
    [SerializeField] private GameObject _middleMiddle;
    [SerializeField] private GameObject _startSide;
    [SerializeField] private GameObject _middleSide;

    public GameObject StartMiddle => _startMiddle;
    public GameObject MiddleMiddle => _middleMiddle;
    public GameObject StartSide => _startSide;
    public GameObject MiddleSide => _middleSide;
}

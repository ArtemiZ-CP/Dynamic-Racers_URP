using UnityEngine;

[CreateAssetMenu(fileName = "FinishCellsContainer", menuName = "Containers/FinishCellsContainer")]
public class FinishCellsContainer : ScriptableObject
{
    [SerializeField] private GameObject _middleLine;
    [SerializeField] private GameObject _middle;
    [SerializeField] private GameObject _leftLine;
    [SerializeField] private GameObject _rightLine;
    [SerializeField] private GameObject _finishObject;
    
    public GameObject MiddleLine => _middleLine;
    public GameObject Middle => _middle;
    public GameObject LeftLine => _leftLine;
    public GameObject RightLine => _rightLine;
    public GameObject FinishObject => _finishObject;
}

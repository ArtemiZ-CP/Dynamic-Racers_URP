using UnityEngine;

[CreateAssetMenu(fileName = "BasisCellsContainer", menuName = "Containers/BasisCellsContainer")]
public class BasisCellsContainer : ScriptableObject
{
    [SerializeField] private GameObject _edgeCorner;
    [SerializeField] private GameObject _edgeMiddle;
    [SerializeField] private GameObject _corner;
    [SerializeField] private GameObject _middle;

    public GameObject EdgeCorner => _edgeCorner;
    public GameObject EdgeMiddle => _edgeMiddle;
    public GameObject Corner => _corner;
    public GameObject Middle => _middle;
}

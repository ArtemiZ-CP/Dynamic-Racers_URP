using UnityEngine;

[CreateAssetMenu(fileName = "MapCellsContainer", menuName = "Containers/MapCellsContainer")]
public class MapCellsContainer : ScriptableObject
{
    [SerializeField] private BasisCellsContainer _basisCellsContainer;
    [SerializeField] private GroundCellsContainer _groundCellsContainer;
    [SerializeField] private WallCellsContainer _wallCellsContainer;
    [SerializeField] private WaterpoolCellsContainer _waterpoolCellsContainer;

    public BasisCellsContainer BasisCellsContainer => _basisCellsContainer;
    public GroundCellsContainer GroundCellsContainer => _groundCellsContainer;
    public WallCellsContainer WallCellsContainer => _wallCellsContainer;
    public WaterpoolCellsContainer WaterpoolCellsContainer => _waterpoolCellsContainer;
}

using UnityEngine;

[CreateAssetMenu(fileName = "MapCellsContainer", menuName = "Containers/MapCellsContainer")]
public class MapCellsContainer : ScriptableObject
{
    [SerializeField] private BasisCellsContainer _basisCellsContainer;
    [SerializeField] private GroundCellsContainer _groundCellsContainer;
    [SerializeField] private WallCellsContainer _wallCellsContainer;
    [SerializeField] private WaterpoolCellsContainer _waterpoolCellsContainer;
    [SerializeField] private MarginCellsContainer _marginCellsContainer;
    [SerializeField] private WallMarginCellsContainer _wallMarginCellsContainer;
    [SerializeField] private EnvironmentContainer _environmentContainer;
    [SerializeField] private FinishCellsContainer _finishCellsContainer;

    public BasisCellsContainer BasisCellsContainer => _basisCellsContainer;
    public GroundCellsContainer GroundCellsContainer => _groundCellsContainer;
    public WallCellsContainer WallCellsContainer => _wallCellsContainer;
    public WaterpoolCellsContainer WaterpoolCellsContainer => _waterpoolCellsContainer;
    public MarginCellsContainer MarginCellsContainer => _marginCellsContainer;
    public WallMarginCellsContainer WallMarginCellsContainer => _wallMarginCellsContainer;
    public EnvironmentContainer EnvironmentContainer => _environmentContainer;
    public FinishCellsContainer FinishCellsContainer => _finishCellsContainer;
}

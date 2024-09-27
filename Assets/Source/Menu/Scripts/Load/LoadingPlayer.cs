using TMPro;
using UnityEngine;

public class LoadingPlayer : MonoBehaviour
{
    [SerializeField] private GadgetCollectionCell _gadgetCollectionCell;
    [SerializeField] private TMP_Text _playerName;

    public void Initialize(Gadget gadget, bool isMainPlayer)
    {
        _gadgetCollectionCell.Initialize(gadget);
    }
}

using System.Linq;
using UnityEngine;

public class RewardGadgetMenu : ShowRewardMenu
{
    [SerializeField] private GadgetCollectionCell _gadgetCollectionCell;

    public void Show(GadgetReward gadgetReward)
    {
        gameObject.SetActive(true);
        Gadget gadget = PlayerData.PlayerGadgets.FirstOrDefault(g => g.ScriptableObject == gadgetReward.ScriptableObject);

        if (gadget != null)
        {
            _gadgetCollectionCell.gameObject.SetActive(true);
            _gadgetCollectionCell.Initialize(gadget);
        }
        else
        {
            throw new System.Exception("Gadget not found");
        }
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }
}

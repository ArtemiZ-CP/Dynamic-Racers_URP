using System.Collections.Generic;

public class EnemyGadgets : CharacterGadgets
{
    protected override void Awake()
    {
        base.Awake();

        if (RunSettings.PlayerGadget != null)
        {
            List<Rare> rares = new();

            for (int i = 0; i <= (int)RunSettings.MaxEnemyGadget; i++)
            {
                rares.Add((Rare)i);
            }

            Init(new Gadget(GlobalSettings.Instance.GetRandomGadget(rares.ToArray()), level: RunSettings.PlayerGadget.Level));
        }
    }
}

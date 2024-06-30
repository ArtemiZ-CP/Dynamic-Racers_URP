using System.Collections.Generic;

public static class RunSettings
{
    public static GadgetScriptableObject PlayerGadget { get; set; }
    public static List<ChunkSettings> Map { get; set; }
    public static MapCellsContainer MapSetting { get; set; }
    public static float ExperienceReward { get; set; }

    public static void Reset()
    {
        PlayerGadget = null;
        Map = null;
        MapSetting = null;
        ExperienceReward = 0;
    }
}

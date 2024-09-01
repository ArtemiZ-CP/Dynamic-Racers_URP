using System.Collections.Generic;

public static class RunSettings
{
    public static Gadget PlayerGadget { get; set; }
    public static List<ChunkSettings> Map { get; set; }
    public static MapCellsContainer MapCellsContainer { get; set; }
    public static int PlayersCount { get; set; }
    public static int ExperienceReward { get; set; }

    public static void Reset()
    {
        PlayerGadget = null;
        Map = null;
        MapCellsContainer = null;
        PlayersCount = 0;
        ExperienceReward = 0;
    }
}

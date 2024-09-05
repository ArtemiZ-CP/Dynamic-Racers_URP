public class TrainingRunStages : RunStagesBase
{
    protected override void GiveReward()
    {
        PlayerData.TrainingPassed();
    }
}

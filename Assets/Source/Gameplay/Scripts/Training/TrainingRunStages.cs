using UnityEngine;

public class TrainingRunStages : RunStagesBase
{
    [Header("Enemy Upgrades")]
    [SerializeField] private int _enemyUpgrades;

    protected override int GetEnemyUpgrades(CharacteristicType characteristicType)
    {
        return _enemyUpgrades;
    }

    protected override void GiveReward()
    {
        PlayerProgress.TrainingPassed();
    }
}

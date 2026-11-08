using Domain.Incentives;
using UnityEngine;

namespace Domain.Quests
{
    public class QuestGenerator
    {
        public Quest Generate()
        {
            return new Quest
            {
                DaysToComplete = Random.Range(1, 4),
                Requirement = Random.Range(1, 40) * 10,
                Reward = new ArmyIncentive(Random.Range(1, 20) * 10),
                Penalty = Random.value < .1f 
                    ? new ArmyIncentive(Random.Range(-20, 1) * 10)
                    : new LivesIncentive(-1),
                Region = Region.Fields
            };
        }
    }
}
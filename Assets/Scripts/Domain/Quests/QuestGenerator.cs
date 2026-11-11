using Domain.Incentives;
using UnityEngine;

namespace Domain.Quests
{
    public class QuestGenerator
    {
        public static Quest Generate()
        {
            return new Quest
            {
                DaysToComplete = Random.Range(1, 4),
                Requirement = Random.Range(1, 4) * 100,
                Reward = IncentiveGenerator.GenerateReward(),
                Penalty = IncentiveGenerator.GeneratePenalty(),
                Region = Region.Orchid
            };
        }
    }
}
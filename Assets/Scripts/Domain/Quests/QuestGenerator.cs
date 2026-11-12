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
                Reward = new () {IncentiveGenerator.GenerateReward()},
                Penalty = new () { IncentiveGenerator.GeneratePenalty() },
                Region = Region.Orchid
            };
        }
    }
}
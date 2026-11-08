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
                Reward = new CurrencyIncentive(Random.Range(1, 20) * 10, Currency.ArmySize),
                Penalty = Random.value < .1f 
                    ? new CurrencyIncentive(Random.Range(-20, 1) * 10, Currency.ArmySize)
                    : new CurrencyIncentive(-1, Currency.Lives),
                Region = Region.Fields
            };
        }
    }
}
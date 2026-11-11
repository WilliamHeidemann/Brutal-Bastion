using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Incentives;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Domain.Quests
{
    public class Quest
    {
        public string Title;
        public string Description;
        public int DaysToComplete;
        public int Requirement;
        public Incentive Reward;
        public Incentive Penalty;
        public Region Region;
    }
    
    [CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest")]
    public class QuestObject : ScriptableObject
    {
        public string Title;
        public string Description;
        public Region Region;
        public int DrawWeight = 10;
        public bool Repeatable = true;
        public QuestCondition Condition;
        public Range Cost = new Range{Min = 100, Max = 300, steps = 50};
        public Range DaysToComplete = new Range{Min = 1, Max = 3};
        public Consequence Success;
        public Consequence Failure;

        public Quest Convert()
        {
            return new Quest
            {
                Title = Title,
                Description = Description,
                Region = Region.Resolve(),
                DaysToComplete = DaysToComplete.Collapse(),
                Requirement = Cost.Collapse(),
                Reward = new CurrencyIncentive(Success.Soldiers.Collapse(), Currency.ArmySize),
                Penalty = new CurrencyIncentive(Failure.Soldiers.Collapse(), Currency.ArmySize),
            };
        }

        public bool IsValid(GameState state)
        {
            return Condition.Test(state);
        }
    }

    [CreateAssetMenu(fileName = "QuestList", menuName = "Quests/Quest List")]
    public class QuestList : ScriptableObject
    {
        public List<QuestObject> Quests;
    }

    [Serializable]
    public class Range
    {
        public int Min;
        public int Max;
        public int steps = 1;

        /*
         * Collapses range into a random number in the range (both inclusive).
         */
        public int Collapse()
        {
            int steprange = (Max-Min) / steps;
            return Random.Range(0, steprange)*steps + steps*Min;
        }
    }

    [Serializable]
    public class QuestCondition
    {
        public Range Days = new Range{Min = -1, Max = -1};
        public List<string> Flags;

        public bool Test(GameState state)
        {
            // There is a minimum day on a quest, and at least that many days have elapsed.
            if (Days.Min <= 0 && Days.Min <= state.Day)
                return false;
            // There is a maximum day on a quest, and at most that many days have elapsed.
            if (Days.Max <= 0 && Days.Max >= state.Day)
                return false;
            return Flags.All(flag => state.flags.Contains(flag));
        }
    }

    [Serializable]
    public class Consequence
    {
        public Range Soldiers = new Range{Min = 50,  Max = 300, steps = 25};
        public Range Lives = new Range{Min = 0, Max = 3};
        public List<string> FlagstoAdd;
        public List<string> FlagstoRemove;
    }
}
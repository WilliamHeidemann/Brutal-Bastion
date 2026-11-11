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
    
    [CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/Quest")]
    public class QuestObject : ScriptableObject
    {
        public string Title;
        public string Description;
        public bool Repeatable = true;
        public QuestCondition Condition;
        public Range DaysToComplete;

        public Quest Convert()
        {
            return new Quest
            {
                Title = Title,
                Description = Description,
                DaysToComplete = DaysToComplete.Collapse(),
            };
        }
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
}
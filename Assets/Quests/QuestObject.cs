using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Incentives;
using Domain.Quests;
using UnityEngine;
using Random = UnityEngine.Random;

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
            Penalty = new CurrencyIncentive(-Failure.Soldiers.Collapse(), Currency.ArmySize),
        };
    }

    public bool IsValid(GameState state)
    {
        return Condition.Test(state);
    }
}

public static class QuestExtensions
{
    public static List<QuestObject> Prune(this IEnumerable<QuestObject> quests, PlayCoordinator playCoordinator)
    {
        return quests.Where(playCoordinator.IsQuestValid).ToList();
    }

    public static int WeightSum(this IEnumerable<QuestObject> quests)
    {
        return quests.Sum(q => q.DrawWeight);
    }

    public static QuestObject Draw(this IEnumerable<QuestObject> quests)
    {
        
        int weightSum = quests.WeightSum();
        int lottery = Random.Range(0, weightSum) + 1;
        foreach (var quest in quests)
        {
            lottery -= quest.DrawWeight;
            if (lottery <= 0)
                return quest;
        }

        throw new IndexOutOfRangeException("Failed to draw quest. Weights were exhausted without a valid draw.");
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
        return Random.Range(0, steprange)*steps + Min;
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
        if (Days.Min >= 0 && Days.Min <= state.Day)
            return false;
        // There is a maximum day on a quest, and at most that many days have elapsed.
        if (Days.Max >= 0 && Days.Max >= state.Day)
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
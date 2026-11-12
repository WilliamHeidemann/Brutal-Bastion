using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Domain;
using Domain.Incentives;
using Domain.Modifiers;
using Domain.Quests;
using UnityEngine;
using Attribute = Domain.Modifiers.Attribute;
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
        List<Incentive> reward = new List<Incentive>();
        int soldiersSuccess = Success.Soldiers.Collapse();
        if (soldiersSuccess != 0)
            reward.Add(new CurrencyIncentive(soldiersSuccess, Currency.ArmySize));
        int successLives = Success.Lives.Collapse();
        if (successLives != 0)
            reward.Add(new CurrencyIncentive(successLives, Currency.Lives));
        reward.AddRange(Success.Arcanas.Select(a => a.GetModifier()));

        List<Incentive> penalty = new List<Incentive>();
        int soldiersFail = Failure.Soldiers.Collapse();
        if (soldiersFail != 0)
            penalty.Add(new CurrencyIncentive(-soldiersFail, Currency.ArmySize));
        int failLives = Failure.Lives.Collapse();
        if (failLives != 0)
            penalty.Add(new CurrencyIncentive(-failLives, Currency.Lives));
        penalty.AddRange(Failure.Arcanas.Select(a => a.GetModifier()));
        
        return new Quest
        {
            Title = Title,
            Description = Description,
            Region = Region.Resolve(),
            DaysToComplete = DaysToComplete.Collapse(),
            Requirement = Cost.Collapse(),
            Reward = reward,
            Penalty = penalty,
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

    public static string GetDescription(this IEnumerable<Incentive> incentives)
    {
        return string.Join("\n", incentives.Select(i => i.GetDescription()));
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
        if (steps == 0)
            steps = 1;
        int steprange = (Max-Min) / steps;
        return Random.Range(0, steprange)*steps + Min;
    }
}

[Serializable]
public class QuestCondition
{
    public Range Days = new Range{Min = -1, Max = -1};
    public Range Lives = new Range{Min = -1, Max = -1};
    public List<string> Flags;

    public bool Test(GameState state)
    {
        // There is a minimum day on a quest, and at least that many days have elapsed.
        if (Days.Min >= 0 && Days.Min <= state.Day)
            return false;
        // There is a maximum day on a quest, and at most that many days have elapsed.
        if (Days.Max >= 0 && Days.Max >= state.Day)
            return false;
        // There is a minimum life on a quest, and the player has at least that many lives.
        if (Lives.Min >= 0 && Lives.Min <= state.Lives)
            return false;
        // There is a maximum life on a quest, and the player has at most that many lives.
        if (Lives.Max >= 0 && Lives.Max >= state.Lives)
            return false;
        return Flags.All(flag => state.flags.Contains(flag));
    }
}

[Serializable]
public class Consequence
{
    public Range Soldiers = new Range{Min = 50,  Max = 300, steps = 25};
    public Range Lives = new Range{Min = 0, Max = 3};
    public List<Arcana> Arcanas = new List<Arcana>();
    public List<string> FlagstoAdd;
    public List<string> FlagstoRemove;
}

[Serializable]
public class Arcana
{
    public ArcanaType type;
    public TimeOfEffect timeOfEffect;
    public Range magnitude = new Range{Min = -1, Max = 1};
    public Range duration = new Range{Min = 1, Max = 3};
    public List<Region> regions = new List<Region>();
    public Range regionAmount = new Range{Min = 1, Max = 1};
    
    public ModifierIncentive GetModifier()
    {
        int duration = this.duration.Collapse();
        if (duration == 0)
            duration = 1;
        
        int magnitude = this.magnitude.Collapse();
        List<Region> regions = this.regions.Resolve();
        
        int regionAmount = this.regionAmount.Collapse();
        if (regionAmount == 0)
            regionAmount = 1;
        regionAmount = Math.Min(regionAmount, regions.Count);

        while (regions.Count > regionAmount)
        {
            int i = Random.Range(0, regions.Count);
            regions.RemoveAt(i);
        }


        ValueChange valueChange = new ValueChange
        {
            Attribute = type.GetAttribute(),
            ChangeType = type.GetChangeType(),
            TimeOfEffect = timeOfEffect,
            Value = magnitude,
        };

        IncentiveModifier mod = new IncentiveModifier
        {
            Changes = new List<ValueChange> { valueChange },
            Duration = duration == -1 ? new InfiniteDuration() : new LimitedDuration { TurnsLeft = duration },
            ToAffect = regions,
        };

        return new ModifierIncentive(mod);
    }
}

public enum ArcanaType
{
    Soldiers,
    SoldiersPercentage,
    Lives,
    LivesPercentage,
}

public static class ArcanaExtensions
{
    public static Attribute GetAttribute(this ArcanaType type) => type switch
    {
        ArcanaType.Soldiers => Attribute.ArmySize,
        ArcanaType.Lives => Attribute.Lives,
        _ => Attribute.ArmySize,
    };

    public static ChangeType GetChangeType(this ArcanaType type) => type switch
    {
        ArcanaType.Soldiers => ChangeType.Value,
        ArcanaType.Lives => ChangeType.Value,
        ArcanaType.SoldiersPercentage => ChangeType.Percentage,
        ArcanaType.LivesPercentage => ChangeType.Percentage,
        _ => ChangeType.Value,
    };
}
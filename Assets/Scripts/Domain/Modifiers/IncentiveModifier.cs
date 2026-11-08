using System.Collections.Generic;

namespace Domain.Modifiers
{
    public class IncentiveModifier
    {
        public List<Region> ToAffect = new();
        public List<ValueChange> Changes = new();
        public Duration Duration;
    }

    public class ValueChange
    {
        public Attribute Attribute;
        public ChangeType ChangeType;
        public TimeOfEffect TimeOfEffect;
        public int Value;
    }

    public enum Attribute
    {
        Lives,
        ArmySize,
        TurnRequirement
    }

    public enum ChangeType
    {
        Value,
        Percentage
    }

    public enum TimeOfEffect
    {
        OnPickUp,
        OnReward,
        OnPenalty
    }

    public abstract class Duration
    {
    }

    public class InfiniteDuration : Duration
    {
    }

    public class LimitedDuration : Duration
    {
        public int TurnsLeft;
    }
}
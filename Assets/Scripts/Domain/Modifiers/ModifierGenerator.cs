using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Domain.Modifiers
{
    public class ModifierGenerator
    {
        public IncentiveModifier Generate()
        {
            return new IncentiveModifier
            {
                ToAffect = GenerateRegions(),
                Changes = GenerateChanges(),
                Duration = Random.value < .5f 
                    ? new LimitedDuration
                    {
                        TurnsLeft = Random.Range(1, 3)
                    }
                    : new InfiniteDuration(),
            };
        }

        private static List<Region> GenerateRegions()
        {
            List<Region> allRegions = Enum.GetValues(typeof(Region)).Cast<Region>().ToList();

            if (Random.value < .35f)
            {
                return allRegions;
            }

            int count = Random.Range(1, allRegions.Count + 1);
            return allRegions.OrderBy(_ => Random.value).Take(count).ToList();
        }

        private static List<ValueChange> GenerateChanges()
        {
            int count = Random.Range(1, 3);
            List<ValueChange> changes = new(count);

            for (int i = 0; i < count; i++)
            {
                Attribute attribute = Random.value < .5f ? Attribute.Lives : Attribute.ArmySize;
                ChangeType changeType = Random.value < .7f ? ChangeType.Value : ChangeType.Percentage;

                changes.Add(new ValueChange
                {
                    Attribute = attribute,
                    ChangeType = changeType,
                    TimeOfEffect = RandomEnum<TimeOfEffect>(),
                    Value = GenerateChangeValue(attribute, changeType)
                });
            }

            return changes;
        }

        private static int GenerateChangeValue(Attribute attribute, ChangeType changeType)
        {
            if (changeType == ChangeType.Percentage)
            {
                int divisor = Random.Range(10, 41);
                return Random.value < .5f ? -divisor : divisor;
            }

            return attribute switch
            {
                Attribute.Lives => Random.value < .5f ? -1 : 1,
                Attribute.ArmySize => Random.Range(-8, 9) * 10,
                _ => Random.Range(-10, 11)
            };
        }

        private static T RandomEnum<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(Random.Range(0, values.Length));
        }
    }
}
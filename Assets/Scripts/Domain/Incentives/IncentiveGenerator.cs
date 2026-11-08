using Domain.Modifiers;
using UnityEngine;

namespace Domain.Incentives
{
    public static class IncentiveGenerator
    {
        public static Incentive GenerateReward()
        {
            if (Random.value < 0.3)
            {
                IncentiveModifier modifier = ModifierGenerator.Generate();
                return new ModifierIncentive(modifier);
            }
            else
            {
                return GenerateRewardCurrencyIncentive();
            }
        }

        public static Incentive GeneratePenalty()
        {
            if (Random.value < 0.3)
            {
                IncentiveModifier modifier = ModifierGenerator.Generate();
                return new ModifierIncentive(modifier);
            }
            else
            {
                return GeneratePenaltyCurrencyIncentive();
            }
        }

        private static CurrencyIncentive GenerateRewardCurrencyIncentive()
        {
            return new CurrencyIncentive(Random.Range(1, 20) * 10, Currency.ArmySize);
        }
        
        private static CurrencyIncentive GeneratePenaltyCurrencyIncentive()
        {
            return Random.value < .9f
                ? new CurrencyIncentive(Random.Range(-20, 0) * 10, Currency.ArmySize)
                : new CurrencyIncentive(-1, Currency.Lives);
        }
    }
}
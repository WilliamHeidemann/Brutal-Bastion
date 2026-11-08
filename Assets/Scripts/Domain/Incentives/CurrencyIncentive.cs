using System;
using UnityEngine;

namespace Domain.Incentives
{
    public class CurrencyIncentive : Incentive
    {
        public int Amount { get; }
        public Currency Currency { get; }

        public CurrencyIncentive(int amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public override void Modify(GameState gameState)
        {
            switch (Currency)
            {
                case Currency.Lives:
                    gameState.Lives = Mathf.Max(gameState.Lives + Amount, 0);
                    break;
                case Currency.ArmySize:
                    gameState.ArmySize = Mathf.Max(gameState.ArmySize + Amount, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string GetDescription() => Currency switch
        {
            Currency.Lives => Amount > 0 ? $"Gain {Amount} life." : $"Lose {Mathf.Abs(Amount)} life.",
            Currency.ArmySize => Amount > 0 ? $"Gain {Amount} army size." : $"Lose {Mathf.Abs(Amount)} army size.",
            _ => string.Empty
        };
    }

    public enum Currency
    {
        ArmySize,
        Lives
    }
}
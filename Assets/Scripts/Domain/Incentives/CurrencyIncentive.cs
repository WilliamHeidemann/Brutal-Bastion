using System;
using Domain.Utility;
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
            Currency.Lives => Amount > 0 ? $"Gain {Descriptions.FormatLives(Amount)}" : $"Lose {Descriptions.FormatLives(Mathf.Abs(Amount))}",
            Currency.ArmySize => Amount > 0 ? $"Gain {Descriptions.FormatSoldiers(Amount)}" : $"Lose {Descriptions.FormatSoldiers(Mathf.Abs(Amount))}",
            _ => string.Empty
        };
    }

    public enum Currency
    {
        ArmySize,
        Lives
    }
}
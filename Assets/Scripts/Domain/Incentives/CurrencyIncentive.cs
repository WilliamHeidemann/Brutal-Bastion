using System;
using UnityEngine;

namespace Domain.Incentives
{
    public class CurrencyIncentive
    {
        private readonly int _amount;
        private readonly Currency _currency;

        public CurrencyIncentive(int amount, Currency currency)
        {
            _amount = amount;
            _currency = currency;
        }

        public void Modify(GameState gameState)
        {
            switch (_currency)
            {
                case Currency.Lives:
                    gameState.Lives = Mathf.Max(gameState.Lives + _amount, 0);
                    break;
                case Currency.ArmySize:
                    gameState.ArmySize = Mathf.Max(gameState.ArmySize + _amount, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string GetDescription() => _currency switch
        {
            Currency.Lives => _amount > 0 ? $"Gain {_amount} life." : $"Lose {_amount} life.",
            Currency.ArmySize => _amount > 0 ? $"Gain {_amount} army size." : $"Lose {_amount} army size.",
            _ => string.Empty
        };
    }

    public enum Currency
    {
        ArmySize,
        Lives
    }
}
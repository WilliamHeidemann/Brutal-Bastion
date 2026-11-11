using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Incentives;
using Domain.Modifiers;
using Domain.Quests;
using UnityEngine;
using Attribute = Domain.Modifiers.Attribute;

namespace Domain
{
    public class PlayCoordinator
    {
        private readonly ActiveQuests _activeQuests = new();
        private readonly OfferedQuests _offeredQuests = new();
        private readonly GameState _gameState = new();

        public Action OnOutOfLives;
        public Action OnArmySizeChange;
        public Action OnNewDay;
        public Action OnLivesChanged;
        public Action OnQuestsChanged;

        public void SetQuestsForTheDay(List<Quest> quests)
        {
            _offeredQuests.Clear();
            foreach (Quest quest in quests)
            {
                _offeredQuests.Add(quest);
            }
        }

        public void Accept(Quest quest)
        {
            _offeredQuests.Accept(quest);
            if (quest.Requirement != 0)
                OnArmySizeChange?.Invoke();
        }

        public void Decline(Quest quest)
        {
            _offeredQuests.Decline(quest);
            if (quest.Requirement != 0)
                OnArmySizeChange?.Invoke();
        }

        public void PassDay()
        {
            PayForAcceptedQuests();

            ApplyRewards();

            ApplyPenalties();
            
            OnLivesChanged?.Invoke();

            if (_gameState.Lives <= 0)
            {
                OnOutOfLives?.Invoke();
            }

            TickModifiers();
            _offeredQuests.Clear();
            OnNewDay?.Invoke();
        }

        private void PayForAcceptedQuests()
        {
            foreach (Quest acceptedQuest in _offeredQuests.AcceptedQuests)
            {
                List<ValueChange> pickUpChanges = _gameState.Modifiers
                    .Where(m => m.ToAffect.Contains(acceptedQuest.Region))
                    .SelectMany(m => m.Changes)
                    .Where(vc => vc.TimeOfEffect == TimeOfEffect.OnPickUp)
                    .ToList();

                float lifeCostBonus = pickUpChanges
                    .Where(vc => vc.Attribute == Attribute.Lives)
                    .Sum(vc => Bonus(vc, acceptedQuest.Requirement));

                _gameState.Lives = Mathf.Max(0, _gameState.Lives - (int)lifeCostBonus);

                float armyCostBonus = pickUpChanges
                    .Where(vc => vc.Attribute == Attribute.ArmySize)
                    .Sum(vc => Bonus(vc, acceptedQuest.Requirement));

                _gameState.ArmySize =
                    Mathf.Max(0, _gameState.ArmySize - (int)armyCostBonus - acceptedQuest.Requirement);

                _activeQuests.Add(acceptedQuest);
            }
        }

        private static float Bonus(ValueChange valueChange, int baseAmount)
        {
            return valueChange.ChangeType switch
            {
                ChangeType.Value => valueChange.Value,
                ChangeType.Percentage => 1f / valueChange.Value * baseAmount,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void ApplyRewards()
        {
            foreach (Quest completedQuest in _activeQuests.PassDay())
            {
                _gameState.ArmySize += completedQuest.Requirement;
                completedQuest.Reward.Modify(_gameState);

                if (completedQuest.Reward is CurrencyIncentive currencyIncentive)
                {
                    GainBonuses(currencyIncentive, completedQuest);
                }

                if (completedQuest.Reward is ModifierIncentive modifierIncentive)
                {
                    modifierIncentive.Modify(_gameState);
                }
            }
        }

        private void GainBonuses(CurrencyIncentive currencyIncentive, Quest completedQuest)
        {
            List<ValueChange> rewardChanges = _gameState.Modifiers
                .Where(m => m.ToAffect.Contains(completedQuest.Region))
                .SelectMany(m => m.Changes)
                .Where(vc => vc.TimeOfEffect == TimeOfEffect.OnReward)
                .ToList();

            float lifeBonus = rewardChanges
                .Where(vc => vc.Attribute == Attribute.Lives)
                .Sum(vc => Bonus(vc, currencyIncentive.Amount));

            _gameState.Lives = Mathf.Max(0, _gameState.Lives + (int)lifeBonus);

            float armyBonus = rewardChanges
                .Where(vc => vc.Attribute == Attribute.ArmySize)
                .Sum(vc => Bonus(vc, currencyIncentive.Amount));

            _gameState.ArmySize = Mathf.Max(0, _gameState.ArmySize + (int)armyBonus);
        }

        private void ApplyPenalties()
        {
            foreach (Quest declinedQuest in _offeredQuests.DeclinedQuests)
            {
                declinedQuest.Penalty.Modify(_gameState);

                if (declinedQuest.Penalty is CurrencyIncentive currencyIncentive)
                {
                    GainPenaltyBonuses(currencyIncentive, declinedQuest);
                }

                if (declinedQuest.Penalty is ModifierIncentive modifierIncentive)
                {
                    modifierIncentive.Modify(_gameState);
                }
            }
        }

        private void GainPenaltyBonuses(CurrencyIncentive currencyIncentive, Quest declinedQuest)
        {
            List<ValueChange> penaltyChanges = _gameState.Modifiers
                .Where(m => m.ToAffect.Contains(declinedQuest.Region))
                .SelectMany(m => m.Changes)
                .Where(vc => vc.TimeOfEffect == TimeOfEffect.OnPenalty)
                .ToList();

            float lifeBonus = penaltyChanges
                .Where(vc => vc.Attribute == Attribute.Lives)
                .Sum(vc => Bonus(vc, currencyIncentive.Amount));

            _gameState.Lives = Mathf.Max(0, _gameState.Lives + (int)lifeBonus);

            float armyBonus = penaltyChanges
                .Where(vc => vc.Attribute == Attribute.ArmySize)
                .Sum(vc => Bonus(vc, currencyIncentive.Amount));

            _gameState.ArmySize = Mathf.Max(0, _gameState.ArmySize + (int)armyBonus);
        }

        private void TickModifiers()
        {
            List<IncentiveModifier> expired = new();

            foreach (IncentiveModifier modifier in _gameState.Modifiers)
            {
                if (modifier.Duration is LimitedDuration limitedDuration)
                {
                    limitedDuration.TurnsLeft -= 1;

                    if (limitedDuration.TurnsLeft <= 0)
                    {
                        expired.Add(modifier);
                    }
                }
            }

            foreach (IncentiveModifier modifier in expired)
            {
                _gameState.Modifiers.Remove(modifier);
            }
        }

        public int GetCommitedSoldiers()
        {
            return _offeredQuests.AcceptedQuests.Sum(q => q.Requirement);
        }

        public int GetArmySize()
        {
            return _gameState.ArmySize;
        }
        
        public int GetLives()
        {
            return _gameState.Lives;
        }

        public IEnumerable<Quest> GetOfferedQuests() => _offeredQuests.AllQuests;

        public bool IsQuestValid(QuestObject quest)
        {
            return quest.IsValid(_gameState);
        }
    }
}
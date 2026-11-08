using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        public void Decline(Quest quest)
        {
            _offeredQuests.Decline(quest);
        }

        public void PassDay()
        {
            PayForAcceptedQuests();
            
            ApplyRewards();
            
            ApplyPenalties();

            if (_gameState.Lives <= 0)
            {
                OnOutOfLives?.Invoke();
            }

            TickModifiers();
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

                float lifeCost = pickUpChanges
                    .Where(vc => vc.Attribute == Attribute.Lives)
                    .Sum(vc => Bonus(vc, acceptedQuest.Requirement));
                
                _gameState.Lives = Mathf.Max(0, _gameState.Lives - (int)lifeCost);

                float armyCost = pickUpChanges
                    .Where(vc => vc.Attribute == Attribute.ArmySize)
                    .Sum(vc => Bonus(vc, acceptedQuest.Requirement));

                _gameState.ArmySize = Mathf.Max(0, _gameState.ArmySize - (int)armyCost);

                _activeQuests.Add(acceptedQuest);
            }
        }

        private static float Bonus(ValueChange valueChange, int originalValue)
        {
            return valueChange.ChangeType switch
            {
                ChangeType.Value => valueChange.Value,
                ChangeType.Percentage => 1f / valueChange.Value * originalValue,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void ApplyRewards()
        {
            foreach (Quest completedQuest in _activeQuests.PassDay())
            {
                completedQuest.Reward.Modify(_gameState);
            }
        }

        private void ApplyPenalties()
        {
            foreach (Quest declinedQuest in _offeredQuests.DeclinedQuests)
            {
                declinedQuest.Penalty.Modify(_gameState);
            }
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
    }
}
using System;
using Domain.Quests;
using UnityEngine;

namespace Domain
{
    public class PlayCoordinator
    {
        private readonly ActiveQuests _activeQuests = new();
        private readonly OfferedQuests _offeredQuests = new();
        private readonly GameState _gameState = new();

        public Action OnOutOfLives;

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
        }
        
        private void PayForAcceptedQuests()
        {
            foreach (Quest acceptedQuest in _offeredQuests.AcceptedQuests)
            {
                _gameState.ArmySize = Mathf.Max(0, _gameState.ArmySize - acceptedQuest.Requirement);

                _activeQuests.Add(acceptedQuest);
            }
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
    }
}
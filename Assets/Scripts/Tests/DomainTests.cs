using System.Collections.Generic;
using System.Reflection;
using Domain;
using Domain.Incentives;
using Domain.Modifiers;
using Domain.Quests;
using NUnit.Framework;

namespace Tests
{
    public class DomainTests
    {
        [Test]
        public void PlayCoordinateOneDayTest()
        {
            PlayCoordinator playCoordinator = new();
            Quest oneDayQuest = new Quest
            {
                DaysToComplete = 1,
                Requirement = 1,
                Region = Region.Orchid,
                Reward = new () { new CurrencyIncentive(2, Currency.Lives) },
                Penalty = new () { new CurrencyIncentive(0, Currency.Lives) }
            };

            playCoordinator.SetQuestsForTheDay(new List<Quest> { oneDayQuest });
            playCoordinator.Accept(oneDayQuest);
            playCoordinator.PassDay();

            GameState gameState = GetGameState(playCoordinator);

            Assert.That(gameState.Lives, Is.EqualTo(5));
        }

        [Test]
        public void ArmyCountChangesAcrossQuestDays()
        {
            PlayCoordinator playCoordinator = new();
            GameState gameState = GetGameState(playCoordinator);

            Quest twoDayQuest = new Quest
            {
                DaysToComplete = 2,
                Requirement = 100,
                Region = Region.Orchid,
                Reward = new() { new CurrencyIncentive(200, Currency.ArmySize) },
                Penalty = new () { new CurrencyIncentive(0, Currency.Lives) }
            };

            Assert.That(gameState.ArmySize, Is.EqualTo(1000));

            playCoordinator.SetQuestsForTheDay(new List<Quest> { twoDayQuest });
            playCoordinator.Accept(twoDayQuest);
            playCoordinator.PassDay();

            Assert.That(gameState.ArmySize, Is.EqualTo(900));

            playCoordinator.SetQuestsForTheDay(new List<Quest>());
            playCoordinator.PassDay();

            Assert.That(gameState.ArmySize, Is.EqualTo(1200));
        }

        [Test]
        public void AcceptingTwoQuestsAppliesBothRewards()
        {
            PlayCoordinator playCoordinator = new();
            GameState gameState = GetGameState(playCoordinator);

            Quest lifeQuest = new Quest
            {
                DaysToComplete = 1,
                Requirement = 1,
                Region = Region.Orchid,
                Reward = new () { new CurrencyIncentive(1, Currency.Lives) },
                Penalty = new() { new CurrencyIncentive(0, Currency.Lives) }
            };

            Quest armyQuest = new Quest
            {
                DaysToComplete = 1,
                Requirement = 1,
                Region = Region.Ashmound,
                Reward = new () { new CurrencyIncentive(200, Currency.ArmySize) },
                Penalty = new () { new CurrencyIncentive(0, Currency.Lives) }
            };

            playCoordinator.SetQuestsForTheDay(new List<Quest> { lifeQuest, armyQuest });
            playCoordinator.Accept(lifeQuest);
            playCoordinator.Accept(armyQuest);
            playCoordinator.PassDay();

            Assert.That(gameState.Lives, Is.EqualTo(4));
            Assert.That(gameState.ArmySize, Is.EqualTo(1200));
        }

        [Test]
        public void DecliningQuestAppliesPenaltyAfterPassingDay()
        {
            PlayCoordinator playCoordinator = new();
            GameState gameState = GetGameState(playCoordinator);

            Quest declinedQuest = new Quest
            {
                DaysToComplete = 1,
                Requirement = 1,
                Region = Region.Orchid,
                Reward = new() { new CurrencyIncentive(0, Currency.Lives) },
                Penalty = new() { new CurrencyIncentive(-2, Currency.Lives) }
            };

            playCoordinator.SetQuestsForTheDay(new List<Quest> { declinedQuest });
            playCoordinator.Decline(declinedQuest);
            playCoordinator.PassDay();

            Assert.That(gameState.Lives, Is.EqualTo(1));
        }

        [Test]
        public void ModifierIncentiveRewardIsAddedToGameStateAfterOneDay()
        {
            PlayCoordinator playCoordinator = new();
            GameState gameState = GetGameState(playCoordinator);
            IncentiveModifier rewardModifier = new IncentiveModifier
            {
                ToAffect = new List<Region> { Region.Orchid },
                Changes = new List<ValueChange>
                {
                    new ValueChange
                    {
                        Attribute = Attribute.Lives,
                        ChangeType = ChangeType.Value,
                        TimeOfEffect = TimeOfEffect.OnReward,
                        Value = -1
                    }
                },
                Duration = new InfiniteDuration()
            };

            Quest modifierRewardQuest = new Quest
            {
                DaysToComplete = 1,
                Requirement = 1,
                Region = Region.Orchid,
                Reward = new () { new ModifierIncentive(rewardModifier) },
                Penalty = new() { new CurrencyIncentive(0, Currency.Lives) }
            };

            Assert.That(gameState.Modifiers.Count, Is.EqualTo(0));
            Assert.That(gameState.Lives, Is.EqualTo(3));

            playCoordinator.SetQuestsForTheDay(new List<Quest> { modifierRewardQuest });
            playCoordinator.Accept(modifierRewardQuest);
            playCoordinator.PassDay();

            Assert.That(gameState.Modifiers.Count, Is.EqualTo(1));
            Assert.That(gameState.Modifiers.Contains(rewardModifier), Is.True);
            
            playCoordinator.SetQuestsForTheDay(new List<Quest> { modifierRewardQuest });
            playCoordinator.Accept(modifierRewardQuest);
            playCoordinator.PassDay();
            
            Assert.That(gameState.Lives, Is.EqualTo(3));
        }

        private static GameState GetGameState(PlayCoordinator playCoordinator)
        {
            FieldInfo gameStateField = typeof(PlayCoordinator).GetField("_gameState",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.That(gameStateField, Is.Not.Null);

            return (GameState)gameStateField.GetValue(playCoordinator);
        }
    }
}
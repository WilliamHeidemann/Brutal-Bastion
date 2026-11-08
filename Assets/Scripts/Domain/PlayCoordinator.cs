using Domain.Quests;

namespace Domain
{
    public class PlayCoordinator
    {
        public readonly ActiveQuests activeQuests = new();
        public readonly OfferedQuests offeredQuests = new();
        public readonly GameState gameState = new();

        public void Accept(Quest quest)
        {
            offeredQuests.Accept(quest);
        }

        public void Decline(Quest quest)
        {
            offeredQuests.Decline(quest);
        }

        public void PassDay()
        {
            foreach (Quest declinedQuest in offeredQuests.DeclinedQuests)
            {
                declinedQuest.Penalty.Modify(gameState);
            }

            foreach (Quest completedQuest in activeQuests.PassDay())
            {
                completedQuest.Reward.Modify(gameState);
            }

            foreach (Quest acceptedQuest in offeredQuests.AcceptedQuests)
            {
                activeQuests.Add(acceptedQuest);
            }
        }
    }
}
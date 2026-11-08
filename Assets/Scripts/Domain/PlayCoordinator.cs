using System.Collections.Generic;

namespace Domain
{
    public class PlayCoordinator
    {
        public readonly QuestTable questTable = new();
        public readonly Army Army = new();

        public void Accept(Quest quest)
        {
            questTable.Add(quest);
        }

        public void Discard(Quest quest)
        {
            Army.Size -= quest.SizePenalty;
            Army.Health -= quest.HealthPenalty;
        }

        public void PassDay()
        {
            foreach (Quest completedQuest in questTable.PassDay())
            {
                Army.Size += completedQuest.Requirement + completedQuest.Reward;
            }
        }
    }
}
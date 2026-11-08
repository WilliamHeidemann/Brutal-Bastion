using System.Collections.Generic;

namespace Domain.Quests
{
    public class ActiveQuests
    {
        private readonly Dictionary<Quest, int> _activeQuests = new();

        public void Add(Quest quest)
        {
            _activeQuests.Add(quest, quest.DaysToComplete);
        }
        
        /// <returns>completed quests.</returns>
        public List<Quest> PassDay()
        {
            List<Quest> completedQuests = new();
            
            foreach (Quest quest in _activeQuests.Keys)
            {
                _activeQuests[quest] -= 1;
                var daysLeft = _activeQuests[quest];
                if (daysLeft <= 0)
                {
                    completedQuests.Add(quest);
                }
            }
            
            foreach (Quest completedQuest in completedQuests)
            {
                _activeQuests.Remove(completedQuest);
            }
            
            return completedQuests;
        }
    }
}
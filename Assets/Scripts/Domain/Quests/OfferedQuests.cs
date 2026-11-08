using System.Collections.Generic;
using System.Linq;

namespace Domain.Quests
{
    public class OfferedQuests
    {
        private enum Status
        {
            Accepted,
            Declined
        }
        
        private readonly Dictionary<Quest, Status> _quests = new();

        public void Clear() => _quests.Clear();
        
        public void Add(Quest quest)
        {
            _quests.Add(quest, Status.Declined);
        }
        
        public void Accept(Quest quest)
        {
            _quests[quest] = Status.Accepted;
        }

        public void Decline(Quest quest)
        {
            _quests[quest] = Status.Declined;
        }

        public IEnumerable<Quest> AcceptedQuests => _quests.Keys.Where(q => _quests[q] == Status.Accepted);
        public IEnumerable<Quest> DeclinedQuests => _quests.Keys.Where(q => _quests[q] == Status.Declined);
    }
}
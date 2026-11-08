using Domain.Incentives;

namespace Domain.Quests
{
    public class Quest
    {
        public int DaysToComplete;
        public int Requirement;
        public Incentive Reward;
        public Incentive Penalty;
        public Region Region;
    }
}
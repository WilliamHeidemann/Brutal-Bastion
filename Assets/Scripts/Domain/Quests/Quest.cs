using Domain.Incentives;

namespace Domain.Quests
{
    public class Quest
    {
        public int DaysToComplete;
        public int Requirement;
        public CurrencyIncentive Reward;
        public CurrencyIncentive Penalty;
        public Region Region;
    }
}
namespace Domain
{
    public record Quest
    {
        public int DaysToComplete;
        public int Requirement;
        public int Reward;
        public int SizePenalty;
        public int HealthPenalty;
    }
}
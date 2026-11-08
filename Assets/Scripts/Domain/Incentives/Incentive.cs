namespace Domain.Incentives
{
    public abstract class Incentive
    {
        public abstract void Modify(GameState gameState);
        public abstract string GetDescription();
    }
}
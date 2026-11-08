namespace Domain.Incentives
{
    public class ArmyIncentive : Incentive
    {
        private readonly int _amount;

        public ArmyIncentive(int amount)
        {
            _amount = amount;
        }

        public override void Modify(GameState gameState)
        {
            gameState.ArmySize += _amount;
        }

        public override string GetDescription()
        {
            return $"Increase army size by {_amount}.";
        }
    }
}
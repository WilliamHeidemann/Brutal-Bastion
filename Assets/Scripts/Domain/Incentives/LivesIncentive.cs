namespace Domain.Incentives
{
    public class LivesIncentive : Incentive
    {
        private readonly int _amount;

        public LivesIncentive(int amount)
        {
            _amount = amount;
        }

        public override void Modify(GameState gameState)
        {
            gameState.Lives += _amount;
        }

        public override string GetDescription()
        {
            return _amount > 0 ? 
                $"Gain {_amount} life." :
                $"Lose {_amount} life.";
        }
    }
}
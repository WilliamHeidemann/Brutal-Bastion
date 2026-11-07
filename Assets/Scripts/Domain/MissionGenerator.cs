using UnityEngine;

namespace Domain
{
    public class MissionGenerator
    {
        public Mission Generate()
        {
            return new Mission
            {
                DaysToComplete = Random.Range(1, 4),
                Requirement = Random.Range(1, 40) * 10,
                Reward = Random.Range(1, 20) * 10,
                SizePenalty = Random.Range(-20, 1) * 10,
                HealthPenalty = Random.value < .1f ? 1 : 0
            };
        }
    }
}
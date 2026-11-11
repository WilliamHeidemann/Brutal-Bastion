using Domain.Incentives;
using UnityEngine;

namespace Domain.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/Quest")]
    public class Quest : ScriptableObject
    {
        public string Title;
        public string Description;
        public int DaysToComplete;
        public int Requirement;
        public Incentive Reward;
        public Incentive Penalty;
        public Region Region;
    }
}
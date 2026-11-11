using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Incentives;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Domain.Quests
{
    public class Quest
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
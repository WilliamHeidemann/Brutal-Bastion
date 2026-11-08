using System.Collections.Generic;
using System.Linq;
using Domain.Modifiers;

namespace Domain
{
    public class GameState
    {
        public int ArmySize = 1000;
        public int Lives = 3;
        public readonly HashSet<IncentiveModifier> Modifiers = new();
    }
}
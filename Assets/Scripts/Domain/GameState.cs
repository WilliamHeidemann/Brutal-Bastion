using System.Collections.Generic;
using System.Linq;
using Domain.Modifiers;

namespace Domain
{
    public class GameState
    {
        public int ArmySize;
        public int Lives;
        public readonly HashSet<IncentiveModifier> Modifiers = new();
    }
}
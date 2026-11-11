using System.Collections.Generic;
using System.Linq;
using Domain.Modifiers;

namespace Domain
{
    public class GameState
    {
        public int Day = 1;
        public IEnumerable<string> flags = new List<string>();
        public int ArmySize = 1000;
        public int Lives = 3;
        public readonly HashSet<IncentiveModifier> Modifiers = new();
    }
}
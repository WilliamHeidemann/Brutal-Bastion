using System.Collections.Generic;

namespace Domain
{
    public class GameState
    {
        public int ArmySize;
        public int Lives;
        public readonly List<Modifier> Modifiers = new();
    }
}
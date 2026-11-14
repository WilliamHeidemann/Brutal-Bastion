using Domain.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Utility;
using Attribute = Domain.Modifiers.Attribute;

namespace Domain.Incentives
{
    public class ModifierIncentive : Incentive
    {
        private IncentiveModifier Modifier { get; }

        public ModifierIncentive(IncentiveModifier modifier)
        {
            Modifier = modifier;
        }

        public override void Modify(GameState gameState) => gameState.Modifiers.Add(Modifier);

        public override string GetDescription() => Descriptions.Get(Modifier);
    }
}
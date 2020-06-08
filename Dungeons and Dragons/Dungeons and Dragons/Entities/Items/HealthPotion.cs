using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeons_and_Dragons.Entities.Items
{
    public class HealthPotion : Item
    {
        private const int HitPointsRestored = 20;

        public HealthPotion() : base(5)
        {
        }

        public override void AffectCharacter(Character character)
        {
            base.AffectCharacter(character);
            character.Health += HitPointsRestored;
        }
    }
}

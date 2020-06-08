using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeons_and_Dragons.Entities.Items
{
    public class PoisonPotion : Item
    {
        private const int HitPointsDamaged = 20;
        public PoisonPotion() : base(5)
        {
        }

        public override void AffectCharacter(Character character)
        {
            base.AffectCharacter(character);

            if (!character.IsAlive)
            {
                return;
            }

            character.Health = Math.Max(0, character.Health - HitPointsDamaged);

            if (character.Health == 0)
            {
                character.IsAlive = false;
            }

        }
    }
}

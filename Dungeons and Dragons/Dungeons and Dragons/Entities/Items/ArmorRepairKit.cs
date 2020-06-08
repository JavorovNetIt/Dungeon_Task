using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeons_and_Dragons.Entities.Items
{
    public class ArmorRepairKit : Item
    {
        public ArmorRepairKit() : base(10)
        {
        }

        public override void AffectCharacter(Character character)
        {
            base.AffectCharacter(character);

            if (!character.IsAlive)
            {
                return;
            }

            character.Armor = character.BaseArmor;
        }
    }
}

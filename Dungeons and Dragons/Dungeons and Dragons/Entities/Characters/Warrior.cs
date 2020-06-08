using Dungeons_and_Dragons.Entities.Characters.Contracts;
using Dungeons_and_Dragons.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeons_and_Dragons.Entities.Characters
{
    public class Warrior : Character, IAttackable
    {
        public Warrior(string name, Faction faction) : base(name, health: 100, armor: 50, abilityPoints: 40, bag: new Satchel(), faction)
        {
        }

        public void Attack(Character character)
        {
            this.EnsureAlive();

            if (character == this)
            {
                throw new InvalidOperationException("Cannot attack self!");
            }

            EnsureNoFriendlyFire(character);

            character.TakeDamege(this.AbilityPoints);
        }

        private void EnsureNoFriendlyFire(Character character)
        {
            if (this.Faction == character.Faction)
            {
                throw new ArgumentException($"Friendly fire! Both characters are from {this.Faction} faction!");
            }
        }
    }
}

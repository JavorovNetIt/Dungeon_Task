using Dungeons_and_Dragons.Entities.Inventory;
using Dungeons_and_Dragons.Entities.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeons_and_Dragons.Entities.Characters
{
    public abstract class Character
    {
		private string name;
		private double baseHealth;
		private double health;
		private double baseArmor;
		private double armor;
		private double abilityPoints;


		protected Character(string name, double health, double armor, double abilityPoints, Bag bag, Faction faction)
		{
			this.Name = name;
			this.BaseHealth = health;
			this.Health = health;

			this.BaseArmor = armor;
			this.Armor = armor;

			this.AbilityPoints = abilityPoints;
			this.Bag = bag;
			this.Faction = faction;
		}

		public double AbilityPoints
		{
			get { return abilityPoints; }
			set { abilityPoints = value; }
		}

		public double Armor
		{
			get { return armor; }
			set { armor = value; }
		}


		public double BaseArmor
		{
			get { return baseArmor; }
			set { baseArmor = value; }
		}


		public double Health
		{
			get { return health; }
			set { health = value; }
		}

		public double BaseHealth
		{
			get { return baseHealth; }
			set { baseHealth = value; }
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public Faction Faction { get; set; }

		public bool IsAlive { get; set; } = true;

		public Bag Bag { get; }

		protected virtual double RestHealMultiplier => (double)1 / 5;

		public void TakeDamege(double hitPoints)
		{
			EnsureAlive();

			var hitpointsLeftAfterArmorDamage = Math.Max(0, hitPoints - Armor);

			this.Armor = Math.Max(0, Armor - hitPoints);
			this.Health = Math.Max(0, Health - hitpointsLeftAfterArmorDamage);

			if (Health ==0)
			{
				IsAlive = false;
			}
		}

		public void Rest()
		{
			EnsureAlive();

			this.Health = Math.Min(Health + BaseHealth * RestHealMultiplier, BaseHealth);
		}

		public void UseItem(Item item)
		{
			UseItemOn(item, this);
		}

		public void UseItemOn(Item item, Character character)
		{
			item.AffectCharacter(character);
		}

		public void GiveCharacterItem(Item item, Character character)
		{
			character.ReceiveItem(item);
		}

		public void ReceiveItem(Item item)
		{
			this.Bag.AddItem(item);
		}

		protected void EnsureAlive()
		{
			if (!IsAlive)
			{
				throw new InvalidOperationException("Must be alive to perform this action!");
			}
		}

		public override string ToString()
		{
			const string format = "{0} - HP: {1}/{2}, AP: {3}/{4}, Status: {5}";

			var result = string.Format(format,
				Name,
				Health, 
				BaseHealth,
				Armor,
				BaseArmor,
				IsAlive ? "Alive" : "Dead");

			return result;
		}
	}
}

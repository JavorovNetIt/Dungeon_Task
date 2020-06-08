using Dungeons_and_Dragons.Entities.Characters;
using Dungeons_and_Dragons.Entities.Characters.Contracts;
using Dungeons_and_Dragons.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeons_and_Dragons.Core
{
    public  class DungeonMaster
    {
        private readonly List<Character> party;
        private readonly Stack<Item> itemPool;

        private int lastSurvivorRounds;
        public DungeonMaster()
        {
            party = new List<Character>();
            itemPool = new Stack<Item>();
        }

        public string JoinParty(string[] args)
        {
            var faction = args[0];
            var characterClass = args[1];
            var name = args[2];

            Faction parsedFaction;

            if (!Enum.TryParse(faction, out parsedFaction))
            {
                throw new ArgumentException($"Invalid faction \"{faction}\"");
            }

            Character character;

            switch (characterClass)
            {
                case "Warrior":
                    character = new Warrior(name, parsedFaction);
                    break;
                case "Clerik":
                    character = new Cleric(name, parsedFaction);
                    break;
                default:
                    throw new ArgumentException($"Invalid character type \"{characterClass}\"");
            }

            return $"{character.Name} joined the party!";
        }

        public string AddItemToPool(string[] args)
        {
            var itemName = args[0];
            Item item;

            switch (itemName)
            {
                case "HealthPotion":
                    item = new HealthPotion();
                    break;
                case "PoisonPotion":
                    item = new PoisonPotion();
                    break;
                case "ArmorRepairKit":
                    item = new ArmorRepairKit();
                    break;
                default:
                    throw new ArgumentException($"Invalid item \"{itemName}\"");
            }

            itemPool.Push(item);

            return $"{itemName} added to pool!";
        }

        public string PickUpItem(string[] args)
        {
            var characterName = args[0];

            var character = FindCharacter(characterName);

            var anyItemsLeft = itemPool.Any();

            if (!anyItemsLeft)
            {
                throw new InvalidOperationException("No items left in pool");
            }

            var item = itemPool.Pop();

            character.ReceiveItem(item);

            return $"{character.Name} picked up {item.GetType().Name}";
        }

        private Character FindCharacter(string characterName)
        {
            var character = party.FirstOrDefault(c => c.Name == characterName);

            if (character == null)
            {
                throw new ArgumentException($"Character {characterName} not found");
            }

            return character;
        }

        public string UseItem(string[] args)
        {
            var characterName = args[0];
            var itemName = args[1];

            var character = FindCharacter(characterName);

            var item = character.Bag.GetItem(itemName);

            character.UseItem(item);

            return $"{character.Name} used {itemName}!";
        }

        public string UseItemOn(string[] args)
        {
            var giverName = args[0];
            var receiverName = args[1];
            var itemName = args[2];

            var giver = FindCharacter(giverName);
            var receiver = FindCharacter(receiverName);

            var item = giver.Bag.GetItem(itemName);

            giver.UseItemOn(item, receiver);

            return $"{giver.Name} used {itemName} on {receiver.Name}";
        }

        public string GiveCharacterItem(string[] args)
        {
            var giverName = args[0];
            var receiverName = args[1];
            var itemName = args[2];

            var giver = FindCharacter(giverName);
            var receiver = FindCharacter(receiverName);

            var item = giver.Bag.GetItem(itemName);

            giver.GiveCharacterItem(item, receiver);

            return $"{giver.Name} gave {receiver.Name} {itemName}";
        }

        public string GetStats()
        {
            var sortedCharacters = party
                 .OrderByDescending(c => c.IsAlive)
                 .ThenByDescending(c => c.Health);

            var result = string.Join(Environment.NewLine, sortedCharacters);

            return result;
        }

        public string Attack(string[] args)
        {
            var attackerName = args[0];
            var receiverName = args[1];

            var attacker = FindCharacter(attackerName);
            var receiver = FindCharacter(receiverName);

            if (!(attacker is IAttackable attackingCharackter))
            {
                throw new ArgumentException($"{attacker.Name} cannot attack");
            }

            attackingCharackter.Attack(receiver);

            var result = $"{attacker.Name} attacks {receiver.Name} for {attacker.AbilityPoints} hit points! {receiver.Name} hase {receiver.Health}/{receiver.BaseHealth} HP and {receiver.Armor}/{receiver.BaseArmor} AP left";

            if (!receiver.IsAlive)
            {
                result += Environment.NewLine + $"{receiverName} is dead!";
            }

            return result;
        }

        public string Heal(string[] args)
        {
            var healerName = args[0];
            var receiverName = args[1];

            var healer = FindCharacter(healerName);
            var receiver = FindCharacter(receiverName);

            if (!(healer is IHealable healingCharacter))
            {
                throw new ArgumentException($"{healer.Name} cannot heal!");
            }

            healingCharacter.Heal(receiver);

            var result = $"{healer.Name} heals {receiver.Name} for {healer.AbilityPoints}! {receiver.Name} has {receiver.Health} health now!";

            return result;
        }

        public string EndTurn(string[] args)
        {
            var aliveCharacters = party.Where(c => c.IsAlive).ToArray();

            var sb = new StringBuilder();

            foreach (var character in aliveCharacters)
            {
                var previousHealth = character.Health;

                character.Rest();

                var currentHealth = character.Health;
                sb.AppendLine($"{character.Name} rests ({previousHealth} => {currentHealth})");
            }

            if (aliveCharacters.Length <= 1)
            {
                lastSurvivorRounds++;
            }

            var result = sb.ToString().TrimEnd('\r', '\n');

            return result;
        }

        public bool IsGameOver()
        {
            var oneOrZeroSurvivorsLeft = party.Count(c => c.IsAlive) <= 1;

            var lastSurviverSurvivedTooLong = this.lastSurvivorRounds > 1;

            return oneOrZeroSurvivorsLeft && lastSurviverSurvivedTooLong;
        }

    }
}

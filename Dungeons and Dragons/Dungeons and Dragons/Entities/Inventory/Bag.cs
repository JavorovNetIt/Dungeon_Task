using Dungeons_and_Dragons.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeons_and_Dragons.Entities.Inventory
{
    public abstract class Bag
    {
        private const int DefaultCapacity = 100;

        private readonly List<Item> items;

        private int capacity;

        protected Bag(int capacity = DefaultCapacity)
        {
            this.Capacity = capacity;
            this.items = new List<Item>();
        }

        public int Capacity
        {
            get { return this.capacity; }
            set
            {
                this.capacity = value;
            }
        }

        public int Load { get => this.items.Sum(i => i.Weight); }

        public IReadOnlyCollection<Item> Items { get => this.items.AsReadOnly(); }

        public void AddItem(Item item)
        {
            if (this.Load + item.Weight > Capacity)
            {
                throw new InvalidOperationException("Bag is full");
            }

            this.items.Add(item);
        }

        public Item GetItem(string name)
        {
            EnsureItemExists(name);

            var item = this.items.First(i => i.GetType().Name == name);

            this.items.Remove(item);

            return item;
        }

        private void EnsureItemExists(string name)
        {
            if (!this.items.Any())
            {
                throw new InvalidOperationException("Bag is empty");
            }

            var itemExists = this.Items.Any(i => i.GetType().Name == name);

            if (!itemExists)
            {
                throw new ArgumentException($"No item with name {name} in bag!");
            }
        }
    }
}

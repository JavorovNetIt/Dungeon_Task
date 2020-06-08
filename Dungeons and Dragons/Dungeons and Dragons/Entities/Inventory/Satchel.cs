using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeons_and_Dragons.Entities.Inventory
{
    public class Satchel : Bag
    {
        private const int DefaultCapacity = 20;

        public Satchel() : base(DefaultCapacity)
        {

        }
    }
}

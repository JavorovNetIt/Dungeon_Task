using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeons_and_Dragons.Entities.Characters.Contracts
{
    public interface IAttackable
    {
        void Attack(Character character);

    }
}

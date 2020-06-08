using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeons_and_Dragons.Entities.Characters.Contracts
{
    public interface IHealable
    {
        void Heal(Character character);
    }
}

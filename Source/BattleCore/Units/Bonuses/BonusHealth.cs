using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vortex.Drawing;
using BattleCore.Units;
using System.Drawing;
using BattleCore.Units.Bonuses;

namespace BattleCore.Bonuses
{
    public class BonusHealth : BonusBase
    {
        public BonusHealth(World world, Point point)
            : base(world, point)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleCore.Units;
using Vortex.Drawing;
using System.Drawing;
using BattleCore.Units.Bonuses;

namespace BattleCore.Bonuses
{
    public class BonusLife : BonusBase
    {
        public BonusLife(World world, Point point)
            : base(world, point)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vortex.Drawing;
using System.Drawing;

namespace BattleCore.Units.Bonuses
{
    public abstract class BonusBase : UnitBase
    {
        public BonusBase(World world, Point point)
            : base(world, WorldLaw.DefaultUnitCreationSpeed,
            WorldLaw.DefaultEnemyLifeCount)
        {
            SetLocation(point.X, point.Y);
        }


        public override void OnDamage(BulletProcessor damager)
        {
        }

        protected override bool CanGoTo(Vector location, out UnitBase collision)
        {
            collision = null;
            return false;
        }
    }
}

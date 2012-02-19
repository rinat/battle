using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vortex.Drawing;
using System.Drawing;

namespace BattleCore.Units
{
    public class WallUnit : UnitBase
    {
        public enum WallType
        {
            MetalWall,
            TreeWall,
        }

        public WallType WallUnitType { get; set; }

        public WallUnit(World world, WallType wallType)
            : base(world, WorldLaw.DefaultUnitCreationSpeed,
            WorldLaw.DefaultEnemyLifeCount)
        {
            WallUnitType = wallType;
        }

        public WallUnit(World world, Point point, WallType wallType)
            : base(world, WorldLaw.DefaultUnitCreationSpeed,
            WorldLaw.DefaultEnemyLifeCount)
        {
            WallUnitType = wallType;
            SetLocation(point.X, point.Y);
        }

        public WallUnit(World world, int x, int y, WallType wallType)
            : base(world, WorldLaw.DefaultUnitCreationSpeed,
            WorldLaw.DefaultEnemyLifeCount)
        {
            WallUnitType = wallType;
            SetLocation(x, y);
        }

        public override void OnDamage(BulletProcessor damager)
        { }

        protected override bool CanGoTo(Vector location, out UnitBase collision)
        {
            collision = null;
            return false;
        }
    }
}

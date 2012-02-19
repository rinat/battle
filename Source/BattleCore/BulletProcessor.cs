using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleCore.Units;

namespace BattleCore
{
    public class BulletProcessor
    {
        public void DoDamage(PlayerUnit target)
        {
            if (target.State == PlayerUnit.PlayerState.Dead)
            {
                return;
            }
            target.Life.Count = target.Life.Count - 1;
        }

        public void DoDamage(EnemyUnit target)
        {
            target.Life.Count = target.Life.Count - 1;
        }

        public void DoDamage(UnitBase target)
        { }
    }
}

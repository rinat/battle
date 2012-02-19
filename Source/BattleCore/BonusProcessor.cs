using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleCore.Units.Bonuses;
using BattleCore.Bonuses;

namespace BattleCore.Units
{
    // todo: need refactoring here
    public class BonusProcessor
    {
        public void AcceptBonus(PlayerUnit player, BonusBase bonus)
        {
            var bonus2 = bonus as BonusBoom;
            if (bonus2 != null)
            {
                foreach (var enemy in player.World.Enemies)
                {
                    enemy.Life.Count = 0;
                }
                if (player.Score.Count / WorldLaw.LevelUpThreshold < 
                    (player.Score.Count + player.World.Enemies.Count) / WorldLaw.LevelUpThreshold)
                {
                    player.World.WorldState.LevelUp = true;
                }
                player.Score.Count += player.World.Enemies.Count;
                player.World.WorldState.BonusExplosion = true;
                return;
            }
            var bonus3 = bonus as BonusHealth;
            if (bonus3 != null)
            {
                player.Life.Count = player.Life.Count + 1;
                return;
            }
            var bonus4 = bonus as BonusLife;
            if (bonus4 != null)
            {
                player.Life.Count = player.Life.Count +
                    WorldLaw.DefaultPlayerLifeCount;
                return;
            }
        }
    }
}

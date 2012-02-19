using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCore
{
    public class WorldState
    {
        public WorldState(World world)
        {
            World = world;
            LevelNumber = 1;
        }

        public World World { get; private set; }

        public bool LevelUp
        {
            get
            {
                return m_levelUp;
            }
            set
            {
                if (!m_levelUp && value)
                {
                    LevelNumber += 1;
                }
                m_levelUp = value;
            }
        }
        
        public bool GameComplete { get; internal set; }

        public bool BonusExplosion { get; internal set; }

        public int LevelNumber { get; internal set; }

        public void Reset(bool resetLevelInfo)
        {
            LevelUp = false;
            GameComplete = false;
            BonusExplosion = false;
            if (resetLevelInfo)
            {
                LevelNumber = 1;
            }
        }

        private bool m_levelUp = false;
    }
}

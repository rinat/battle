using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vortex.Core;
using BattleCore;
using System.Drawing;
using BattleCore.Units.Bonuses;
using System.Diagnostics;
using BattleCore.Bonuses;
using Vortex.Drawing;
using BattleUI.VisualUnits;

namespace BattleUI.Life
{
    class BonusesPopulationController : IUpdatable
    {
        const float BonusGenerationPeriod = 6.0f;

        public BonusesPopulationController(int maxBonusesCount, World world,
            IEnumerable<Point> bonusesGenerationPoints)
        {
            World = world;
            MaxBonusesCount = maxBonusesCount;
            BonusesGenerationPoints = new List<Point>(bonusesGenerationPoints);
        }

        public int MaxBonusesCount { get; set; }

        public World World { get; private set; }

        public List<Point> BonusesGenerationPoints { get; private set; }

        #region IUpdatable Members

        public void Update(float timeDelta)
        {
            if (World.Bonuses.Count < MaxBonusesCount)
            {
                // todo need refactoring here
                m_timeDelta += timeDelta;
                if (m_timeDelta < BonusGenerationPeriod)
                {
                    return;
                }
                m_timeDelta = 0.0f;

                int randPosition = m_random.Next(0, (BonusesGenerationPoints.Count));
                var selectedPoint = BonusesGenerationPoints[randPosition];
                if (World.Bonuses.Count == 0)
                {
                    AddBonus(selectedPoint);
                }
                else if (EmptyBonusesIn(selectedPoint))
                {
                    AddBonus(selectedPoint);
                }
            }
        }

        #endregion

        private bool EmptyBonusesIn(Point point)
        {
            foreach (var bonus in World.Bonuses)
            {
                if (bonus.Location.ToPoint() == point)
                {
                    return false;
                }
            }
            return true;
        }

        private void AddBonus(Point point)
        {
            BonusBase bonus = null;
            SpriteAnimation bonusAnimation = null;
            int nextBonusType = m_random.Next(1, 4);
            switch (nextBonusType)
            {
                case 1:
                    bonus = new BonusBoom(World, point);
                    bonusAnimation = Gfx.Instance.GetBonusBoomAnimation();
                    break;
                case 2:
                    bonus = new BonusHealth(World, point);
                    bonusAnimation = Gfx.Instance.GetBonusHealthAnimation();
                    break;
                case 3:
                    bonus = new BonusLife(World, point);
                    bonusAnimation = Gfx.Instance.GetBonusLifeAnimation();
                    break;
                default:
                    throw new InvalidOperationException("nextBonusType");
            }
            bonus.UserData = new VisualBonus(bonus, bonusAnimation);
            World.Bonuses.Add(bonus);
        }

        private float m_timeDelta = 0.0f;
        private Random m_random = new Random();
    }
}

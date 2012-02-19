using System;
using System.Collections.Generic;
using System.Drawing;
using BattleCore;
using BattleCore.Units;
using BattleUI.VisualUnits;
using Vortex.Core;
using System.Timers;
using Vortex.Drawing;
using Vortex;

namespace BattleUI.Life
{
    class EnemiesPopulationController : IUpdatable
    {
        const float EnemyGenerationPeriod = 2.0f;

        public EnemiesPopulationController(int maxEnemiesCount, World world,
            IEnumerable<Point> enemiesGenerationPoints)
        {
            World = world;
            MaxEnemiesCount = maxEnemiesCount;
            EnemiesGenerationPoints = new List<Point>(enemiesGenerationPoints);
        }

        public int MaxEnemiesCount { get; set; }

        public World World { get; private set; }

        public List<Point> EnemiesGenerationPoints { get; private set; }

        #region IUpdatable Members

        public void Update(float timeDelta)
        {
            if (EnemiesGenerationPoints.Count == 0)
            {
                return;
            }

            if (World.Enemies.Count < MaxEnemiesCount)
            {
                // todo need refactoring here
                m_timeDelta += timeDelta;
                if (m_timeDelta < EnemyGenerationPeriod)
                {
                    return;
                }
                m_timeDelta = 0.0f;


                int randPosition = m_random.Next(0, (EnemiesGenerationPoints.Count));
                var selectedPoint = EnemiesGenerationPoints[randPosition];
                var location = new Vector(selectedPoint.X, selectedPoint.Y);
                if (World.Enemies.Count == 0)
                {
                    AddEnemy(selectedPoint);
                }
                else
                {
                    bool ok = true;
                    foreach (var enemy in World.Enemies)
                    {
                        if (location.Distance(enemy.Location) < 1.0)
                        {
                            ok = false;
                        }
                    }
                    if (ok)
                    {
                        AddEnemy(selectedPoint);
                    }
                }
            }
        }

        #endregion IUpdatable Members

        private void AddEnemy(Point point)
        {
            var enemyUnit = new EnemyUnit(World, point, World.Player);
            enemyUnit.Life.Count = World.WorldState.LevelNumber;

            enemyUnit.Direction = UnitDirection.Left;
            enemyUnit.UserData = new VisualEnemyUnit(enemyUnit, Gfx.Instance.GetEnemyMovementAnimation());
            World.Enemies.Add(enemyUnit);
        }

        private float m_timeDelta = 0.0f;
        private Random m_random = new Random();
    }
}

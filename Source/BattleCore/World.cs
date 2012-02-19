using System.Collections.Generic;
using BattleCore.Units;
using Vortex;
using Vortex.Drawing;
using BattleCore.Units.Bonuses;

namespace BattleCore
{
    public class World
    {
        public enum MapType
        {
            Barrier = 100000000,
            Empty = 10000,
        }

        public World()
        {
            Reset();
        }

        public WorldState WorldState { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public PlayerUnit Player { get; set; }

        public List<WallUnit> Walls { get; private set; }

        public List<EnemyUnit> Enemies { get; private set; }

        public List<BulletUnit> Bullets { get; private set; }

        public List<BonusBase> Bonuses { get; private set; }

        public WorldLaw Law { get; private set; }

        public BonusProcessor BonusProcessor { get; private set; }

        public BulletProcessor BulletProcessor { get; private set; }

        public int[,] Map = null;

        public void UpdateMap()
        {
            Map = new int[Width, Height];
            
            for(int i = 0; i < Width; ++i)
            {
                for(int j = 0; j < Height; ++j)
                {
                    Map[i, j] = (int)MapType.Empty;
                }
            }

            foreach (WallUnit wall in Walls)
            {
                Map[(int)wall.Location.X, (int)wall.Location.Y] = (int)MapType.Barrier;
            }

            FindPath((int)Player.Location.X, (int)Player.Location.Y);
        }

        public void ClearPath()
        {
            for (int i = 0; i < Width; ++i)
            {
                for (int j = 0; j < Height; ++j)
                {
                    if(Map[i, j] != (int)MapType.Barrier)
                    {
                        Map[i, j] = (int)MapType.Empty;
                    }
                }
            }
        }

        public void FindPath(int x, int y)
        {
            ClearPath();

            path(x, y, 0);
        }

        private void path(int x, int y, int n)
        {
            if((x >= 0) && (x < Width) && (y >= 0) && (y < Height))
            {
                if(Map[x, y] != (int)MapType.Barrier)
                {
                    if((Map[x, y] == (int)MapType.Empty) ||
                       (Map[x, y] > n))
                    {
                        Map[x, y] = n;

                        path(x - 1, y, n + 1);
                        path(x + 1, y, n + 1);
                        path(x, y - 1, n + 1);
                        path(x, y + 1, n + 1);
                    }
                }
            }
        }

        public void Reset()
        {
            Width = 0;
            Height = 0;
            Law = new WorldLaw(this);
            Player = new PlayerUnit(this);
            WorldState = new WorldState(this);
            Walls = new List<WallUnit>();
            Enemies = new List<EnemyUnit>();
            Bullets = new List<BulletUnit>();
            Bonuses = new List<BonusBase>();
            BonusProcessor = new BonusProcessor();
            BulletProcessor = new BulletProcessor();
        }

        public void Update(float timeDelta)
        {
            Bullets.RemoveAll(item => item.BulletState == 
                BulletUnit.BulletUnitState.Dead);
            
            Enemies.RemoveAll(item => item.State == EnemyUnit.EnemyState.Dead);

            ResetStates();

            foreach (var wall in Walls)
            {
                wall.Update(timeDelta);
            }
            foreach (var bullet in Bullets)
            {
                bullet.Update(timeDelta);
            }
            foreach (var enemy in Enemies)
            {
                enemy.Update(timeDelta);
            }
            foreach (var bonus in Bonuses)
            {
                bonus.Update(timeDelta);
            }
            Player.Update(timeDelta);
        }

        private void ResetStates()
        {
            WorldState.Reset(false);
            if (Player.State == PlayerUnit.PlayerState.Dead)
            {
                WorldState.GameComplete = true;
            }
        }
    }
}

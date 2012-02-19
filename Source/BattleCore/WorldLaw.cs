using Vortex.Drawing;
using BattleCore.Units;

namespace BattleCore
{
    public class WorldLaw
    {
        public const float DefaultUnitCreationSpeed = 0.0f;
        public const float EnemyUnitCreationSpeed = 1.8f;
        public const float PlayerUnitCreationSpeed = 1.8f;
        public const float BulletUnitCreationSpeed = 7.0f;
        public const float UnitCollisionDistance = 1.0f;
        public const float EnemyUnitCollisionDistance = 1.0f;
        public const float BulletCollisionDistance = 0.5f;
        public const int DefaultPlayerLifeCount = 7;
        public const int DefaultEnemyLifeCount = 1;
        public const int LevelUpThreshold = 30;

        public WorldLaw(World world)
        {
            World = world;
        }

        public World World { get; private set; }

        public bool IsAccessible(PlayerUnit unit, Vector newPosition, out UnitBase collision)
        {
            return !IsWall(unit, newPosition, UnitCollisionDistance, out collision) &&
                !IsEnemy(unit, newPosition, UnitCollisionDistance, out collision);
        }

        public bool IsAccessible(EnemyUnit unit, Vector newPosition, out UnitBase collision)
        {
            return !IsWall(unit, newPosition, EnemyUnitCollisionDistance, out collision) &&
                !IsEnemy(unit, newPosition, EnemyUnitCollisionDistance, out collision) &&
                !IsPlayer(newPosition, EnemyUnitCollisionDistance, out collision);
        }

        public bool IsAccessible(BulletUnit unit, Vector newPosition, out UnitBase collision)
        {
            return !IsWall(unit, newPosition, BulletCollisionDistance, out collision) &&
                !IsEnemy(unit, newPosition, BulletCollisionDistance, out collision) &&
                !IsPlayer(newPosition, BulletCollisionDistance, out collision);
        }

        public bool IsPlayer(Vector newPosition, float collisionCoeff, out UnitBase collision)
        {
            collision = null;
            if (World.Player.State != PlayerUnit.PlayerState.Dead &&
                World.Player.Location.Distance(newPosition) < collisionCoeff)
            {
                collision = World.Player;
                return true;
            }
            return false;
        }

        public bool IsEnemy(UnitBase requestor, Vector newPosition, float collisionCoeff, out UnitBase collision)
        {
            collision = null;
            var enemies = World.Enemies;
            foreach (var enemy in enemies)
            {
                if (enemy.Location.Distance(newPosition) < collisionCoeff &&
                    enemy.GetHashCode() != requestor.GetHashCode())
                {
                    collision = enemy;
                    return true;
                }
            }
            return false;
        }

        
        public bool IsWall(UnitBase requestor, Vector newPosition, float collisionCoeff, out UnitBase collision)
        {
            collision = null;
            var walls = World.Walls;
            foreach (var wall in walls)
            {
                if (wall.Location.Distance(newPosition) < collisionCoeff &&
                    wall.GetHashCode() != requestor.GetHashCode())
                {
                    collision = wall;
                    return true;
                }
            }
            return false;
        }
    }
}

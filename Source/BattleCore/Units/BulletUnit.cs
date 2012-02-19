using System.Drawing;
using Vortex.Drawing;

namespace BattleCore.Units
{
    public class BulletUnit : UnitBase
    {
        public enum BulletUnitState
        {
            Running,
            Dead
        }

        public BulletUnit(World world, PointF point, UnitBase sender)
            : base(world, WorldLaw.BulletUnitCreationSpeed,
            WorldLaw.DefaultEnemyLifeCount)
        {
            SetLocation(point.X, point.Y);
            Sender = sender;
            BulletState = BulletUnitState.Running;
        }

        public BulletUnitState BulletState { get; private set; }

        public UnitBase Sender { get; private set; }

        public UnitBase DeadOn { get { return m_deadOn; } }

        #region Unit Members

        public override void Update(float timeDelta)
        {
            Vector newLocation = Location + Direction.ToVector() * Speed * timeDelta;
            if (CanGoTo(newLocation, out m_deadOn))
            {
                SetLocation(newLocation);
            }
            else
            {
                DeadOn.OnDamage(World.BulletProcessor);
                BulletState = BulletUnitState.Dead;

                TryAddScore(Sender, DeadOn);
            }
            base.Update(timeDelta);
        }

        public override void OnDamage(BulletProcessor damager)
        {
        }

        #endregion Unit Members

        protected override bool CanGoTo(Vector location, out UnitBase collision)
        {
            return World.Law.IsAccessible(this, location, out collision);
        }

        // todo: need refactoring
        private void TryAddScore(UnitBase sender, UnitBase deadOn)
        {
            var possiblePlayer = sender as PlayerUnit;
            var possibleEnemy = deadOn as EnemyUnit;
            if (possiblePlayer != null &&
                possibleEnemy != null)
            {
                possiblePlayer.Score.Count += 1;
                if ((possiblePlayer.Score.Count % WorldLaw.LevelUpThreshold) == 0)
                {
                    sender.World.WorldState.LevelUp = true;
                }
            }
        }

        private UnitBase m_deadOn = null;
    }
}

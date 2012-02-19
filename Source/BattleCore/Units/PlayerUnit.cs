using System;
using System.Drawing;
using Vortex.Drawing;
using BattleCore.Units.Bonuses;

namespace BattleCore.Units
{
    public class PlayerUnit : UnitBase
    {
        const float TurnThresold = 0.01f;

        public enum PlayerState
        {
            Running,
            Waiting,
            Dead
        }

        public PlayerUnit(World map)
            : base(map, WorldLaw.PlayerUnitCreationSpeed,
            WorldLaw.DefaultPlayerLifeCount)
        {
            State = PlayerState.Waiting;
        }

        public PlayerUnit(World map, Point point)
            : base(map, WorldLaw.PlayerUnitCreationSpeed,
            WorldLaw.DefaultPlayerLifeCount)
        {
            SetLocation(point.X, point.Y);
            State = PlayerState.Waiting;
            m_prevLocation.X = point.X;
            m_prevLocation.Y = point.Y;
        }

        public PlayerState State { get; set; }

        public Nullable<UnitDirection> UserDirection { get; set; }

        private Vector m_prevLocation;

        private void CheckLocation()
        {
            if((Location.Distance(Location.Round()) < 0.1) &&
                !m_prevLocation.Equals(Location.Round()))
            {
                m_prevLocation = Location.Round();
                World.FindPath((int)m_prevLocation.X, (int)m_prevLocation.Y);
            }
        }

        public override void OnDamage(BulletProcessor damager)
        {
            damager.DoDamage(this);
        }

        public override void Update(float timeDelta)
        {
            if (Life.Count <= 0)
            {
                State = PlayerState.Dead;
                return;
            }
            
            TryAcceptBonus();

            if (UserDirection == null)
            {
                State = PlayerState.Waiting;
                return;
            }

            State = PlayerState.Running;

            UnitDirection userDirection = UserDirection.Value;

            bool turn = !userDirection.IsParallelTo(Direction);
            if (turn)
            {
                Vector cellLocation = new Vector((float)Math.Round(Location.X), (float)Math.Round(Location.Y));
                if(Location.Distance(cellLocation) < TurnThresold)
                {
                    if (CanGoTo(cellLocation + userDirection.ToVector()))
                    {
                        SetLocation(cellLocation);
                    }
                    Direction = userDirection;
                }
            }
            else
            {
                if (CanGoTo(Location + userDirection.ToVector() * Speed * timeDelta))
                {
                    Direction = userDirection;
                }
            }

            Vector location = Location + Direction.ToVector() * Speed * timeDelta;

            UnitBase unit;
            if (!CanGoTo(location, out unit))
            {
                Direction = UserDirection.Value;
                FixLocation(unit);
            }
            else
            {
                SetLocation(location);
            }

            CheckLocation();
        }


        protected override bool CanGoTo(Vector location, out UnitBase collision)
        {
            return World.Law.IsAccessible(this, location, out collision);
        }

        private void TryAcceptBonus()
        {
            foreach (var bonus in World.Bonuses)
            {
                if (bonus.Location.Distance(Location) < WorldLaw.UnitCollisionDistance)
                {
                    World.BonusProcessor.AcceptBonus(this, bonus);
                    World.Bonuses.Remove(bonus);
                    return;
                }
            }
        }
    }
}

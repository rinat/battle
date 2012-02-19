using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vortex.Drawing;
using System.Drawing;

namespace BattleCore.Units
{
    public class EnemyUnit : UnitBase
    {
        public enum EnemyState
        {
            Running,
            Dead,
            Waiting
        }

        public const float ChaseBehaviourDistance = 20;
        //float delta = 0.1f;
        private Vector m_prevLocation;

        public EnemyUnit(World world, UnitBase target)
            : base(world, WorldLaw.EnemyUnitCreationSpeed,
            WorldLaw.DefaultEnemyLifeCount)
        {
            State = EnemyState.Running;
            Target = target;
        }

        private void CheckLocation()
        {
            if ((Location.Distance(Location.Round()) < 0.1) &&
                !m_prevLocation.Equals(Location.Round()))
            {
                m_prevLocation = Location.Round();
            }
        }

        public EnemyUnit(World world, Point position, UnitBase target)
            : base(world, WorldLaw.EnemyUnitCreationSpeed,
            WorldLaw.DefaultEnemyLifeCount)
        {
            State = EnemyState.Running;
            Target = target;
            
            SetLocation(position.X, position.Y);

            m_prevLocation.X = position.X;
            m_prevLocation.Y = position.Y;
        }

        public EnemyState State { get; set; }

        public UnitBase Target { get; private set; }

        #region Unit Members

        public override void OnDamage(BulletProcessor damager)
        {
            damager.DoDamage(this);
        }

        public override void Update(float timeDelta)
        {
            if (World.WorldState.GameComplete)
            {
                State = EnemyState.Waiting;
                return;
            }

            if (Life.Count == 0)
            {
                State = EnemyState.Dead;
                return;
            }

            m_timeStep += Speed * timeDelta;
            //check for choosing next direction

            UnitBase unit;

            if(Location.Distance(Location.Round()) < 0.1)
            {
                ChooseNextDirection();
            }
            
            
            Vector newLocation = Location + Direction.ToVector() * Speed * timeDelta;

            if(CanGoTo(newLocation, out unit))
            {
                SetLocation(newLocation);
            }
            else
            {
                FixLocation(unit);
                ChooseNextDirection();
            }

            if (m_timeStep >= 1.0f)
            {
                TryFire();
            }

            CheckLocation();

            base.Update(timeDelta);
        }



        #endregion Unit Members

        private int CountNumberOfMovementOptions(Vector fromPoint)
        {
            int counter = 0;
            if (CanGoTo(fromPoint + UnitDirection.Right.ToVector()))
            {
                counter++;
            }
            if (CanGoTo(fromPoint + UnitDirection.Left.ToVector()))
            {
                counter++;
            }
            if (CanGoTo(fromPoint + UnitDirection.Up.ToVector()))
            {
                counter++;
            }
            if (CanGoTo(fromPoint + UnitDirection.Down.ToVector()))
            {
                counter++;
            }
            return counter;
        }

        private void ChooseNextDirection()
        {
            List<UnitDirection> directions = new List<UnitDirection>();
            directions.Add(UnitDirection.Right);
            directions.Add(UnitDirection.Left);
            directions.Add(UnitDirection.Up);
            directions.Add(UnitDirection.Down);

            UnitDirection best = UnitDirection.None;
            int minDistance = World.Map[(int)m_prevLocation.X, (int)m_prevLocation.Y];
            foreach (UnitDirection direction in directions)
            {
                Vector location = m_prevLocation + direction.ToVector();
                int distance = World.Map[(int)location.X, (int)location.Y];
                if(distance < minDistance)
                {
                    minDistance = distance;
                    best = direction;
                }
            }

            if (!best.Equals(UnitDirection.None) && !Direction.Equals(best))
            {
                Direction = best;
                FixLocation();
            }
            
        }
         
        private void TryFire()
        {
            if ((int)Location.Y == (int)Target.Location.Y)
            {
                if (Target.Location.X > Location.X &&
                    Direction.Equals(UnitDirection.Right))
                {
                    foreach (var enemy in World.Enemies)
                    {
                        if (enemy.Location.X > this.Location.X &&
                            enemy.Location.X < Target.Location.X)
                        {
                            return;
                        }
                    }
                    Fire();
                }
                else if (Target.Location.X < Location.X &&
                    Direction.Equals(UnitDirection.Left))
                {
                    foreach (var enemy in World.Enemies)
                    {
                        if (enemy.Location.X < this.Location.X &&
                            enemy.Location.X > Target.Location.X)
                        {
                            return;
                        }
                    }
                    Fire();
                }
            }
            else if ((int)Location.X == (int)Target.Location.X)
            {
                if (Target.Location.Y < Location.Y &&
                    Direction.Equals(UnitDirection.Up))
                {
                    foreach (var enemy in World.Enemies)
                    {
                        if (enemy.Location.Y < this.Location.Y &&
                            enemy.Location.Y > Target.Location.Y)
                        {
                            return;
                        }
                    }
                    Fire();
                }
                else if (Target.Location.Y > Location.Y &&
                    Direction.Equals(UnitDirection.Down))
                {
                    foreach (var enemy in World.Enemies)
                    {
                        if (enemy.Location.Y > this.Location.Y &&
                            enemy.Location.Y < Target.Location.Y)
                        {
                            return;
                        }
                    }
                    Fire();
                }
            }
        }

        private void Fire()
        {
            var nLocation = Location + Direction.ToVector();
            var unit = new BulletUnit(World, nLocation.ToPointF(), this);
            unit.Direction = Direction;
            World.Bullets.Add(unit);
            m_timeStep = 0.0f;
        }

        protected override bool CanGoTo(Vector location, out UnitBase collision)
        {
            return World.Law.IsAccessible(this, location, out collision);
        }

        private float m_timeStep;
        private static Random m_random = new Random();
    }
}

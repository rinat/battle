using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vortex.Drawing;

namespace BattleCore.Units
{
    public abstract class UnitBase
    {
        public UnitBase(World map, float speed, int lifeCount)
        {
            World = map;
            Speed = speed;
            Life = new Life(lifeCount);
            Score = new Score(0);
        }

        public object UserData { get; set; }

        public Life Life { get; set; }

        public Score Score { get; set; }

        public World World { get; private set; }

        public UnitDirection Direction { get; set; }

        public Vector Location { get { return m_location; } }

        public float Speed { get; set; }

        public void SetLocation(float atX, float atY)
        {
            m_location = new Vector(atX, atY);
        }

        public void SetLocation(Vector location)
        {
            m_location = location;
        }

        public void FixLocation(UnitBase unit)
        {
            if (unit == null)
            {
                m_location = Location.Round();
                return;
            }

            if(unit.Direction.Equals(UnitDirection.None))
            {
                m_location = Location.Round();
            }
            else
            {
                Vector p1 = Location;
                Vector p2 = unit.Location;

                if (Direction.IsParallelTo(unit.Direction))
                {
                    m_location = p1 + Direction.ToVector() * (p1.Distance(p2) - 1);
                }
                else
                {
                    m_location = Location.Round();
                }
            }            
        }

        public void FixLocation()
        {
            m_location = Location.Round();
        }

        public abstract void OnDamage(BulletProcessor damager);   

        ///<summary>Updates unit for a some frame time delta (in sec)</summary>
        public virtual void Update(float timeDelta)
        {
            Vector mapSize = new Vector(World.Width - 1, World.Height - 1);
            if (Location.X < 0.0f)
            {
                m_location.X += mapSize.X;
            }
            if (Location.X > mapSize.X)
            {
                m_location.X -= mapSize.X;
            }
            if (Location.Y < 0.0f)
            {
                m_location.Y += mapSize.Y;
            }
            if (Location.Y > mapSize.Y)
            {
                m_location.Y -= mapSize.Y;
            }
        }

        protected abstract bool CanGoTo(Vector location, out UnitBase collision);

        protected virtual bool CanGoTo(Vector location)
        {
            UnitBase collision;
            return CanGoTo(location, out collision);
        }

        private Vector m_location;
    }
}

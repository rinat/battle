using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Vortex.Drawing;

namespace BattleCore.Units
{
    public struct UnitDirection
    {
        int m_HorizontalOffset;
        int m_VerticalOffset;

        UnitDirection(int horizontalOffset, int verticalOffset)
        {
            m_VerticalOffset = verticalOffset;
            m_HorizontalOffset = horizontalOffset;
        }

        public UnitDirection(Point from, Point to)
        {
            m_VerticalOffset = to.Y - from.Y;
            m_HorizontalOffset = to.X - from.X;
            
            //validate offset
            if (m_HorizontalOffset > 1 || m_VerticalOffset > 1)
            {
                throw new Exception("Invalid unit direction");
            }
        }

        public int HorizontalOffset
        {
            get { return m_HorizontalOffset; }
        }

        public int VerticalOffset
        {
            get { return m_VerticalOffset; }
        }

        public float Angle
        {
            get { return (float)Math.Atan2((double)m_VerticalOffset, (double)m_HorizontalOffset); }
        }

        public bool IsEmpty
        {
            get { return 0 == VerticalOffset && 0 == HorizontalOffset; }
        }

        ///<summary>Returns true if directions are parallel</summary>
        public bool IsParallelTo(UnitDirection direction)
        {
            return
                (VerticalOffset == direction.VerticalOffset && (HorizontalOffset == direction.HorizontalOffset || HorizontalOffset == -direction.HorizontalOffset)) ||
                (HorizontalOffset == direction.HorizontalOffset && (VerticalOffset == direction.VerticalOffset || VerticalOffset == -direction.VerticalOffset));
        }

        public override bool Equals(object obj)
        {
            if (obj is UnitDirection)
            {
                return Equals((UnitDirection)obj);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return m_VerticalOffset ^ m_HorizontalOffset;
        }

        public bool Equals(UnitDirection dir)
        {
            return this.HorizontalOffset == dir.HorizontalOffset && this.VerticalOffset == dir.VerticalOffset;
        }

        public Vector ToVector()
        {
            return new Vector(HorizontalOffset, VerticalOffset);
        }

        public UnitDirection Negate()
        {
            return new UnitDirection(-m_HorizontalOffset, -m_VerticalOffset);
        }

        public readonly static UnitDirection None = new UnitDirection(0, 0);
        public readonly static UnitDirection Left = new UnitDirection(-1, 0);
        public readonly static UnitDirection Up = new UnitDirection(0, -1);
        public readonly static UnitDirection Right = new UnitDirection(1, 0);
        public readonly static UnitDirection Down = new UnitDirection(0, 1);

    }

}

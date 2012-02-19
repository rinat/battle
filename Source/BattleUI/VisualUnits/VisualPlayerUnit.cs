using BattleCore.Units;
using Vortex.Drawing;
using Vortex;
using Vortex.Input;
using System.Drawing;
using BattleCore;

namespace BattleUI.VisualUnits
{
    class VisualPlayerUnit : VisualUnit
    {
        public VisualPlayerUnit(PlayerUnit playerUnit, SpriteAnimation spriteAnimation)
        {
            PlayerUnit = playerUnit;
            MovementAnimation = spriteAnimation;
        }

        private float m_fireLimit = 0.0f;

        public PlayerUnit PlayerUnit { get; private set; }

        public SpriteAnimation MovementAnimation { get; private set; }

        #region VisualUnit Members

        public override void Update(float timeDelta)
        {
            PlayerUnit.UserDirection = null;
            m_fireLimit += timeDelta;
            if (Game.Keyboard.IsReleased(Key.Space) && IsRecharged())
            {
                OnFire();
            }
            if (Game.Keyboard.IsDown(Key.Down))
            {
                PlayerUnit.UserDirection = UnitDirection.Down;
            }
            else if (Game.Keyboard.IsDown(Key.Up))
            {
                PlayerUnit.UserDirection = UnitDirection.Up;
            }
            else if (Game.Keyboard.IsDown(Key.Left))
            {
                PlayerUnit.UserDirection = UnitDirection.Left;
            }
            else if (Game.Keyboard.IsDown(Key.Right))
            {
                PlayerUnit.UserDirection = UnitDirection.Right;
            }

            if (PlayerUnit.State == PlayerUnit.PlayerState.Running)
            {
                MovementAnimation.Update(timeDelta);
            }
        }

        public override void Render(Canvas2D canvas, RendererBase renderer)
        {
            if (MovementAnimation != null)
            {
                renderer.Render(canvas, this);
            }
        }

        #endregion VisualUnit Members

        private void OnFire()
        {
            m_fireLimit = 0.0f;
            Vector bulletStartPoint = PlayerUnit.Location + PlayerUnit.Direction.ToVector();
         
            var bulletUnit = new BulletUnit(PlayerUnit.World,
                bulletStartPoint.ToPointF(), PlayerUnit);

            bulletUnit.Direction = PlayerUnit.Direction;
            bulletUnit.FixLocation();

            bulletUnit.UserData = new VisualBulletUnit(bulletUnit,
                Gfx.Instance.GetPlayerBulletFlightAnimation());

            PlayerUnit.World.Bullets.Add(bulletUnit);
        }

        private bool IsRecharged()
        {
            switch (PlayerUnit.World.WorldState.LevelNumber)
            {
                case 1:
                    return m_fireLimit >= 0.5;
                case 2:
                    return m_fireLimit >= 0.3;
                case 3:
                    return m_fireLimit >= 0.2;
                default:
                    return m_fireLimit >= 0.1;
            }
        }
    }
}

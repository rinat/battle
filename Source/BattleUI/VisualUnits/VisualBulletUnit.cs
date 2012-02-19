using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleCore.Units;
using Vortex.Drawing;
using Vortex.Drawing.Particles;

namespace BattleUI.VisualUnits
{
    class VisualBulletUnit : VisualUnit
    {
        public VisualBulletUnit(BulletUnit bulletUnit,
            SpriteAnimation flightAnimation)
        {
            BulletUnit = bulletUnit;
            FlightAnimation = flightAnimation;
        }

        public BulletUnit BulletUnit { get; private set; }

        public SpriteAnimation FlightAnimation { get; private set; }

        #region VisualUnit Members

        public override void Update(float timeDelta)
        {
            FlightAnimation.Update(timeDelta);
        }

        public override void Render(Canvas2D canvas, RendererBase renderer)
        {
            renderer.Render(canvas, this);
        }

        #endregion VisualUnit Members
    }
}

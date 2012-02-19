using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vortex.Drawing;
using BattleCore.Units.Bonuses;

namespace BattleUI.VisualUnits
{
    class VisualBonus : VisualUnit
    {
        public VisualBonus(BonusBase bonus, SpriteAnimation animation)
        {
            BonusUnit = bonus;
            BonusAnimation = animation;
        }

        public BonusBase BonusUnit { get; private set; }

        public SpriteAnimation BonusAnimation { get; private set; }

        #region VisualUnit Members

        public override void Update(float timeDelta)
        {
            BonusAnimation.Update(timeDelta);
        }

        public override void Render(Canvas2D canvas, RendererBase renderer)
        {
            if (BonusAnimation != null)
            {
                renderer.Render(canvas, this);
            }
        }

        #endregion VisualUnit Members
    }
}

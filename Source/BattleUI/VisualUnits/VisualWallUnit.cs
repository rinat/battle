using BattleCore.Units;
using Vortex.Drawing;

namespace BattleUI.VisualUnits
{
    class VisualWallUnit : VisualUnit
    {
        public VisualWallUnit(WallUnit wallUnit, Sprite sprite)
        {
            WallUnit = wallUnit;
            Sprite = sprite;
        }

        public WallUnit WallUnit { get; private set; }

        public Sprite Sprite { get; private set; }

        #region VisualUnit Members

        public override void Update(float timeDelta)
        {}

        public override void Render(Canvas2D canvas, RendererBase renderer)
        {
            if (Sprite != null)
            {
                renderer.Render(canvas, this);
            }
        }

        #endregion VisualUnit Members
    }
}

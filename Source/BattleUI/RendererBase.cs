using BattleUI.VisualUnits;
using Vortex.Core;
using Vortex.Drawing;
using BattleCore;

namespace BattleUI
{
    class RendererBase : IUpdatable
    {
        public virtual void Render(Canvas2D canvas, VisualUnit unit)
        { }
        public virtual void Render(Canvas2D canvas, VisualEnemyUnit unit)
        { }
        public virtual void Render(Canvas2D canvas, VisualWallUnit unit)
        { }
        public virtual void Render(Canvas2D canvas, VisualPlayerUnit unit)
        { }
        public virtual void Render(Canvas2D canvas, VisualBulletUnit unit)
        { }
        public virtual void Render(Canvas2D canvas, VisualBonus unit)
        { }

        public virtual void Render(Canvas2D canvas)
        { }

        public virtual void Render(Canvas2D canvas, RenderTarget target)
        { }

        #region IUpdatable Members

        public virtual void Update(float timeDelta)
        { }

        #endregion IUpdatable Members
    }
}

using Vortex.Core;
using Vortex.Drawing;

namespace BattleUI.VisualUnits
{
    abstract class VisualUnit : IUpdatable
    {
        #region IUpdatable Members

        public virtual void Update(float timeDelta)
        { }

        #endregion

        public virtual void Render(Canvas2D canvas, RendererBase renderer)
        { }
    }
}

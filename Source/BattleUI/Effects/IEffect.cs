using Vortex.Core;
using Vortex.Drawing;

namespace BattleUI.Effects
{
    interface IEffect : IUpdatable
    {
        void Render(Canvas2D canvas, RenderTarget target);
    }
}

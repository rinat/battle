using System;
using Vortex.Drawing;

namespace BattleUI.Effects
{
    class ShakeEffect : IEffect
    {
        public ShakeEffect(float shakeStrength,
            float shakeMinimizingSpeed)
        {
            m_random = new Random();
            ShakeLength = shakeStrength;
            ShakeMinimizingSpeed = shakeMinimizingSpeed;
        }

        public float ShakeLength { get; set; }

        public float ShakeMinimizingSpeed { get; set; }

        #region IEffect Members

        public void Render(Canvas2D canvas, RenderTarget target)
        {
            Vector offset = Vector.Zero;
            if (ShakeLength > 0)
            {
                offset.X = (float)(m_random.NextDouble() - 0.5) * ShakeLength;
                offset.Y = (float)(m_random.NextDouble() - 0.5) * ShakeLength;
            }

            float progress = 1;
            canvas.DrawSprite(canvas.Region + offset, target.ToSprite(),
                ColorU.White.SemiTransparent(progress));
        }

        #endregion IEffect Members

        #region IUpdatable Members

        public void Update(float timeDelta)
        {
            if (ShakeLength > 0)
            {
                ShakeLength -= ShakeMinimizingSpeed * timeDelta;
            }
        }

        #endregion IUpdatable Members

        private Random m_random;
    }
}

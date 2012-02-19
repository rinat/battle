using BattleUI.VisualUnits;
using Vortex.Drawing;
using BattleCore.Units;
using System.Collections.Generic;
using Vortex.Drawing.Particles;
using BattleUI.Effects;
using BattleCore;

namespace BattleUI
{
    class WorldRenderer : RendererBase
    {
        public const int BulletExplosionParticlesCount = 200;
        public const float ShakeStrength = 10;
        public const float ShakeMinimizingSpeed = 15;

        public WorldRenderer(int tileSize)
        {
            TileSize = tileSize;
            _shakeEffect = new ShakeEffect(0, ShakeMinimizingSpeed);

            _bulletExplosion = Gfx.Instance.RocketExplosionParticleEffect.CreateParticleSystem();
            _bulletExplosion.Continous = false;

            _unitExplosion = Gfx.Instance.UnitExplosion.CreateParticleSystem();
            _unitExplosion.Continous = false;
        }

        public int TileSize { get; private set; }

        public override void Render(Canvas2D canvas, VisualUnit unit)
        {
            base.Render(canvas, unit);
        }

        public override void Render(Canvas2D canvas, VisualEnemyUnit unit)
        {
            if (unit.EnemyUnit.State == EnemyUnit.EnemyState.Dead)
            {
                _unitExplosion.MoveTo(unit.EnemyUnit.Location * TileSize);
                _unitExplosion.Emit(BulletExplosionParticlesCount);
                return;
            }
            Vector pos = unit.EnemyUnit.Location * TileSize;
            float directionAngle = unit.EnemyUnit.Direction.Angle;
            canvas.DrawSprite(pos, directionAngle,
                unit.MovementAnimation.ToSprite(), ColorU.White);
        }

        public override void Render(Canvas2D canvas, VisualPlayerUnit unit)
        {
            if (unit.PlayerUnit.State == PlayerUnit.PlayerState.Dead &&
                !unit.PlayerUnit.World.WorldState.GameComplete)
            {
                _unitExplosion.MoveTo(unit.PlayerUnit.Location * TileSize);
                _unitExplosion.Emit(BulletExplosionParticlesCount);

                ActivateShackeEffect(ShakeStrength, ShakeMinimizingSpeed);

                return;
            }
            if (unit.PlayerUnit.State != PlayerUnit.PlayerState.Dead)
            {
                Vector pos = unit.PlayerUnit.Location * TileSize;
                float directionAngle = unit.PlayerUnit.Direction.Angle;
                canvas.DrawSprite(pos, directionAngle, unit.MovementAnimation.ToSprite(), ColorU.White);
            }
        }

        public override void Render(Canvas2D canvas, VisualWallUnit unit)
        {
            canvas.DrawSprite(unit.WallUnit.Location.X * TileSize, unit.WallUnit.Location.Y * TileSize,
                unit.Sprite, ColorU.White);
        }

        public override void Render(Canvas2D canvas, VisualBonus unit)
        {
            canvas.DrawSprite(unit.BonusUnit.Location.X * TileSize, unit.BonusUnit.Location.Y * TileSize,
                unit.BonusAnimation.ToSprite(), ColorU.White);
        }

        public override void Render(Canvas2D canvas, VisualBulletUnit unit)
        {
            if (unit.BulletUnit.BulletState ==
                BattleCore.Units.BulletUnit.BulletUnitState.Dead)
            {
                var possiblePlayerUnit = unit.BulletUnit.DeadOn as PlayerUnit;
                if (possiblePlayerUnit != null)
                {
                    ActivateShackeEffect(ShakeStrength, ShakeMinimizingSpeed);
                }

                _bulletExplosion.MoveTo(unit.BulletUnit.Location * TileSize);
                _bulletExplosion.Emit(BulletExplosionParticlesCount);
            }
            else
            {
                Vector pos = unit.BulletUnit.Location * TileSize;
                float directionAngle = unit.BulletUnit.Direction.Angle;
                canvas.DrawSprite(pos, directionAngle, unit.FlightAnimation.ToSprite(), ColorU.White);
            }
        }

        public override void Render(Canvas2D canvas)
        {
            using (canvas <= Blending.Add)
            {
                _bulletExplosion.Draw(canvas);
                _unitExplosion.Draw(canvas);
            }
        }

        public override void Render(Canvas2D canvas, RenderTarget target)
        {
            _shakeEffect.Render(canvas, target);
        }

        public override void Update(float timeDelta)
        {
            _shakeEffect.Update(timeDelta);
            _bulletExplosion.Update(timeDelta);
            _unitExplosion.Update(timeDelta);
        }

        public void ActivateShackeEffect(float shakeStrength, float minimizingSpeed)
        {
            _shakeEffect.ShakeLength = shakeStrength;
            _shakeEffect.ShakeMinimizingSpeed = minimizingSpeed;
        }

        private ShakeEffect _shakeEffect;
        private ParticleSystem _unitExplosion;
        private ParticleSystem _bulletExplosion;
    }
}

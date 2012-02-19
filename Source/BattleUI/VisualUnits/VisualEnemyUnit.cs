using BattleCore.Units;
using Vortex.Drawing;

namespace BattleUI.VisualUnits
{
    class VisualEnemyUnit : VisualUnit
    {
        public VisualEnemyUnit(EnemyUnit enemyUnit, SpriteAnimation spriteAnimation)
        {
            EnemyUnit = enemyUnit;
            MovementAnimation = spriteAnimation;
        }

        public EnemyUnit EnemyUnit { get; private set; }

        public SpriteAnimation MovementAnimation { get; private set; }

        #region VisualUnit Members

        public override void Update(float timeDelta)
        {
            if (EnemyUnit.State == EnemyUnit.EnemyState.Running)
            {
                MovementAnimation.Update(timeDelta);
            }
        }

        public override void Render(Canvas2D canvas, RendererBase renderer)
        {
            renderer.Render(canvas, this);
        }

        #endregion VisualUnit Members
    }
}

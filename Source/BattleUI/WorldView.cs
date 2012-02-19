using System.Collections.Generic;
using BattleCore;
using BattleCore.Units;
using BattleUI.VisualUnits;
using Vortex;
using Vortex.Core;
using Vortex.Drawing;
using BattleCore.Units.Bonuses;

namespace BattleUI
{
    class WorldView : IUpdatable
    {
        public const int TileSize = 32;
        public const int HalfTileSize = 16;

        public WorldView(World world)
        {
            World = world;
        }

        public World World  { get; private set; }

        #region Members IUpdatable

        public void Update(float timeDelta)
        {
            m_renderer.Update(timeDelta);

            VisualUnit visualUnit = null;
            foreach (var enemy in World.Enemies)
            {
                visualUnit = enemy.UserData as VisualUnit;
                if (visualUnit != null)
                {
                    visualUnit.Update(timeDelta);
                }
            }
            foreach (var wall in World.Walls)
            {
                visualUnit = wall.UserData as VisualUnit;
                if (visualUnit != null)
                {
                    visualUnit.Update(timeDelta);
                } 
            }
            foreach (var bullet in World.Bullets)
            {
                visualUnit = bullet.UserData as VisualUnit;
                if (visualUnit != null)
                {
                    visualUnit.Update(timeDelta);
                }
            }
            foreach (var bonus in World.Bonuses)
            {
                visualUnit = bonus.UserData as VisualUnit;
                if (visualUnit != null)
                {
                    visualUnit.Update(timeDelta);
                }
            }
            visualUnit = World.Player.UserData as VisualUnit;
            if (visualUnit != null)
            {
                visualUnit.Update(timeDelta);
            }
        }

        #endregion Members IUpdatable

        public void Render(Canvas2D canvas)
        {
            float worldOffsetX = (Game.Window.Width + TileSize - World.Width * TileSize) / 2;
            float worldOffsetY = (Game.Window.Height + TileSize - World.Height * TileSize) / 2;

            using (canvas <= m_frameBuffer)
            {
                canvas.Clear(ColorU.Black);
                DrawGround(canvas);
                using (canvas <= new Translation(worldOffsetX, worldOffsetY))
                {
                    DrawPlayer(canvas, World.Player);
                    DrawEnemies(canvas, World.Enemies);
                    RenderWalls(canvas, World.Walls);
                    DrawBullets(canvas, World.Bullets);
                    DrawBonuses(canvas, World.Bonuses);
                    m_renderer.Render(canvas);
                }
            }
            m_renderer.Render(canvas, m_frameBuffer);
            DrawWorldEvents(canvas, World);
        }

        private void DrawGround(Canvas2D canvas)
        {
            canvas.DrawSprite(canvas.Region, Gfx.Instance.GetGroundSprite(), ColorU.White);
        }

        private void DrawPlayer(Canvas2D canvas, PlayerUnit unit)
        {
            if (unit != null)
            {
                var visualUnit = (VisualUnit)unit.UserData;
                visualUnit.Render(canvas, m_renderer);
            }
        }

        private void RenderWalls(Canvas2D canvas, List<WallUnit> walls)
        {
            foreach (var wall in walls)
            {
                var visualUnit = (VisualUnit)wall.UserData;
                visualUnit.Render(canvas, m_renderer);
            }
        }

        private void DrawEnemies(Canvas2D canvas, List<EnemyUnit> enemies)
        {
            foreach (EnemyUnit unit in enemies)
            {
                var visualUnit = (VisualUnit)unit.UserData;
                visualUnit.Render(canvas, m_renderer);
            }
        }

        private void DrawBonuses(Canvas2D canvas, List<BonusBase> bonuses)
        {
            foreach (var bonus in bonuses)
            {
                var visualUnit = (VisualUnit)bonus.UserData;
                visualUnit.Render(canvas, m_renderer);
            }
        }

        private void DrawBullets(Canvas2D canvas, List<BulletUnit> bullets)
        {
            foreach (BulletUnit bullet in bullets)
            {
                if (bullet.UserData == null)
                {
                    bullet.UserData = new VisualBulletUnit(bullet,
                        Gfx.Instance.GetPlayerBulletFlightAnimation());
                }
                var visualUnit = (VisualUnit)bullet.UserData;
                visualUnit.Render(canvas, m_renderer);
            }
        }

        private void DrawWorldEvents(Canvas2D canvas, World world)
        {
            DrawBonusBoom(canvas, world);
            DrawGameOver(canvas, world);
            DrawPlayerLife(canvas, world);
            DrawPlayerScore(canvas, world);
            DrawLevelInfo(canvas, world);
        }

        private void DrawPlayerScore(Canvas2D canvas, World world)
        {

            float scoresHeight = (world.Height * TileSize) - HalfTileSize;
            string text = "Score " + World.Player.Score.Count;

            Vector textSize = canvas.MeasureString(Fonts.Instance.TanksSmallFont, text);
            canvas.DrawString(Fonts.Instance.TanksAltFont,
                new Vector(TileSize, World.Height * TileSize - HalfTileSize),
                text, ColorU.White);
        }

        private void DrawPlayerLife(Canvas2D canvas, World world)
        {
            float extrasHeight = (world.Height * TileSize) - HalfTileSize;
            string text = "Life " + World.Player.Life.Count;
            Vector textSize = canvas.MeasureString(Fonts.Instance.TanksSmallFont, text);
            canvas.DrawString(Fonts.Instance.TanksAltFont,
                new Vector((world.Width * TileSize - HalfTileSize) - textSize.X, extrasHeight),
                text, ColorU.White);
        }

        private void DrawBonusBoom(Canvas2D canvas, World world)
        {
            if (World.WorldState.BonusExplosion)
            {
                m_renderer.ActivateShackeEffect(WorldRenderer.ShakeStrength * 5,
                    WorldRenderer.ShakeMinimizingSpeed + WorldRenderer.ShakeMinimizingSpeed);
            }
        }

        private void DrawLevelInfo(Canvas2D canvas, World world)
        {
            float extrasHeight = TileSize - HalfTileSize;
            string text = "Level " + World.WorldState.LevelNumber;
            Vector textSize = canvas.MeasureString(Fonts.Instance.TanksSmallFont, text);
            canvas.DrawString(Fonts.Instance.TanksAltFont,
                new Vector(((world.Width / 2) * TileSize) + textSize.X, extrasHeight),
                text, ColorU.White);
        }

        private void DrawGameOver(Canvas2D canvas, World world)
        {
            if (World.Player.Life.Count == 0)
            {
                DrawTempEvent(canvas, world, "GAME OVER");
            }
        }

        private void DrawTempEvent(Canvas2D canvas, World world, string text)
        {
            Vector textSize = canvas.MeasureString(Fonts.Instance.TankBigFont, text);
            canvas.DrawString(Fonts.Instance.TankBigFont, (canvas.Size - textSize) / 2, text, ColorU.Yellow);
        }

        private RenderTarget m_frameBuffer = new RenderTarget();
        private WorldRenderer m_renderer = new WorldRenderer(TileSize);
    }
}

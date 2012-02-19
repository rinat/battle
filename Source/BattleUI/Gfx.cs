using Vortex.Drawing;
using Vortex.Drawing.Particles;
using System;

namespace BattleUI
{
    public class Gfx
    {
        private static Gfx m_instance;

        private Gfx()
        {
            var images = new ImageCollection();
            // walls
            images.AddImage("walls", "Resources/gfx/blocks/green-wall.png");
            images.AddImage("trees", "Resources/gfx/blocks/tree-wall.png");
            
            // players
            images.AddImage("player-movement", "Resources/gfx/tanks/tank.png");
            images.AddImage("enemy-movement", "Resources/gfx/tanks/enemy.png");
            images.AddImage("player-bullet-flight", "Resources/gfx/bullets/bullet.png");

            // menu
            images.AddImage("game-menu-background", "Resources/gfx/menu/title.png");
            images.AddImage("game-ground", "Resources/gfx/menu/title.png");

            // bonus
            images.AddImage("bonus-boom", "Resources/gfx/bonus/boom.png");
            images.AddImage("bonus-health", "Resources/gfx/bonus/health.png");
            images.AddImage("bonus-life", "Resources/gfx/bonus/life.png");

            m_worldTexture = new Texture(images, PixelFormat.DefaultAlpha);

            UnitExplosion = new ParticleEffect("Resources/gfx/particles/rocket-explosion-smoke.peff");
            RocketExplosionParticleEffect = new ParticleEffect("Resources/gfx/particles/rocket-explosion.peff");
        }

        public SpriteAnimation GetBonusBoomAnimation()
        {
            return (new SpriteAnimation(m_worldTexture["bonus-boom"].Split(4, 4), 16));
        }

        public SpriteAnimation GetBonusHealthAnimation()
        {
            return (new SpriteAnimation(m_worldTexture["bonus-health"].Split(4, 4), 16));
        }

        public SpriteAnimation GetBonusLifeAnimation()
        {
            return (new SpriteAnimation(m_worldTexture["bonus-life"].Split(4, 4), 16));
        }

        public ParticleEffect RocketExplosionParticleEffect { get; private set; }

        public ParticleEffect UnitExplosion { get; private set; }

        public SpriteAnimation GetPlayerBulletFlightAnimation()
        {
            return (new SpriteAnimation(m_worldTexture["player-bullet-flight"].Split(8, 1), 16));
        }

        public Sprite GetMainMenuBackground()
        {
            return m_worldTexture["game-menu-background"].ToSprite();
        }

        public Sprite GetTreeWallSprite()
        {
            return m_worldTexture["trees"].ToSprite();
        }

        public Sprite GetGroundSprite()
        {
            return m_worldTexture["game-ground"].ToSprite();
        }

        public Sprite GetMetalWallSprite()
        {
            return m_worldTexture["walls"].ToSprite();
        }

        public SpriteAnimation GetPlayerMovementAnimation()
        {
            return (new SpriteAnimation(m_worldTexture["player-movement"].Split(4, 1), 16));
        }

        public SpriteAnimation GetEnemyMovementAnimation()
        {
            return (new SpriteAnimation(m_worldTexture["enemy-movement"].Split(4, 1), 16));
        }

        public static Gfx Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new Gfx();
                }
                return m_instance;
            }
        }

        private Texture m_worldTexture;
    }
}

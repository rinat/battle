using Vortex;
using Vortex.Drawing;
using Vortex.SceneGraph;

namespace BattleUI
{
    class BattleGame : Game
    {
        public readonly string WindowTitle = "Battle Game";
        public const int ResolutionX = 1024;
        public const int ResolutionY = 768;

        public BattleGame()
            : base()
        {
            Game.Window.Title = WindowTitle;
            Game.Window.Resize(ResolutionX, ResolutionY);
        }

        #region Game Members

        protected override void Load()
        {
            m_MainMenuScene = new MainMenuScene();
            Screen.SetScene(m_MainMenuScene);
        }

        protected override void Update(GameTime time)
        {
            Screen.Update(time);
        }

        protected override void Render(Canvas2D canvas)
        {
            canvas.Clear(ColorU.Black);
            Screen.Draw(canvas);
        }

        protected override void Unload()
        {
        }

        #endregion Game Members

        private MainMenuScene m_MainMenuScene;
    }
}

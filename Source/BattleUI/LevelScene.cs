using BattleCore;
using Vortex;
using Vortex.Input;
using Vortex.SceneGraph;
using BattleUI.VisualUnits;
using BattleCore.Units;
using BattleUI.Life;

namespace BattleUI
{
    class LevelScene : Scene
    {
        public LevelScene(MainMenuScene owner)
        {
            _world = new World();
            _worldView = new WorldView(_world);
            _mainMenuScene = owner;
        }

        public bool Paused { get; private set; }

        public bool GotoMainMenu { get; private set; }

        #region Members Scene Override

        public override void Update(Vortex.GameTime gameTime)
        {
            base.Update(gameTime);
            if (!Paused)
            {
                _world.Update(gameTime.FrameTime);
                _worldView.Update(gameTime.FrameTime);
                _enemiesGenerator.Update(gameTime.FrameTime);
                _bonusesGenerator.Update(gameTime.FrameTime);
            }
            else if (GotoMainMenu)
            {
                Screen.PopScene();
            }

            if (Game.Keyboard.IsPressed(Key.P))
            {
                Paused = !Paused;
            }
            if (Game.Keyboard.IsPressed(Key.Escape))
            {
                Paused = true;
                GotoMainMenu = true;
            }
        }

        protected override void DrawSelf(Vortex.Drawing.Canvas2D canvas)
        {
            _worldView.Render(canvas);
        }

        #endregion Members Scene Override

        public void StartLevel(int levelNumber)
        {
            var levelInfo = new LevelInfo();
            levelInfo.Load(levelNumber);

            // world properties
            _world.Width = levelInfo.Width;
            _world.Height = levelInfo.Height;

            // init player
            _world.Player = new PlayerUnit(_world, levelInfo.Player);
            _world.Player.Direction = UnitDirection.Up;
            _world.Player.UserData = new VisualPlayerUnit(_world.Player,
                Gfx.Instance.GetPlayerMovementAnimation());
            
            // init walls
            foreach (var wall in levelInfo.MetalWalls)
            {
                var wallUnit = new WallUnit(_world, wall, WallUnit.WallType.MetalWall);
                wallUnit.UserData = new VisualWallUnit(wallUnit, Gfx.Instance.GetMetalWallSprite());
                _world.Walls.Add(wallUnit);
            }
            foreach (var wall in levelInfo.TreeWalls)
            {
                var wallUnit = new WallUnit(_world, wall, WallUnit.WallType.TreeWall);
                wallUnit.UserData = new VisualWallUnit(wallUnit, Gfx.Instance.GetTreeWallSprite());
                _world.Walls.Add(wallUnit);
            }

            const int defaultEnemiesCount = 4;
            const int defaultBonusesCount = 4;
            _enemiesGenerator = new EnemiesPopulationController(defaultEnemiesCount + levelNumber,
                _world, levelInfo.EnemiesGenerationPoints);
            _bonusesGenerator = new BonusesPopulationController(defaultBonusesCount,
                _world, levelInfo.BonusGenerationPoints);

            _world.UpdateMap();
        }

        private World _world;
        private WorldView _worldView;
        private MainMenuScene _mainMenuScene;
        private EnemiesPopulationController _enemiesGenerator;
        private BonusesPopulationController _bonusesGenerator;
    }
}

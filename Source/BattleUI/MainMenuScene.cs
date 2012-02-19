using Vortex.Drawing;
using Vortex.SceneGraph;
using Vortex.SceneGraph.Gui;
using Vortex.Core;
using System;

namespace BattleUI
{
    class MainMenuScene : Scene
    {
        public MainMenuScene()
        {
            Vector menuStartAt = new Vector(BattleGame.ResolutionX / 2, BattleGame.ResolutionY / 2 + 20);

            TextWidget menuItem = new TextWidget();
            SetupMenuItem(menuItem);
            menuItem.Text = "Start Game";
            menuItem.Location = menuStartAt;
            menuItem.OnClick += delegate(Element sender, MouseEventArgs args)
            {
                StartGame();
            };
            m_menu.AddChild(menuItem);

            menuStartAt.Y += MenuItemSize.Y;

            menuItem = new TextWidget();
            SetupMenuItem(menuItem);
            menuItem.Text = "Instructions";
            menuItem.Location = menuStartAt;
            menuItem.OnClick += delegate(Element sender, MouseEventArgs args)
            {
                GoToSubmenu(m_instructionsMenu);
            };
            m_menu.AddChild(menuItem);

            menuStartAt.Y += MenuItemSize.Y;

            menuItem = new TextWidget();
            SetupMenuItem(menuItem);
            menuItem.Text = "About";
            menuItem.Location = menuStartAt;
            menuItem.OnClick += delegate(Element sender, MouseEventArgs args)
            {
                GoToSubmenu(m_aboutMenu);
            };
            m_menu.AddChild(menuItem);

            menuStartAt.Y += MenuItemSize.Y;

            menuItem = new TextWidget();
            SetupMenuItem(menuItem);
            menuItem.Text = "Exit";
            menuItem.Location = menuStartAt;
            menuItem.OnClick += delegate(Element sender, MouseEventArgs args)
            {
                BattleGame.Terminate();
            };
            m_menu.AddChild(menuItem);

            menuItem = new TextWidget();
            SetupMenuItem(menuItem);
            menuItem.Text = "Back To Menu";
            menuItem.Location = new Vector(menuStartAt.X, 500);
            menuItem.OnClick += delegate(Element sender, MouseEventArgs args)
            {
                BackToMenu(m_instructionsMenu);
            };
            m_instructionsMenu.AddChild(menuItem);

            menuItem = new TextWidget();
            SetupMenuItem(menuItem);
            menuItem.Text = "Back To Menu";
            menuItem.Location = new Vector(menuStartAt.X, 500);
            menuItem.OnClick += delegate(Element sender, MouseEventArgs args)
            {
                BackToMenu(m_aboutMenu);
            };
            m_aboutMenu.AddChild(menuItem);

            m_aboutMenu.Location = new Vector(BattleGame.ResolutionX, 0);
            m_instructionsMenu.Location = new Vector(BattleGame.ResolutionX, 0);

            CreateAboutMenuContent(m_aboutMenu);
            CreateInstructionsMenu(m_instructionsMenu);

            AddChild(m_menu);
            AddChild(m_aboutMenu);
            AddChild(m_instructionsMenu);            
        }

        private void CreateAboutMenuContent(Element menu)
        {
            TextWidget text = new TextWidget();
            text.Font = Fonts.Instance.TanksAltFont;
            text.Location = new Vector(BattleGame.ResolutionX / 2, 350);
            text.Size = new Vector(600, 280);

            {
                var gameInfo = "Simple Tanks game (based on Vortex2D.NET).\n";
                var courseInfo = "Course - The Perspective Software\n";
                var lecturerInfo = "Lecturer - Druzhinin U.V.\n";
                var studentsInfo = "Students - Zhdanov A.V., Shaihutdinov R.G.\n";

                text.Text = gameInfo + courseInfo + lecturerInfo + studentsInfo;
                text.Enabled = false;
            }

            menu.AddChild(text);
        }

        private void CreateInstructionsMenu(Element menu)
        {
            TextWidget text = new TextWidget();
            text.Font = Fonts.Instance.TanksAltFont;
            text.Location = new Vector(BattleGame.ResolutionX / 2, 350);
            text.Size = new Vector(300, 280);

            text.Text = "Arrow keys - movement,\nSpace - fire,\nP - pause,\nEscape - main menu";
            text.Enabled = false;

            menu.AddChild(text);
        }

        private void SetupMenuItem(TextWidget tw)
        {
            tw.Font = Fonts.Instance.TankBigFont;
            tw.TextPallete = MenuTextPallete;
            tw.Size = MenuItemSize;
            tw.CursorType = Vortex.Input.CursorType.Pointer;
        }

        private void GoToSubmenu(Element menu)
        {
            Timeline.AddEvent(new ProgressTimeEvent(MenuChangeTime, delegate(TimeEvent e)
            {
                MoveMenu(-e.Progress, m_menu, menu);
            }));
        }

        private void BackToMenu(Element menu)
        {
            Timeline.AddEvent(new ProgressTimeEvent(MenuChangeTime, delegate(TimeEvent e)
            {
                MoveMenu(e.Progress - 1, m_menu, menu);
            }));
        }

        private void MoveMenu(float progress, Element leftMenu, Element rightMenu)
        {
            progress = progress * Math.Abs(progress);
            leftMenu.Location = new Vector(BattleGame.ResolutionX * progress, leftMenu.Location.Y);
            rightMenu.Location = new Vector(BattleGame.ResolutionX * (1 + progress), leftMenu.Location.Y);
        }

        private void StartGame()
        {
            m_levelScene = new LevelScene(this);
            m_levelScene.StartLevel(1);
            Screen.PushScene(m_levelScene);
        }

        protected override void DrawSelf(Canvas2D canvas)
        {
            canvas.DrawSprite(canvas.Region, Gfx.Instance.GetMainMenuBackground(), ColorU.White);
            canvas.DrawString(Fonts.Instance.TanksAltFont, BattleGame.ResolutionX / 2 - 2, 190,
                "Powered by PS-51", ColorU.Lime);
        }

        private const float MenuChangeTime = 0.35f;
        private static readonly Vector MenuItemSize = new Vector(240, 60);
        private readonly ElementPallete MenuTextPallete = new ElementPallete(
            ColorU.White, ColorU.Yellow, ColorU.OrangeRed);

        private LevelScene m_levelScene;
        private Element m_menu = new Element();
        private Element m_aboutMenu = new Element();
        private Element m_instructionsMenu = new Element();
    }
}

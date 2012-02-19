using System;
using Vortex.Drawing;

namespace BattleUI
{
    class Fonts
    {
        private static Fonts m_instance;

        private Fonts()
        {
            TankBigFont = new SpriteFont(@"Resources\fonts\Brady Bunch, 48px");
            TanksAltFont = new SpriteFont(@"Resources\fonts\tanks_alt");
            TanksSmallFont = new SpriteFont(@"Resources\fonts\tanks_small");
        }

        public SpriteFont TankBigFont { get; private set; }

        public SpriteFont TanksAltFont { get; private set; }

        public SpriteFont TanksSmallFont { get; private set; }

        public static Fonts Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new Fonts();
                }
                return m_instance;
            }
        }
    }
}

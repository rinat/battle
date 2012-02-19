using System;

namespace BattleUI
{
    static class Program
    {
        static void Main()
        {
            try
            {
                using (BattleGame game = new BattleGame())
                {
                    game.Run();
                }
            }
            catch (Exception e)
            {
                Vortex.Debug.Error(e, "Application failed");
            }
        }
    }
}

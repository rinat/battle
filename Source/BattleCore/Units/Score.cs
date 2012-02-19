using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCore.Units
{
    public class Score
    {
        public Score(int count)
        {
            Count = count;
        }

        public int Count { get; set; }
    }
}

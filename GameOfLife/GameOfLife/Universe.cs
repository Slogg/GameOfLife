using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public enum StatusLiveCells { Dead = 0, Live = 1 }

    class Universe
    {
        public int countColumns;
        public int countRows;
        public int countLiveCells;
        public StatusLiveCells[,] plane;
        void ClearBoard()
        {
            Array.Clear(plane, 0, countRows * countColumns);
        }

        void UpdateBoard()
        {
            
        }
    }
}

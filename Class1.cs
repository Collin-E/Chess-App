using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCS
{
    internal class chessPiece
    {
        public string name;
        public string team;
        public bool isAlive;
        public int row;
        public int column;
        public chessPiece(string newName, string newTeam, int newRow, int newColumn) 
        {
            name = newName;
            team = newTeam;
            isAlive = true;
            row = newRow;
            column = newColumn;
        }
        public void kill()
        {
            isAlive = false;
        }
        public void revive()
        {
            isAlive = true;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessCS
{
    public partial class Form1 : Form
    {
        List<chessPiece> whitePieces;
        List<chessPiece> blackPieces;
        List<chessPiece> combinedPieces;
        List<List<Button>> buttonList;
        chessPiece clicked = new chessPiece("Temp", "NoTeam", -1, -1);
        chessPiece newClicked = new chessPiece("Temp", "NoTeam", -1, -1);
        chessPiece promotedPiece = new chessPiece("Temp", "NoTeam", -1, -1);
        int turn = 0; // 0 is white 1 is black
        //int saveI = -1;
        int saveNewClickedI = -1;
        bool whiteChecked = false;
        bool blackChecked = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<Button> buttonRow0 = new List<Button>() {b00, b01, b02, b03, b04, b05, b06, b07 };
            List<Button> buttonRow1 = new List<Button>() { b10, b11, b12, b13, b14, b15, b16, b17 };
            List<Button> buttonRow2 = new List<Button>() { b20, b21, b22, b23, b24, b25, b26, b27 };
            List<Button> buttonRow3 = new List<Button>() { b30, b31, b32, b33, b34, b35, b36, b37 };
            List<Button> buttonRow4 = new List<Button>() { b40, b41, b42, b43, b44, b45, b46, b47 };
            List<Button> buttonRow5 = new List<Button>() { b50, b51, b52, b53, b54, b55, b56, b57 };
            List<Button> buttonRow6 = new List<Button>() { b60, b61, b62, b63, b64, b65, b66, b67 };
            List<Button> buttonRow7 = new List<Button>() { b70, b71, b72, b73, b74, b75, b76, b77 };
            buttonList = new List<List<Button>>() { buttonRow0, buttonRow1, buttonRow2, buttonRow3, buttonRow4, buttonRow5, buttonRow6, buttonRow7 };

            queenPromo.Visible = false;
            rookPromo.Visible = false;
            knightPromo.Visible = false;
            bishopPromo.Visible = false;
            queenPromo.Enabled = false;
            rookPromo.Enabled = false;
            knightPromo.Enabled = false;
            bishopPromo.Enabled = false;

            //blacksTurnLabel.

            resetBoardPieces();
            makeBoard();
            //textBox1.Text += (findAvailableMoves(combinedPieces[15]).Count / 2).ToString() + "\r\n";
        }
        private void reviveAll()
        {
            for(int i = 0; i < combinedPieces.Count(); i++)
            {
                combinedPieces[i].revive();
            }
        }
        private void makeBoard()
        {
            if(turn == 0)
            {
                blackTurnLabel.Visible = false;
                whiteTurnLabel.Visible = true;
            }
            else if(turn == 1)
            {
                whiteTurnLabel.Visible = false;
                blackTurnLabel.Visible = true;
            }
            for(int i = 0; i < combinedPieces.Count(); i++)
            {
                chessPiece here = combinedPieces[i];
                string name = here.name;
                string team = here.team;
                int row = here.row;
                int column = here.column;
                bool isAlive = here.isAlive;
                if(isAlive)
                {
                    buttonList[row][column].Text = name;
                    if(team == "White")
                    {
                        buttonList[row][column].ForeColor = Color.Gray;
                    }
                    else
                    {
                        buttonList[row][column].ForeColor = Color.Black;
                    }
                    
                }
            }
            resetButtons();
        }
        private void resetButtons()
        {
            for(int i = 0; i < buttonList.Count(); i++)
            {
                for(int j = 0; j < buttonList[i].Count(); j++)
                {
                    if(buttonList[i][j].Text != "")
                    {
                        bool isThereARealPieceHere = false;
                        for(int k = 0; k < combinedPieces.Count(); k++)
                        {
                            chessPiece temp = combinedPieces[k];
                            if(temp.row == i && temp.column == j)
                            {
                                isThereARealPieceHere = true;
                            }
                        }
                        if(!isThereARealPieceHere)
                        {
                            buttonList[i][j].Text = "";
                        }
                    }
                }
            }
        }
        private List<int> findAvailableMoves(chessPiece here)
        {
            List<int> moves = new List<int>();
            string name = here.name;
            string team = here.team;
            int row = here.row;
            int column = here.column;
            bool isAlive = here.isAlive;
            if (!isAlive) return new List<int>();
            if(name == "Pawn")
            {
                if(team == "White")
                {
                    if (row - 1 >= 0 && buttonList[row - 1][column].Text == "")
                    {
                        moves.Add(row - 1);
                        moves.Add(column);

                    }
                    if(row - 2 >= 0 && row == 6 && buttonList[row - 2][column].Text == "" && buttonList[row-1][column].Text == "")
                    {
                        moves.Add(row - 2);
                        moves.Add(column);
                    }
                    if(row - 1 >= 0 && column - 1 >= 0 && buttonList[row - 1][column-1].Text != "" && buttonList[row - 1][column-1].ForeColor == Color.Black)
                    {
                        moves.Add(row - 1);
                        moves.Add(column - 1);
                    }
                    if(row - 1 >= 0 && column + 1 < 8 && buttonList[row - 1][column+1].Text != "" && buttonList[row - 1][column+1].ForeColor == Color.Black)
                    {
                        moves.Add(row - 1);
                        moves.Add(column + 1);
                    }
                }
                else if(team == "Black")
                {
                    if (row + 1 < 8 && buttonList[row+1][column].Text == "")
                    {
                        //if (row + 1 < 8)
                        //{
                        moves.Add(row + 1);
                        moves.Add(column);
                        //}
                    }
                    if (row + 2 < 8 && row == 1 && buttonList[row + 2][column].Text == "" && buttonList[row+1][column].Text == "")
                    {
                        moves.Add(row + 2);
                        moves.Add(column);
                    }
                    if (column - 1 >= 0 && row + 1 < 8 && buttonList[row + 1][column - 1].Text != "" && buttonList[row + 1][column - 1].ForeColor == Color.Gray)
                    {
                        //if (row + 1 < 8 && column - 1 >= 0)
                        //{
                            moves.Add(row + 1);
                            moves.Add(column - 1);
                        //}
                    }
                    if (row + 1 < 8 && column + 1 < 8 && buttonList[row + 1][column + 1].Text != "" && buttonList[row + 1][column + 1].ForeColor == Color.Gray)
                    {
                        //if (row + 1 < 8 && column + 1 >= 0)
                        //{
                            moves.Add(row + 1);
                            moves.Add(column + 1);
                        //}
                    }
                }
            }
            else if(name == "Bishop")
            {
                int increment = 1;
                //int count = 0;
                while(row+increment < 8 && column+increment < 8)
                {
                    if(buttonList[row+increment][column+increment].Text == "")
                    {
                        moves.Add(row + increment);
                        moves.Add(column + increment);
                        increment++;
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row + increment][column + increment].ForeColor == Color.Gray)
                        {
                            moves.Add(row + increment);
                            moves.Add(column + increment);
                        }
                        else if (team == "White" && buttonList[row + increment][column + increment].ForeColor == Color.Black)
                        {
                            moves.Add(row + increment);
                            moves.Add(column + increment);
                        }
                        break;
                    }
                }
                increment = 1;
                //count = 0;
                while (row + increment < 8 && column - increment >= 0)
                {
                    if (buttonList[row + increment][column - increment].Text == "")
                    {
                        moves.Add(row + increment);
                        moves.Add(column - increment);
                        increment++;
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row + increment][column - increment].ForeColor == Color.Gray)
                        {
                            moves.Add(row + increment);
                            moves.Add(column - increment);
                        }
                        else if (team == "White" && buttonList[row + increment][column - increment].ForeColor == Color.Black)
                        {
                            moves.Add(row + increment);
                            moves.Add(column - increment);
                        }
                        break;
                    }
                }
                increment = 1;
                //count = 0;
                while (row - increment >= 0 && column + increment < 8)
                {
                    if (buttonList[row - increment][column + increment].Text == "")
                    {
                        moves.Add(row - increment);
                        moves.Add(column + increment);
                        increment++;
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row - increment][column + increment].ForeColor == Color.Gray)
                        {
                            moves.Add(row - increment);
                            moves.Add(column + increment);
                        }
                        else if (team == "White" && buttonList[row - increment][column + increment].ForeColor == Color.Black)
                        {
                            moves.Add(row - increment);
                            moves.Add(column + increment);
                        }
                        break;
                    }
                }
                increment = 1;
                //count = 0;
                while (row - increment >= 0 && column - increment >= 0)
                {
                    if (buttonList[row - increment][column - increment].Text == "")
                    {
                        moves.Add(row - increment);
                        moves.Add(column - increment);
                        increment++;
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row - increment][column - increment].ForeColor == Color.Gray)
                        {
                            moves.Add(row - increment);
                            moves.Add(column - increment);
                        }
                        else if (team == "White" && buttonList[row - increment][column - increment].ForeColor == Color.Black)
                        {
                            moves.Add(row - increment);
                            moves.Add(column - increment);
                        }
                        break;
                    }
                }
            }
            else if(name == "Knight")
            {
                if(row + 1 < 8 && column + 2 < 8)
                {
                    if (buttonList[row + 1][column + 2].Text == "")
                    {
                        moves.Add(row + 1);
                        moves.Add(column + 2);
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row + 1][column + 2].ForeColor == Color.Gray)
                        {
                            moves.Add(row + 1);
                            moves.Add(column + 2);
                        }
                        else if (team == "White" && buttonList[row + 1][column + 2].ForeColor == Color.Black)
                        {
                            moves.Add(row + 1);
                            moves.Add(column + 2);
                        }
                    }
                }
                if (row + 1 < 8 && column - 2 >= 0)
                {
                    if (buttonList[row + 1][column - 2].Text == "")
                    {
                        moves.Add(row + 1);
                        moves.Add(column - 2);
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row + 1][column - 2].ForeColor == Color.Gray)
                        {
                            moves.Add(row + 1);
                            moves.Add(column - 2);
                        }
                        else if (team == "White" && buttonList[row + 1][column - 2].ForeColor == Color.Black)
                        {
                            moves.Add(row + 1);
                            moves.Add(column - 2);
                        }
                    }
                }
                if (row + 2 < 8 && column + 1 < 8)
                {
                    if (buttonList[row + 2][column + 1].Text == "")
                    {
                        moves.Add(row + 2);
                        moves.Add(column + 1);
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row + 2][column + 1].ForeColor == Color.Gray)
                        {
                            moves.Add(row + 2);
                            moves.Add(column + 1);
                        }
                        else if (team == "White" && buttonList[row + 2][column + 1].ForeColor == Color.Black)
                        {
                            moves.Add(row + 2);
                            moves.Add(column + 1);
                        }
                    }
                }
                if (row + 2 < 8 && column - 1 >= 0)
                {
                    if (buttonList[row + 2][column - 1].Text == "")
                    {
                        moves.Add(row + 2);
                        moves.Add(column - 1);
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row + 2][column - 1].ForeColor == Color.Gray)
                        {
                            moves.Add(row + 2);
                            moves.Add(column - 1);
                        }
                        else if (team == "White" && buttonList[row + 2][column - 1].ForeColor == Color.Black)
                        {
                            moves.Add(row + 2);
                            moves.Add(column - 1);
                        }
                    }
                }
                if (row-1 >= 0 && column + 2 < 8)
                {
                    if (buttonList[row - 1][column + 2].Text == "")
                    {
                        moves.Add(row - 1);
                        moves.Add(column + 2);
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row - 1][column + 2].ForeColor == Color.Gray)
                        {
                            moves.Add(row - 1);
                            moves.Add(column + 2);
                        }
                        else if (team == "White" && buttonList[row - 1][column + 2].ForeColor == Color.Black)
                        {
                            moves.Add(row - 1);
                            moves.Add(column + 2);
                        }
                    }
                }
                if (row - 1 >= 0 && column - 2 >= 0)
                {
                    if (buttonList[row - 1][column - 2].Text == "")
                    {
                        moves.Add(row - 1);
                        moves.Add(column - 2);
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row - 1][column - 2].ForeColor == Color.Gray)
                        {
                            moves.Add(row - 1);
                            moves.Add(column - 2);
                        }
                        else if (team == "White" && buttonList[row - 1][column - 2].ForeColor == Color.Black)
                        {
                            moves.Add(row - 1);
                            moves.Add(column - 2);
                        }
                    }
                }
                if (row-2 >= 0 && column+1 < 8)
                {
                    if (buttonList[row - 2][column + 1].Text == "")
                    {
                        moves.Add(row - 2);
                        moves.Add(column + 1);
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row - 2][column + 1].ForeColor == Color.Gray)
                        {
                            moves.Add(row - 2);
                            moves.Add(column + 1);
                        }
                        else if (team == "White" && buttonList[row - 2][column + 1].ForeColor == Color.Black)
                        {
                            moves.Add(row - 2);
                            moves.Add(column + 1);
                        }
                    }
                }
                if (row-2 >= 0 && column-1 >= 0)
                {
                    if (buttonList[row - 2][column - 1].Text == "")
                    {
                        moves.Add(row - 2);
                        moves.Add(column - 1);
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row - 2][column - 1].ForeColor == Color.Gray)
                        {
                            moves.Add(row - 2);
                            moves.Add(column - 1);
                        }
                        else if (team == "White" && buttonList[row - 2][column - 1].ForeColor == Color.Black)
                        {
                            moves.Add(row - 2);
                            moves.Add(column - 1);
                        }
                    }
                }
            }
            else if(name == "Rook")
            {
                int increment = 1;
                while(row + increment < 8)
                {
                    if(buttonList[row+increment][column].Text == "")
                    {
                        moves.Add(row + increment);
                        moves.Add(column);
                        increment++;
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row + increment][column].ForeColor == Color.Gray)
                        {
                            moves.Add(row + increment);
                            moves.Add(column);
                        }
                        else if (team == "White" && buttonList[row + increment][column].ForeColor == Color.Black)
                        {
                            moves.Add(row + increment);
                            moves.Add(column);
                        }
                        break;
                    }
                }
                increment = 1;
                while (column - increment >= 0)
                {
                    if (buttonList[row][column - increment].Text == "")
                    {
                        moves.Add(row);
                        moves.Add(column - increment);
                        increment++;
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row][column - increment].ForeColor == Color.Gray)
                        {
                            moves.Add(row);
                            moves.Add(column - increment);
                        }
                        else if (team == "White" && buttonList[row][column - increment].ForeColor == Color.Black)
                        {
                            moves.Add(row);
                            moves.Add(column - increment);
                        }
                        break;
                    }
                }
                increment = 1;
                while (row - increment >= 0)
                {
                    if (buttonList[row - increment][column].Text == "")
                    {
                        moves.Add(row - increment);
                        moves.Add(column);
                        increment++;
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row - increment][column].ForeColor == Color.Gray)
                        {
                            moves.Add(row - increment);
                            moves.Add(column);
                        }
                        else if (team == "White" && buttonList[row - increment][column].ForeColor == Color.Black)
                        {
                            moves.Add(row - increment);
                            moves.Add(column);
                        }
                        break;
                    }
                }
                increment = 1;
                while (column + increment < 8)
                {
                    if (buttonList[row][column + increment].Text == "")
                    {
                        moves.Add(row);
                        moves.Add(column + increment);
                        increment++;
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row][column + increment].ForeColor == Color.Gray)
                        {
                            moves.Add(row);
                            moves.Add(column + increment);
                        }
                        else if (team == "White" && buttonList[row][column + increment].ForeColor == Color.Black)
                        {
                            moves.Add(row);
                            moves.Add(column + increment);
                        }
                        break;
                    }
                }
            }
            else if(name == "Queen")
            {
                int increment = 1;
                while (row + increment < 8)
                {
                    if (buttonList[row + increment][column].Text == "")
                    {
                        moves.Add(row + increment);
                        moves.Add(column);
                        increment++;
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row + increment][column].ForeColor == Color.Gray)
                        {
                            moves.Add(row + increment);
                            moves.Add(column);
                        }
                        else if (team == "White" && buttonList[row + increment][column].ForeColor == Color.Black)
                        {
                            moves.Add(row + increment);
                            moves.Add(column);
                        }
                        break;
                    }
                }
                increment = 1;
                while (column - increment >= 0)
                {
                    if (buttonList[row][column - increment].Text == "")
                    {
                        moves.Add(row);
                        moves.Add(column - increment);
                        increment++;
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row][column - increment].ForeColor == Color.Gray)
                        {
                            moves.Add(row);
                            moves.Add(column - increment);
                        }
                        else if (team == "White" && buttonList[row][column - increment].ForeColor == Color.Black)
                        {
                            moves.Add(row);
                            moves.Add(column - increment);
                        }
                        break;
                    }
                }
                increment = 1;
                while (row - increment >= 0)
                {
                    if (buttonList[row - increment][column].Text == "")
                    {
                        moves.Add(row - increment);
                        moves.Add(column);
                        increment++;
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row - increment][column].ForeColor == Color.Gray)
                        {
                            moves.Add(row - increment);
                            moves.Add(column);
                        }
                        else if (team == "White" && buttonList[row - increment][column].ForeColor == Color.Black)
                        {
                            moves.Add(row - increment);
                            moves.Add(column);
                        }
                        break;
                    }
                }
                increment = 1;
                while (column + increment < 8)
                {
                    if (buttonList[row][column + increment].Text == "")
                    {
                        moves.Add(row);
                        moves.Add(column + increment);
                        increment++;
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row][column + increment].ForeColor == Color.Gray)
                        {
                            moves.Add(row);
                            moves.Add(column + increment);
                        }
                        else if (team == "White" && buttonList[row][column + increment].ForeColor == Color.Black)
                        {
                            moves.Add(row);
                            moves.Add(column + increment);
                        }
                        break;
                    }
                }
                increment = 1;
                while (row + increment < 8 && column + increment < 8)
                {
                    if (buttonList[row + increment][column + increment].Text == "")
                    {
                        //textBox1.Text += (row + increment) + ", " + (column + increment) + " is added \r\n";
                        moves.Add(row + increment);
                        moves.Add(column + increment);
                        increment++;
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row + increment][column + increment].ForeColor == Color.Gray)
                        {
                            moves.Add(row + increment);
                            moves.Add(column + increment);
                        }
                        else if (team == "White" && buttonList[row + increment][column + increment].ForeColor == Color.Black)
                        {
                            moves.Add(row + increment);
                            moves.Add(column + increment);
                        }
                        break;
                    }
                }
                increment = 1;
                while (row + increment < 8 && column - increment >= 0)
                {
                    if (buttonList[row + increment][column - increment].Text == "")
                    {
                        //textBox1.Text += (row + increment) + ", " + (column-increment) + " is added \r\n";
                        moves.Add(row + increment);
                        moves.Add(column - increment);
                        increment++;
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row + increment][column - increment].ForeColor == Color.Gray)
                        {
                            moves.Add(row + increment);
                            moves.Add(column - increment);
                        }
                        else if (team == "White" && buttonList[row + increment][column - increment].ForeColor == Color.Black)
                        {
                            moves.Add(row + increment);
                            moves.Add(column - increment);
                        }
                        break;
                    }
                }
                increment = 1;
                while (row - increment >= 0 && column + increment < 8)
                {
                    if (buttonList[row - increment][column + increment].Text == "")
                    {
                        //textBox1.Text += (row - increment) + ", " + (column + increment) + " is added \r\n";
                        moves.Add(row - increment);
                        moves.Add(column + increment);
                        increment++;
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row - increment][column + increment].ForeColor == Color.Gray)
                        {
                            moves.Add(row - increment);
                            moves.Add(column + increment);
                        }
                        else if (team == "White" && buttonList[row - increment][column + increment].ForeColor == Color.Black)
                        {
                            moves.Add(row - increment);
                            moves.Add(column + increment);
                        }
                        break;
                    }
                }
                increment = 1;
                while (row - increment >= 0 && column - increment >= 0)
                {
                    if (buttonList[row - increment][column - increment].Text == "")
                    {
                        //textBox1.Text += (row - increment) + ", " + (column - increment) + " is added \r\n";
                        moves.Add(row - increment);
                        moves.Add(column - increment);
                        increment++;
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row - increment][column - increment].ForeColor == Color.Gray)
                        {
                            moves.Add(row - increment);
                            moves.Add(column - increment);
                        }
                        else if (team == "White" && buttonList[row - increment][column - increment].ForeColor == Color.Black)
                        {
                            moves.Add(row - increment);
                            moves.Add(column - increment);
                        }
                        break;
                    }
                }
            }
            else if(name == "King")
            {
               // int y = checkCheck();
                if(row + 1 < 8)
                {
                    if (buttonList[row + 1][column].Text == "")
                    {
                        moves.Add(row + 1);
                        moves.Add(column);
                        chessPiece newKing = new chessPiece("King", team, row + 1, column);
                        combinedPieces.Remove(here);
                        combinedPieces.Add(newKing);
                        makeBoard();
                        int y = checkCheck();
                        textBox1.Text += "y = " + y + "\r\n";
                        if(y == 0 && newKing.team == "White")
                        {
                            moves.Remove(row + 1);
                            moves.Remove(column);
                        }
                        else if (y == 1 && newKing.team == "Black")
                        {
                            moves.Remove(row + 1);
                            moves.Remove(column);
                        }
                        combinedPieces.Remove(newKing);
                        combinedPieces.Add(here);
                        makeBoard();
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row + 1][column].ForeColor == Color.Gray)
                        {
                            moves.Add(row + 1);
                            moves.Add(column);
                            chessPiece newKing = new chessPiece("King", team, row + 1, column);
                            combinedPieces.Remove(here);
                            combinedPieces.Add(newKing);
                            makeBoard();
                            int y = checkCheck();
                            textBox1.Text += "y = " + y + "\r\n";
                            if (y == 0 && newKing.team == "White")
                            {
                                moves.Remove(row + 1);
                                moves.Remove(column);
                            }
                            else if (y == 1 && newKing.team == "Black")
                            {
                                moves.Remove(row + 1);
                                moves.Remove(column);
                            }
                            combinedPieces.Remove(newKing);
                            combinedPieces.Add(here);
                            makeBoard();

                        }
                        else if (team == "White" && buttonList[row + 1][column].ForeColor == Color.Black)
                        {
                            moves.Add(row + 1);
                            moves.Add(column);
                            chessPiece newKing = new chessPiece("King", team, row + 1, column);
                            combinedPieces.Remove(here);
                            combinedPieces.Add(newKing);
                            makeBoard();
                            int y = checkCheck();
                            textBox1.Text += "y = " + y + "\r\n";
                            if (y == 0 && newKing.team == "White")
                            {
                                moves.Remove(row + 1);
                                moves.Remove(column);
                            }
                            else if (y == 1 && newKing.team == "Black")
                            {
                                moves.Remove(row + 1);
                                moves.Remove(column);
                            }
                            combinedPieces.Remove(newKing);
                            combinedPieces.Add(here);
                            makeBoard();
                        }
                    }

                }
                if (row + 1 < 8 && column + 1 < 8)
                {
                    if (buttonList[row + 1][column + 1].Text == "")
                    {
                        moves.Add(row + 1);
                        moves.Add(column + 1);
                        chessPiece newKing = new chessPiece("King", team, row + 1, column+1);
                        combinedPieces.Remove(here);
                        combinedPieces.Add(newKing);
                        makeBoard();
                        int y = checkCheck();
                        textBox1.Text += "y = " + y + "\r\n";
                        if (y == 0 && newKing.team == "White")
                        {
                            moves.Remove(row + 1);
                            moves.Remove(column+1);
                        }
                        else if (y == 1 && newKing.team == "Black")
                        {
                            moves.Remove(row + 1);
                            moves.Remove(column+1);
                        }
                        combinedPieces.Remove(newKing);
                        combinedPieces.Add(here);
                        makeBoard();
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row + 1][column + 1].ForeColor == Color.Gray)
                        {
                            moves.Add(row + 1);
                            moves.Add(column + 1);
                            chessPiece newKing = new chessPiece("King", team, row + 1, column + 1);
                            combinedPieces.Remove(here);
                            combinedPieces.Add(newKing);
                            makeBoard();
                            int y = checkCheck();
                            textBox1.Text += "y = " + y + "\r\n";
                            if (y == 0 && newKing.team == "White")
                            {
                                moves.Remove(row + 1);
                                moves.Remove(column + 1);
                            }
                            else if (y == 1 && newKing.team == "Black")
                            {
                                moves.Remove(row + 1);
                                moves.Remove(column + 1);
                            }
                            combinedPieces.Remove(newKing);
                            combinedPieces.Add(here);
                            makeBoard();
                        }
                        else if (team == "White" && buttonList[row + 1][column + 1].ForeColor == Color.Black)
                        {
                            moves.Add(row + 1);
                            moves.Add(column + 1);
                            chessPiece newKing = new chessPiece("King", team, row + 1, column + 1);
                            combinedPieces.Remove(here);
                            combinedPieces.Add(newKing);
                            makeBoard();
                            int y = checkCheck();
                            textBox1.Text += "y = " + y + "\r\n";
                            if (y == 0 && newKing.team == "White")
                            {
                                moves.Remove(row + 1);
                                moves.Remove(column + 1);
                            }
                            else if (y == 1 && newKing.team == "Black")
                            {
                                moves.Remove(row + 1);
                                moves.Remove(column + 1);
                            }
                            combinedPieces.Remove(newKing);
                            combinedPieces.Add(here);
                            makeBoard();
                        }
                    }
                }
                if (row + 1 < 8 && column - 1 >= 0)
                {
                    if (buttonList[row + 1][column - 1].Text == "")
                    {
                        moves.Add(row + 1);
                        moves.Add(column - 1);
                        chessPiece newKing = new chessPiece("King", team, row + 1, column - 1);
                        combinedPieces.Remove(here);
                        combinedPieces.Add(newKing);
                        makeBoard();
                        int y = checkCheck();
                        textBox1.Text += "y = " + y + "\r\n";
                        if (y == 0 && newKing.team == "White")
                        {
                            moves.Remove(row + 1);
                            moves.Remove(column - 1);
                        }
                        else if (y == 1 && newKing.team == "Black")
                        {
                            moves.Remove(row + 1);
                            moves.Remove(column - 1);
                        }
                        combinedPieces.Remove(newKing);
                        combinedPieces.Add(here);
                        makeBoard();
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row + 1][column - 1].ForeColor == Color.Gray)
                        {
                            moves.Add(row + 1);
                            moves.Add(column - 1);
                            chessPiece newKing = new chessPiece("King", team, row + 1, column - 1);
                            combinedPieces.Remove(here);
                            combinedPieces.Add(newKing);
                            makeBoard();
                            int y = checkCheck();
                            textBox1.Text += "y = " + y + "\r\n";
                            if (y == 0 && newKing.team == "White")
                            {
                                moves.Remove(row + 1);
                                moves.Remove(column - 1);
                            }
                            else if (y == 1 && newKing.team == "Black")
                            {
                                moves.Remove(row + 1);
                                moves.Remove(column - 1);
                            }
                            combinedPieces.Remove(newKing);
                            combinedPieces.Add(here);
                            makeBoard();
                        }
                        else if (team == "White" && buttonList[row + 1][column - 1].ForeColor == Color.Black)
                        {
                            moves.Add(row + 1);
                            moves.Add(column - 1);
                            chessPiece newKing = new chessPiece("King", team, row + 1, column - 1);
                            combinedPieces.Remove(here);
                            combinedPieces.Add(newKing);
                            makeBoard();
                            int y = checkCheck();
                            textBox1.Text += "y = " + y + "\r\n";
                            if (y == 0 && newKing.team == "White")
                            {
                                moves.Remove(row + 1);
                                moves.Remove(column - 1);
                            }
                            else if (y == 1 && newKing.team == "Black")
                            {
                                moves.Remove(row + 1);
                                moves.Remove(column - 1);
                            }
                            combinedPieces.Remove(newKing);
                            combinedPieces.Add(here);
                            makeBoard();
                        }
                    }
                }
                if (column+1 < 8)
                {
                    if (buttonList[row][column + 1].Text == "")
                    {
                        moves.Add(row);
                        moves.Add(column + 1);
                        chessPiece newKing = new chessPiece("King", team, row, column + 1);
                        combinedPieces.Remove(here);
                        combinedPieces.Add(newKing);
                        makeBoard();
                        int y = checkCheck();
                        textBox1.Text += "y = " + y + "\r\n";
                        if (y == 0 && newKing.team == "White")
                        {
                            moves.Remove(row);
                            moves.Remove(column + 1);
                        }
                        else if (y == 1 && newKing.team == "Black")
                        {
                            moves.Remove(row);
                            moves.Remove(column + 1);
                        }
                        combinedPieces.Remove(newKing);
                        combinedPieces.Add(here);
                        makeBoard();
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row][column + 1].ForeColor == Color.Gray)
                        {
                            moves.Add(row);
                            moves.Add(column + 1);
                            chessPiece newKing = new chessPiece("King", team, row, column + 1);
                            combinedPieces.Remove(here);
                            combinedPieces.Add(newKing);
                            makeBoard();
                            int y = checkCheck();
                            textBox1.Text += "y = " + y + "\r\n";
                            if (y == 0 && newKing.team == "White")
                            {
                                moves.Remove(row);
                                moves.Remove(column + 1);
                            }
                            else if (y == 1 && newKing.team == "Black")
                            {
                                moves.Remove(row);
                                moves.Remove(column + 1);
                            }
                            combinedPieces.Remove(newKing);
                            combinedPieces.Add(here);
                            makeBoard();
                        }
                        else if (team == "White" && buttonList[row][column + 1].ForeColor == Color.Black)
                        {
                            moves.Add(row);
                            moves.Add(column + 1);
                            chessPiece newKing = new chessPiece("King", team, row, column + 1);
                            combinedPieces.Remove(here);
                            combinedPieces.Add(newKing);
                            makeBoard();
                            int y = checkCheck();
                            textBox1.Text += "y = " + y + "\r\n";
                            if (y == 0 && newKing.team == "White")
                            {
                                moves.Remove(row);
                                moves.Remove(column + 1);
                            }
                            else if (y == 1 && newKing.team == "Black")
                            {
                                moves.Remove(row);
                                moves.Remove(column + 1);
                            }
                            combinedPieces.Remove(newKing);
                            combinedPieces.Add(here);
                            makeBoard();
                        }
                    }
                }
                if (column-1 >= 0)
                {
                    if (buttonList[row][column - 1].Text == "")
                    {
                        moves.Add(row);
                        moves.Add(column - 1);
                        chessPiece newKing = new chessPiece("King", team, row, column - 1);
                        combinedPieces.Remove(here);
                        combinedPieces.Add(newKing);
                        makeBoard();
                        int y = checkCheck();
                        textBox1.Text += "y = " + y + "\r\n";
                        if (y == 0 && newKing.team == "White")
                        {
                            moves.Remove(row);
                            moves.Remove(column - 1);
                        }
                        else if (y == 1 && newKing.team == "Black")
                        {
                            moves.Remove(row);
                            moves.Remove(column - 1);
                        }
                        combinedPieces.Remove(newKing);
                        combinedPieces.Add(here);
                        makeBoard();
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row][column - 1].ForeColor == Color.Gray)
                        {
                            moves.Add(row);
                            moves.Add(column - 1);
                            chessPiece newKing = new chessPiece("King", team, row, column - 1);
                            combinedPieces.Remove(here);
                            combinedPieces.Add(newKing);
                            makeBoard();
                            int y = checkCheck();
                            textBox1.Text += "y = " + y + "\r\n";
                            if (y == 0 && newKing.team == "White")
                            {
                                moves.Remove(row);
                                moves.Remove(column - 1);
                            }
                            else if (y == 1 && newKing.team == "Black")
                            {
                                moves.Remove(row);
                                moves.Remove(column - 1);
                            }
                            combinedPieces.Remove(newKing);
                            combinedPieces.Add(here);
                            makeBoard();
                        }
                        else if (team == "White" && buttonList[row][column - 1].ForeColor == Color.Black)
                        {
                            moves.Add(row);
                            moves.Add(column - 1);
                            chessPiece newKing = new chessPiece("King", team, row, column - 1);
                            combinedPieces.Remove(here);
                            combinedPieces.Add(newKing);
                            makeBoard();
                            int y = checkCheck();
                            textBox1.Text += "y = " + y + "\r\n";
                            if (y == 0 && newKing.team == "White")
                            {
                                moves.Remove(row);
                                moves.Remove(column - 1);
                            }
                            else if (y == 1 && newKing.team == "Black")
                            {
                                moves.Remove(row);
                                moves.Remove(column - 1);
                            }
                            combinedPieces.Remove(newKing);
                            combinedPieces.Add(here);
                            makeBoard();
                        }
                    }
                }
                if (row - 1 >= 0)
                {
                    if (buttonList[row - 1][column].Text == "")
                    {
                        moves.Add(row - 1);
                        moves.Add(column);
                        chessPiece newKing = new chessPiece("King", team, row-1, column);
                        combinedPieces.Remove(here);
                        combinedPieces.Add(newKing);
                        makeBoard();
                        int y = checkCheck();
                        textBox1.Text += "y = " + y + "\r\n";
                        if (y == 0 && newKing.team == "White")
                        {
                            moves.Remove(row-1);
                            moves.Remove(column);
                        }
                        else if (y == 1 && newKing.team == "Black")
                        {
                            moves.Remove(row-1);
                            moves.Remove(column);
                        }
                        combinedPieces.Remove(newKing);
                        combinedPieces.Add(here);
                        makeBoard();
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row - 1][column].ForeColor == Color.Gray)
                        {
                            moves.Add(row - 1);
                            moves.Add(column);
                            chessPiece newKing = new chessPiece("King", team, row - 1, column);
                            combinedPieces.Remove(here);
                            combinedPieces.Add(newKing);
                            makeBoard();
                            int y = checkCheck();
                            textBox1.Text += "y = " + y + "\r\n";
                            if (y == 0 && newKing.team == "White")
                            {
                                moves.Remove(row - 1);
                                moves.Remove(column);
                            }
                            else if (y == 1 && newKing.team == "Black")
                            {
                                moves.Remove(row - 1);
                                moves.Remove(column);
                            }
                            combinedPieces.Remove(newKing);
                            combinedPieces.Add(here);
                            makeBoard();
                        }
                        else if (team == "White" && buttonList[row - 1][column].ForeColor == Color.Black)
                        {
                            moves.Add(row - 1);
                            moves.Add(column);
                            chessPiece newKing = new chessPiece("King", team, row - 1, column);
                            combinedPieces.Remove(here);
                            combinedPieces.Add(newKing);
                            makeBoard();
                            int y = checkCheck();
                            textBox1.Text += "y = " + y + "\r\n";
                            if (y == 0 && newKing.team == "White")
                            {
                                moves.Remove(row - 1);
                                moves.Remove(column);
                            }
                            else if (y == 1 && newKing.team == "Black")
                            {
                                moves.Remove(row - 1);
                                moves.Remove(column);
                            }
                            combinedPieces.Remove(newKing);
                            combinedPieces.Add(here);
                            makeBoard();
                        }
                    }
                }
                if (row - 1 >= 0 && column + 1 < 8)
                {
                    if (buttonList[row - 1][column + 1].Text == "")
                    {
                        moves.Add(row - 1);
                        moves.Add(column + 1);
                        chessPiece newKing = new chessPiece("King", team, row - 1, column+1);
                        combinedPieces.Remove(here);
                        combinedPieces.Add(newKing);
                        makeBoard();
                        int y = checkCheck();
                        textBox1.Text += "y = " + y + "\r\n";
                        if (y == 0 && newKing.team == "White")
                        {
                            moves.Remove(row - 1);
                            moves.Remove(column+1);
                        }
                        else if (y == 1 && newKing.team == "Black")
                        {
                            moves.Remove(row - 1);
                            moves.Remove(column+1);
                        }
                        combinedPieces.Remove(newKing);
                        combinedPieces.Add(here);
                        makeBoard();
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row - 1][column + 1].ForeColor == Color.Gray)
                        {
                            moves.Add(row - 1);
                            moves.Add(column + 1);
                            chessPiece newKing = new chessPiece("King", team, row - 1, column + 1);
                            combinedPieces.Remove(here);
                            combinedPieces.Add(newKing);
                            makeBoard();
                            int y = checkCheck();
                            textBox1.Text += "y = " + y + "\r\n";
                            if (y == 0 && newKing.team == "White")
                            {
                                moves.Remove(row - 1);
                                moves.Remove(column + 1);
                            }
                            else if (y == 1 && newKing.team == "Black")
                            {
                                moves.Remove(row - 1);
                                moves.Remove(column + 1);
                            }
                            combinedPieces.Remove(newKing);
                            combinedPieces.Add(here);
                            makeBoard();
                        }
                        else if (team == "White" && buttonList[row - 1][column + 1].ForeColor == Color.Black)
                        {
                            moves.Add(row - 1);
                            moves.Add(column + 1);
                            chessPiece newKing = new chessPiece("King", team, row - 1, column + 1);
                            combinedPieces.Remove(here);
                            combinedPieces.Add(newKing);
                            makeBoard();
                            int y = checkCheck();
                            textBox1.Text += "y = " + y + "\r\n";
                            if (y == 0 && newKing.team == "White")
                            {
                                moves.Remove(row - 1);
                                moves.Remove(column + 1);
                            }
                            else if (y == 1 && newKing.team == "Black")
                            {
                                moves.Remove(row - 1);
                                moves.Remove(column + 1);
                            }
                            combinedPieces.Remove(newKing);
                            combinedPieces.Add(here);
                            makeBoard();
                        }
                    }
                }
                if (row - 1 >= 0 && column - 1 >= 0)
                {
                    if (buttonList[row - 1][column - 1].Text == "")
                    {
                        moves.Add(row - 1);
                        moves.Add(column - 1);
                        chessPiece newKing = new chessPiece("King", team, row - 1, column - 1);
                        combinedPieces.Remove(here);
                        combinedPieces.Add(newKing);
                        makeBoard();
                        int y = checkCheck();
                        textBox1.Text += "y = " + y + "\r\n";
                        if (y == 0 && newKing.team == "White")
                        {
                            moves.Remove(row - 1);
                            moves.Remove(column - 1);
                        }
                        else if (y == 1 && newKing.team == "Black")
                        {
                            moves.Remove(row - 1);
                            moves.Remove(column - 1);
                        }
                        combinedPieces.Remove(newKing);
                        combinedPieces.Add(here);
                        makeBoard();
                    }
                    else
                    {
                        if (team == "Black" && buttonList[row - 1][column - 1].ForeColor == Color.Gray)
                        {
                            moves.Add(row - 1);
                            moves.Add(column - 1);
                            chessPiece newKing = new chessPiece("King", team, row - 1, column - 1);
                            combinedPieces.Remove(here);
                            combinedPieces.Add(newKing);
                            makeBoard();
                            int y = checkCheck();
                            textBox1.Text += "y = " + y + "\r\n";
                            if (y == 0 && newKing.team == "White")
                            {
                                moves.Remove(row - 1);
                                moves.Remove(column - 1);
                            }
                            else if (y == 1 && newKing.team == "Black")
                            {
                                moves.Remove(row - 1);
                                moves.Remove(column - 1);
                            }
                            combinedPieces.Remove(newKing);
                            combinedPieces.Add(here);
                            makeBoard();
                        }
                        else if (team == "White" && buttonList[row - 1][column - 1].ForeColor == Color.Black)
                        {
                            moves.Add(row - 1);
                            moves.Add(column - 1);
                            chessPiece newKing = new chessPiece("King", team, row - 1, column - 1);
                            combinedPieces.Remove(here);
                            combinedPieces.Add(newKing);
                            makeBoard();
                            int y = checkCheck();
                            textBox1.Text += "y = " + y + "\r\n";
                            if (y == 0 && newKing.team == "White")
                            {
                                moves.Remove(row - 1);
                                moves.Remove(column - 1);
                            }
                            else if (y == 1 && newKing.team == "Black")
                            {
                                moves.Remove(row - 1);
                                moves.Remove(column - 1);
                            }
                            combinedPieces.Remove(newKing);
                            combinedPieces.Add(here);
                            makeBoard();
                        }
                    }
                }
            }
            return moves;
        }
        private void resetBoardColors()
        {
            for(int i = 0; i < buttonList.Count(); i++)
            {
                for(int j = 0; j < buttonList[i].Count(); j++)
                {
                    if(i % 2 == 0)
                    {
                        if(j % 2 == 0)
                        {
                            buttonList[i][j].BackColor = Color.White;
                        }
                        else
                        {
                            buttonList[i][j].BackColor = Color.DarkOrange;
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            buttonList[i][j].BackColor = Color.DarkOrange;
                        }
                        else
                        {
                            buttonList[i][j].BackColor = Color.White;
                        }
                    }
                }
            }
        }
        private void resetBoardPieces()
        {
            chessPiece wp1 = new chessPiece("Pawn", "White", 6, 0);
            chessPiece wp2 = new chessPiece("Pawn", "White", 6, 1);
            chessPiece wp3 = new chessPiece("Pawn", "White", 6, 2);
            chessPiece wp4 = new chessPiece("Pawn", "White", 6, 3);
            chessPiece wp5 = new chessPiece("Pawn", "White", 6, 4);
            chessPiece wp6 = new chessPiece("Pawn", "White", 6, 5);
            chessPiece wp7 = new chessPiece("Pawn", "White", 6, 6);
            chessPiece wp8 = new chessPiece("Pawn", "White", 6, 7);
            chessPiece wr1 = new chessPiece("Rook", "White", 7, 0);
            chessPiece wr2 = new chessPiece("Rook", "White", 7, 7);
            chessPiece wk1 = new chessPiece("Knight", "White", 7, 1);
            chessPiece wk2 = new chessPiece("Knight", "White", 7, 6);
            chessPiece wb1 = new chessPiece("Bishop", "White", 7, 2);
            chessPiece wb2 = new chessPiece("Bishop", "White", 7, 5);
            chessPiece wq = new chessPiece("Queen", "White", 7, 3);
            chessPiece wk = new chessPiece("King", "White", 7, 4);
            whitePieces = new List<chessPiece>() { wp1, wp2, wp3, wp4, wp5, wp6, wp7, wp8, wr1, wr2, wk1, wk2, wb1, wb2, wq, wk };
            chessPiece bp1 = new chessPiece("Pawn", "Black", 1, 0);
            chessPiece bp2 = new chessPiece("Pawn", "Black", 1, 1);
            chessPiece bp3 = new chessPiece("Pawn", "Black", 1, 2);
            chessPiece bp4 = new chessPiece("Pawn", "Black", 1, 3);
            chessPiece bp5 = new chessPiece("Pawn", "Black", 1, 4);
            chessPiece bp6 = new chessPiece("Pawn", "Black", 1, 5);
            chessPiece bp7 = new chessPiece("Pawn", "Black", 1, 6);
            chessPiece bp8 = new chessPiece("Pawn", "Black", 1, 7);
            chessPiece br1 = new chessPiece("Rook", "Black", 0, 0);
            chessPiece br2 = new chessPiece("Rook", "Black", 0, 7);
            chessPiece bk1 = new chessPiece("Knight", "Black", 0, 1);
            chessPiece bk2 = new chessPiece("Knight", "Black", 0, 6);
            chessPiece bb1 = new chessPiece("Bishop", "Black", 0, 2);
            chessPiece bb2 = new chessPiece("Bishop", "Black", 0, 5);
            chessPiece bq = new chessPiece("Queen", "Black", 0, 3);
            chessPiece bk = new chessPiece("King", "Black", 0, 4);
            blackPieces = new List<chessPiece>() { bp1, bp2, bp3, bp4, bp5, bp6, bp7, bp8, br1, br2, bk1, bk2, bb1, bb2, bq, bk };
            combinedPieces = new List<chessPiece>() { wp1, wp2, wp3, wp4, wp5, wp6, wp7, wp8, wr1, wr2, wk1, wk2, wb1, wb2, wq, wk, bp1, bp2, bp3, bp4, bp5, bp6, bp7, bp8, br1, br2, bk1, bk2, bb1, bb2, bq, bk };
        }

        private void tileClick(object sender, EventArgs e)
        {
            //resetBoardColors();
            string s = (string)(sender as Button).Tag;
            int row = s[0] - '0';
            int column = s[1] - '0';
            //chessPiece x = new chessPiece("Temp", "NoTeam", -1, -1);
            /*chessPiece x = new chessPiece("Temp", "NoTeam", -1, -1);
            for (int i = 0; i < combinedPieces.Count(); i++)
            {
                int row2 = combinedPieces[i].row;
                int column2 = combinedPieces[i].column;
                if (row2 == row && column2 == column)
                {
                    x = combinedPieces[i];
                }
            }
            List<int> moves = findAvailableMoves(x);*/
            if (buttonList[row][column].BackColor != Color.DodgerBlue)
            {
                resetBoardColors();
               // textBox1.Text += row + " blue " + column + "\r\n";
                chessPiece x = new chessPiece("Temp", "NoTeam", -1, -1);
                for (int i = 0; i < combinedPieces.Count(); i++)
                {
                    int row2 = combinedPieces[i].row;
                    int column2 = combinedPieces[i].column;
                    if (row2 == row && column2 == column)
                    {
                        if (combinedPieces[i].isAlive)
                        {
                            x = combinedPieces[i];
                           // saveI = i;
                        }
                    }
                }
                /*if (!x.isAlive)
                {
                    saveI = -1;
                    textBox1.Text += " Is Dead " + "\r\n";
                    return;
                }*/
                if(x.team == "White" && turn != 0)
                {
                    return;
                }
                if(x.team == "Black" && turn != 1)
                {
                    return;
                }
                List<int> moves = findAvailableMoves(x);
                for (int i = 0; i < moves.Count(); i += 2)
                {
                    int newRow = moves[i];
                    int newColumn = moves[i + 1];
                    buttonList[newRow][newColumn].BackColor = Color.DodgerBlue;
                }
                clicked = x;
            }
            else
            {
                int savedRow2 = -1;
                int savedColumn2 = -1;
                //List<int> moves = findAvailableMoves(x);
                if(clicked.team != "NoTeam")
                {
                    for (int i = 0; i < combinedPieces.Count(); i++)
                    {
                        int row2 = combinedPieces[i].row;
                        int column2 = combinedPieces[i].column;
                        if (row2 == row && column2 == column)
                        {
                            //newClicked = combinedPieces[i];
                            if(combinedPieces[i].isAlive)
                            {
                                // newClicked = new chessPiece("Temp", "NoTeam", -1, -1);
                                //break;
                                newClicked = combinedPieces[i];
                                saveNewClickedI = i;
                                savedRow2 = row2;
                                savedColumn2 = column2;
                            }
                            else
                            {
                                if(i == combinedPieces.Count()-1)
                                {
                                    newClicked = new chessPiece("Temp", "NoTeam", -1, -1);
                                    break;
                                }
                            }
                           /* saveNewClickedI = i;
                            savedRow2 = row2;
                            savedColumn2 = column2;*/
                        }
                    }

                    //if(clicked.team == "White" && turn != 0) {
                    //    textBox1.Text += "It is not your turn: " + turn + "\r\n";
                    //    return; 
                    //}
                    //else if(clicked.team == "Black" && turn != 1) {
                    //     textBox1.Text += "It is not your turn: " + turn + "\r\n";
                    //     return;  
                    // }

                    textBox1.Text += "Moved " + clicked.name + " of team " + clicked.team + " to " + row + " " + column + "\r\n";
                    textBox1.Text += "Clicked = " + clicked.name + clicked.team + clicked.row + " " + clicked.column + "\r\n";
                    textBox1.Text += "newClicked = " + newClicked.name + newClicked.team + newClicked.row + " " + newClicked.column + "\r\n";

                    if (newClicked.team != "NoTeam" && saveNewClickedI != -1)
                    {
                        textBox1.Text += " Killed " + newClicked.team + " " + newClicked.name + " by " + clicked.team + " " + clicked.name + "\r\n";
                        combinedPieces[saveNewClickedI].kill();
                        newClicked = new chessPiece("Temp", "NoTeam", -1, -1);
                        saveNewClickedI = -1;
                        buttonList[savedRow2][savedColumn2].Text = "";
                    }
                    else
                    {
                       textBox1.Text += "Didnt kill because " + newClicked.team + " and " + saveNewClickedI + "\r\n";
                    }
                    //newClicked = new chessPiece("Temp", "NoTeam", -1, -1);
                    //saveNewClickedI = -1;


                    //textBox1.Text += "We get here" + "\r\n";
                    //combinedPieces[saveI].row += (combinedPieces[saveI].row - row);

                    //int oldRow = combinedPieces[saveI].row;
                    //int oldColumn = combinedPieces[saveI].column;
                    // combinedPieces[saveI].row = row;
                    // combinedPieces[saveI].column = column;
                    int oldRow = clicked.row;
                    int oldColumn = clicked.column;
                    clicked.row = row;
                    clicked.column = column;

                    /*textBox1.Text += "Moved " + combinedPieces[saveI].name + " of team " + combinedPieces[saveI].team + " to " + row + " " + column + "\r\n";  hi
                    textBox1.Text += "Clicked = " + clicked.name + clicked.team + clicked.row + " " + clicked.column + "\r\n";
                    textBox1.Text += "newClicked = " + newClicked.name + newClicked.team + newClicked.row + " " + newClicked.column + "\r\n";                    hi */
                    //combinedPieces[saveI].kill();
                    //saveI = -1;
                    
                    clicked = new chessPiece("Temp", "NoTeam", -1, -1);

                    resetBoardColors();
                    //makeBoard();
                    //buttonList[oldRow][oldColumn].Text = "";

                    turn = turn == 0 ? 1 : 0;
                    makeBoard();
                    buttonList[oldRow][oldColumn].Text = "";
                  /*  int y = checkCheck();
                    if (y == 0)
                    {
                        whiteChecked = true;
                    }
                    else if (y == 1)
                    {
                        blackChecked = true;
                    }
                    else if (y == -1)
                    {
                        whiteChecked = false;
                        blackChecked = false;
                    }*/
                }
                //turn = turn == 0 ? 1 : 0;
            }
            checkForPromo();
        }
        private void checkForPromo()
        {
            for(int i = 0; i < combinedPieces.Count(); i++)
            {
                chessPiece here = combinedPieces[i];
                if(here.name == "Pawn" && here.isAlive)
                {
                    if(here.team == "White")
                    {
                        if(here.row == 0)
                        {
                            promotePiece(here);
                        }
                    }
                    else if(here.team == "Black")
                    {
                        if(here.row == 7)
                        {
                            promotePiece(here);
                        }
                    }
                }
            }
        }
        private void promotePiece(chessPiece here)
        {
            queenPromo.Visible = true;
            rookPromo.Visible = true;
            knightPromo.Visible = true;
            bishopPromo.Visible = true;
            queenPromo.Enabled = true;
            rookPromo.Enabled = true;
            knightPromo.Enabled = true;
            bishopPromo.Enabled = true;
            promotedPiece = here;
        }

        private void promo(object sender, EventArgs e)
        {
            string s = (string)(sender as Button).Tag;
            promotedPiece.name = s;
            makeBoard();
            queenPromo.Visible = false;
            rookPromo.Visible = false;
            knightPromo.Visible = false;
            bishopPromo.Visible = false;
            queenPromo.Enabled = false;
            rookPromo.Enabled = false;
            knightPromo.Enabled = false;
            bishopPromo.Enabled = false;
        }
        private int checkCheck()
        {
            chessPiece whiteK = new chessPiece("Temp", "NoTeam", -1, -1);
            chessPiece blackK = new chessPiece("Temp", "NoTeam", -1, -1);
            for (int i = 0; i < combinedPieces.Count; i++)
            {
                if (combinedPieces[i].name == "King" && combinedPieces[i].team == "White")
                {
                    whiteK = combinedPieces[i];
                }
                else if(combinedPieces[i].name == "King" && combinedPieces[i].team == "Black")
                {
                    blackK = combinedPieces[i];
                }
            }
            textBox1.Text += "White king is at " + whiteK.row + " " + whiteK.column + "\r\n";
            textBox1.Text += "Black king is at " + blackK.row + " " + blackK.column + "\r\n";
            int saveIWhite = -1;
            int saveJWhite = -1;
            int saveIBlack = -1;
            int saveJBlack = -1;
            for(int i = 0; i < buttonList.Count; i++)
            {
                for(int j = 0; j < buttonList[i].Count; j++)
                {
                    if(i == whiteK.row && j == whiteK.column)
                    {
                        saveIWhite = i;
                        saveJWhite = j;
                    }
                    if(i == blackK.row && j == blackK.column)
                    {
                        saveIBlack = i;
                        saveJBlack = j;
                    }
                }
            }
            //textBox1.Text += "White king is at button " + saveIWhite + " " + saveJWhite + "\r\n";
            //textBox1.Text += "Black king is at button " + saveIBlack + " " + saveJBlack + "\r\n";
            for (int j = 0; j < combinedPieces.Count; j++)
            {
                if (combinedPieces[j].name != "King")
                {


                    List<int> moves = findAvailableMoves(combinedPieces[j]);
                    for (int i = 0; i < moves.Count(); i += 2)
                    {
                        int newRow = moves[i];
                        int newColumn = moves[i + 1];
                        buttonList[newRow][newColumn].BackColor = Color.DodgerBlue;
                    }
                    if (saveIWhite != -1 && saveJWhite != -1 && buttonList[saveIWhite][saveJWhite].BackColor == Color.DodgerBlue)
                    {
                        textBox1.Text += " White King is in check from " + combinedPieces[j] + " \r\n";
                        resetBoardColors();
                        return 0;
                    }
                    else if (saveIBlack != -1 && saveJBlack != -1 && buttonList[saveIBlack][saveJBlack].BackColor == Color.DodgerBlue)
                    {
                        textBox1.Text += " Black King is in check from " + combinedPieces[j] + " \r\n";
                        resetBoardColors();
                        return 1;
                    }
                    resetBoardColors();
                }
            }
            return -1;
        }
        private bool stageChange(string name1, string team1, int row1, int column1, string name2, string team2, int row2, int column2)
        {
            chessPiece tempPiece = new chessPiece(name2, team2, row2, column2);
            chessPiece oldPiece = new chessPiece(name1, team1, row1, column1);
            int saveThisI = -1;
            for(int i = 0; i < combinedPieces.Count(); i++)
            {
                if(combinedPieces[i].name == oldPiece.name && combinedPieces[i].team == oldPiece.team && combinedPieces[i].row == oldPiece.row && combinedPieces[i].column == oldPiece.column)
                {
                    oldPiece = combinedPieces[i];
                    saveThisI = i;
                }
            }
            textBox1.Text += "Removing piece " + oldPiece.name + " " + oldPiece.team + " " + oldPiece.row + " " + oldPiece.column + " \r\n";
            combinedPieces.RemoveAt(saveThisI);
            //combinedPieces[saveThisI].kill();
            /*for(int i = 0; i < combinedPieces.Count(); i++)
            {
                textBox1.Text += combinedPieces[i].name + " " + combinedPieces[i].team + " ";
            }
            textBox1.Text += "\r\n\r\n";*/
            combinedPieces.Add(tempPiece);
            makeBoard();
            int y = checkCheck();
            bool temp = true;

            if(team1 == "White" && y == 0)
            {
                temp = false;
            }
            else if(team1 == "White" && y == -1)
            {
                temp = true;
            }
            else if(team1 == "Black" && y == 1)
            {
                temp = false;
            }
            else if(team1 == "Black" && y == -1)
            {
                temp = true;
            }

            combinedPieces.RemoveAt(combinedPieces.Count()-1);
            combinedPieces.Add(oldPiece);
            makeBoard();

            return temp;
        }
    }
}

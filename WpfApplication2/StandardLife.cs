﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Life
{
    /**
    The original rules for Conway's game of life 
    */
    public class StandardLife : LifeMatrix
    {
        private int _size;
        public StandardLife(int inSize)
        {
            this._size = inSize;
            cells = new StandardCell[_size,_size];
            for (int row =0; row < _size; row++)
            {
                for (int column = 0; column < _size; column++)
                {
                    cells[row, column] = new StandardCell();
                }
            }
        }

        public override String Label { get { return "Standard"; } }
        public override int Size
        {
            get
            {
                return _size;
            }
        }

        public override void Randomize()
        {
            Random rnd = new Random();
            for (int row = 0; row < _size; row++)
            {
                for (int column = 0; column < _size; column++)
                {
                    //get a random number that is either 0 or 1
                    GetCell(row, column).IsAlive = rnd.Next(2) == 0;
                    
                }
            }
        }

        public override void Iterate()
        {
            StandardCell[,] newCells = new StandardCell[_size,_size];

            for (int row = 0; row < _size; row++)
            {
                for (int column = 0; column < _size; column++)
                {
                    int living = countLivingSurroundingCells(row, column);
                    //standard conway rules: https://en.wikipedia.org/wiki/Conway's_Game_of_Life
                    if (GetCell(row, column).IsAlive) {
                        if (living == 2 || living == 3)
                        {
                            newCells[row,column] = new StandardCell(true); //alive
                        }
                        else
                        {
                            newCells[row,column] = new StandardCell(); //dead
                        }
                    }
                    else
                    {
                        if (living == 3)
                        {
                            newCells[row, column] = new StandardCell(true); //alive
                        }
                        else
                        {
                            newCells[row, column] = new StandardCell(); //dead
                        }
                    }
                }
            }
            cells = newCells;
                    
        }

        public override void UpdateDisplay(Grid matrix)
        {
            matrix.Children.Clear();



            for (int rowIndex = 0; rowIndex < _size; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < _size; columnIndex++)
                {
                    Button cell = new Button();
                    cell.Background = GetCell(rowIndex, columnIndex).IsAlive ? Brushes.Black : Brushes.White;
                    matrix.Children.Add(cell);
                    Grid.SetRow(cell, rowIndex);
                    Grid.SetColumn(cell, columnIndex);

                }
            }
        }

        private int countLivingSurroundingCells(int row, int column)
        {
            int sum = 0;
            for (int rowIndex = row-1; rowIndex < row + 2; rowIndex++)
            {
                for (int columnIndex = column-1; columnIndex < column+2; columnIndex++)
                {
                    if (isAlive(rowIndex, columnIndex) && (columnIndex != column || rowIndex != row))
                    {
                        sum++;
                    }
                }
            }
            return sum;
        }

        public StandardCell GetCell(int row, int column)
        {
            return (StandardCell)cells[row, column];
        }
        private bool isAlive(int row, int column)
        {
            if (row < 0 || row >= _size || column < 0 || column >= _size)
            {
                return false;
            }
            else
            {
                return GetCell(row, column).IsAlive;
            }
        }
        public override String ToString()
        {
            String result = "";
            for (int rowIndex = 0; rowIndex < _size; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < _size; columnIndex++)
                {
                    result += (GetCell(rowIndex, columnIndex).IsAlive ? "O" : "X") + "\t";
                }
                result += "\n";
            }
            return result;
        }
    }


}

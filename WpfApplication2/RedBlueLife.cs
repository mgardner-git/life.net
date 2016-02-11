using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Life
{
    /**
    This one I made up myself.
    The board consists of dead squares, red living squares and blue living squars. 
    On Each iteration: 
        Any live cell with fewer than two live neighbours dies, as if caused by under population
        Any live cell with 2 or 3 neighbours lives on to the next generation.  It's color becomes the most common color in it's local block of 9 (including itself). On a tie, it remains the same color
        Any live cell with more than 3 live neighbours dies, as if by overpopulation
        Any dead cell with exactly three live neighbours becomes alive, It's color becomes the most common color in it's local block of 9.
    */
    public class RedBlueLife : Life
    {
        
        private int size;
        public RedBlueLife(int inSize)
        {
            this.size = inSize;
            cells = new RedBlueCell[size,size];
            for (int row =0; row < size; row++)
            {
                for (int column = 0; column < size; column++)
                {
                    cells[row, column] = new RedBlueCell();
                }
            }
        }

        public int MatrixSize
        {
            get
            {
                return size;
            }
            set
            {
                this.size = value;
            }
        }

        public override void Randomize()
        {
            Random rnd = new Random();
            for (int row = 0; row < size; row++)
            {
                for (int column = 0; column < size; column++)
                {
                    //get a random number that is from 0 to 3 inclusive
                    int random = rnd.Next(4);
                    RedBlueCell cell = GetCell(row, column);
                    switch (random)
                    {
                        case 0:
                            cell.state = RedBlueState.RED;
                            break;
                        case 1:
                            cell.state = RedBlueState.BLUE;
                            break;
                        default:
                            cell.state = RedBlueState.DEAD;
                            break;
                        
                    }                    
                    
                }
            }
        }

        public override void Iterate()
        {
            RedBlueCell[,] newCells = new RedBlueCell[size,size];

            for (int row = 0; row < size; row++)
            {
                for (int column = 0; column < size; column++)
                {
                    int living = countLivingSurroundingCells(row, column);
                    RedBlueState victoriousState = getVictors(row, column); //this species wins in this area
                    if (isAlive(row, column)) {
                        if (living == 2 || living == 3)
                        {
                            RedBlueState dominantSpecies = getVictors(row, column);
                            newCells[row, column] = new RedBlueCell(dominantSpecies);
                            
                        }
                        else
                        {
                            newCells[row,column] = new RedBlueCell(RedBlueState.DEAD); //dead
                        }
                    }
                    else
                    {
                        if (living == 3)
                        {
                            RedBlueState dominantSpecies = getVictors(row, column);
                            newCells[row, column] = new RedBlueCell(dominantSpecies); //no tie possible here
                        }
                        else
                        {
                            newCells[row, column] = new RedBlueCell(RedBlueState.DEAD);
                        }
                    }
                }
            }
            cells = newCells;
                    
        }

        public override void UpdateDisplay(Grid matrix)
        {
            matrix.Children.Clear();



            for (int rowIndex = 0; rowIndex < size; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < size; columnIndex++)
                {
                    Button square = new Button();
                    RedBlueState state = GetCell(rowIndex, columnIndex).state;
                    switch (state)
                    {
                        case RedBlueState.RED:
                            square.Background = Brushes.Red;
                            break;
                        case RedBlueState.BLUE:
                            square.Background = Brushes.Blue;
                            break;
                        case RedBlueState.DEAD:
                            square.Background = Brushes.White;
                            break;
                    }
                    matrix.Children.Add(square);
                    Grid.SetRow(square, rowIndex);
                    Grid.SetColumn(square, columnIndex);

                }
            }
        }

        private RedBlueState getVictors(int row, int column)
        {            
            int numRed = 0;
            int numBlue = 0;
            for (int rowIndex = row - 1; rowIndex < row + 2; rowIndex++)
            {
                for (int columnIndex = column - 1; columnIndex < column + 2; columnIndex++)
                {
                    if (isAlive(rowIndex, columnIndex))
                    {
                        RedBlueState state = GetCell(rowIndex, columnIndex).state;
                        if (state == RedBlueState.RED)
                        {
                            numRed++;
                        }else if (state == RedBlueState.BLUE)
                        {
                            numBlue++;
                        }
                    }
                }
            }
            if (numRed == numBlue)
            {
                //a tie can only occur when this center cell is alive, in that case it retains it's color
                return GetCell(row, column).state;  
            }
            else if (numRed > numBlue)
            {
                return RedBlueState.RED;
            }
            else
            {
                return RedBlueState.BLUE;
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

        public RedBlueCell GetCell(int row, int column)
        {
            return (RedBlueCell)cells[row, column];
        }
        private bool isAlive(int row, int column)
        {
            if (row < 0 || row >= size || column < 0 || column >= size)
            {
                return false;
            }
            else
            {
                RedBlueCell cell = GetCell(row, column);
                return cell.state == RedBlueState.RED || cell.state == RedBlueState.BLUE;
            }
        }
        public override String ToString()
        {
            String result = "";
            for (int rowIndex = 0; rowIndex < size; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < size; columnIndex++)
                {
                    result += (GetCell(rowIndex, columnIndex).state) + "\t";
                }
                result += "\n";
            }
            return result;
        }
    }


}

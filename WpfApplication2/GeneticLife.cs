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
    I would prefer to use the .net Color class, but it's a struct, so it won't go into the object matrix
    */
    public class ColorCell
    {
        byte _red, _green, _blue;
        bool _isAlive;

        public bool IsAlive
        {
            get { return _isAlive; }
            set { this._isAlive = value; }
        }
        public byte red {
            get { return _red; }
            set { this._red = value; }
        }
        public byte green
        {
            get { return _green; }
            set { _green = value; }
        }
        public byte blue
        {
            get { return _blue; }
            set { _blue = value; }
        }
    }
         /**
    This one I got from a book somewhere.
    The board consists of dead squares, and living squares, each of which is a different random color
    On Each iteration: 
        Any live cell with fewer than two live neighbours dies, as if caused by under population
        Any live cell with 2 or 3 neighbours lives on to the next generation.  It's color remains the same
        Any live cell with more than 3 live neighbours dies, as if by overpopulation
        Any dead cell with exactly three live neighbours becomes alive, It's color becomes the average of the colors of live cells in it's local block of 9
    */
    public class GeneticLife : LifeMatrix
    {
        
        private int _size;
        public GeneticLife(int inSize)
        {
            this._size = inSize;
                      
            cells = new ColorCell[_size,_size];
            for (int row =0; row < _size; row++)
            {
                for (int column = 0; column < _size; column++)
                {
                    cells[row, column] = new ColorCell();
                }
            }
        }

        public override String Label { get { return "Genetic"; } }

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
                    //get a random number that is from 0 to 5 inclusive
                    int random = rnd.Next(6);
                    ColorCell cell = GetCell(row, column);
                    if (random > 2)
                    {
                        cell.IsAlive = false;
                    }
                    else
                    {
                        /*
                        Byte[] randomColorValues = new byte[3];
                        Random randomNumberGenerator = new Random();
                        randomNumberGenerator.NextBytes(randomColorValues); //fill the array with 3 random values within the range allowed for bytes
                                              
                        cell.IsAlive = true;
                        cell.red = randomColorValues[0];
                        cell.green = randomColorValues[1];
                        cell.blue = randomColorValues[2];
                        */
                        cell.IsAlive = true;                        
                        cell.red = (byte)rnd.Next(255); 
                        cell.green = (byte)rnd.Next(255);
                        cell.blue = (byte)rnd.Next(255);

                    }
                }
            }
        }

        public override void Iterate()
        {
            ColorCell[,] newCells = new ColorCell[_size,_size];

            for (int row = 0; row < _size; row++)
            {
                for (int column = 0; column < _size; column++)
                {
                    List<ColorCell> living = getLivingSurroundingCells(row, column);
                   
                    if (isAlive(row, column)) {
                        if (living.Count == 2 || living.Count == 3)
                        {
                            newCells[row, column] = GetCell(row, column);  //lives on with no change   
                        }
                        else
                        {
                            newCells[row, column] = new ColorCell();
                            newCells[row, column].IsAlive = false;
                        }
                    }
                    else
                    {
                        if (living.Count == 3)
                        {
                            ColorCell combine = combineCells(living);
                            newCells[row, column] = combine;
                        }
                        else
                        {
                            newCells[row, column] = new ColorCell();
                            newCells[row, column].IsAlive = false;
                        }
                    }
                }
            }
            cells = newCells;
                    
        }

        private ColorCell combineCells(List<ColorCell> parents)
        {
            //use integer to avoid overflow
            int[] sums = new int[3];
            for (int index=0; index < parents.Count; index++)
            {
                ColorCell check = parents[index];
                sums[0] += check.red;
                sums[1] += check.green;
                sums[2] += check.blue;
            }
            //now convert back down to byte for the average
            byte redAvg = (byte)(sums[0] / parents.Count);  //we loose some precision on the division, but that's ok
            byte greenAvg = (byte)(sums[1] / parents.Count);
            byte blueAvg = (byte)(sums[2] / parents.Count);
            ColorCell result = new ColorCell();
            result.IsAlive = true;
            result.red = redAvg;
            result.green = greenAvg;
            result.blue = blueAvg;
            return result;
        }

        public override void UpdateDisplay(Grid matrix)
        {
            matrix.Children.Clear();

            for (int rowIndex = 0; rowIndex < _size; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < _size; columnIndex++)
                {
                    Button square = new Button();
                    ColorCell cell = GetCell(rowIndex, columnIndex);
                    if (cell.IsAlive)
                    {
                        Color color = Color.FromArgb(255, cell.red, cell.green, cell.blue);                        
                        SolidColorBrush brush = new SolidColorBrush(color);
                        square.Background = brush;                                                
                    }
                    else
                    {
                        square.Background = Brushes.White;
                    }
                    matrix.Children.Add(square);
                    Grid.SetRow(square, rowIndex);
                    Grid.SetColumn(square, columnIndex);

                }
            }
        }

        
        private List<ColorCell> getLivingSurroundingCells(int row, int column)
        {
            List<ColorCell> neighbors = new List<ColorCell>();
            for (int rowIndex = row-1; rowIndex < row + 2; rowIndex++)
            {
                for (int columnIndex = column-1; columnIndex < column+2; columnIndex++)
                {
                    if (isAlive(rowIndex, columnIndex) && (columnIndex != column || rowIndex != row))
                    {
                        neighbors.Add(GetCell(rowIndex, columnIndex));
                    }
                }
            }
            return neighbors;
        }

        public ColorCell GetCell(int row, int column)
        {
            return (ColorCell)cells[row, column];
        }
        private bool isAlive(int row, int column)
        {
            if (row < 0 || row >= _size || column < 0 || column >= _size)
            {
                return false;
            }
            else
            {
                ColorCell cell = GetCell(row, column);
                return cell.IsAlive;
            }
        }
        public override String ToString()
        {
            String result = "";
            for (int rowIndex = 0; rowIndex < _size; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < _size; columnIndex++)
                {
                    ColorCell cell = GetCell(rowIndex, columnIndex);
                    result += "(" + cell.red + "," + cell.green + "," + cell.blue + ")\t";
                }
                result += "\n";
            }
            return result;
        }
    }


}

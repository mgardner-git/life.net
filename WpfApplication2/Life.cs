using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    /**
    This is the Parent class which defines the interface and state that Life and all variants must impement
    */
    public abstract class LifeMatrix
    {
        protected Object[,] cells;

        public abstract void Iterate();
        public abstract void UpdateDisplay(System.Windows.Controls.Grid matrix);
        public abstract void Randomize();
        //used to display a label in the list control in the window
        public abstract String Label { get; }
        public abstract int Size { get; }
        public Object Get(int row, int column)
        {
            return cells[row,column];
        }        

    }

    
}

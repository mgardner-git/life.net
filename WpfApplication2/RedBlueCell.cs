using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    public class RedBlueCell
    {
        private RedBlueState _state;

        public RedBlueCell()
        {
            _state = RedBlueState.DEAD;
        }
        public RedBlueCell(RedBlueState newState)
        {
            _state = newState;
        }
        public RedBlueState state
        {
            get
            {
                return _state;
            }
            set
            {
                this._state = value;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    public class StandardCell
    {
        bool alive;
        public StandardCell()
        {
            alive = false;
        }

        public StandardCell(bool inAlive)
        {
            this.alive = inAlive;
        }

        public bool IsAlive
        {
            get
            {
                return alive;
            }
            set
            {
                alive = value;
            }
        }
    }
}

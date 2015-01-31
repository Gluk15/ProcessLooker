using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProccesLooker 
{
    class Procc
    {
        private int id;
        private string name;
        private DateTime start;
        private DateTime end;

        public Procc(int id, string name, DateTime start)
        {
            this.id = id;
            this.name = name;
            this.start = start;
        }

        public void Update(TimeSpan idle){
            this.end = this.start + idle;
        }

        public override string ToString()
        {
            if (end != null) 
            {
                return String.Format("{0} || {1} || {2} || {3} ", id, name, start, end);
            }
            else
            {
                return String.Format("{0} || {1} || {2} ", id, name, start);
            }
        }

    }
}

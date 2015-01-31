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
        public TimeSpan end;

        public Procc(int id, string name, DateTime start)
        {
            this.id = id;
            this.name = name;
            this.start = start;
            this.end = DateTime.Now.Subtract(start);
        }

        public void UpdateEnd(){
            end = DateTime.Now.Subtract(this.start);
        }

        public override string ToString()
        {   
                return String.Format("{0} || {1} || {2} || {3} ", id, name, start, end);
        }

    }
}

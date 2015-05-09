using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Lurker
{
    class Class1
    { 
        System.Timers.Timer _timer;
        public int currproc = 0;
        public int timeLeft = 0;

        String connStr = @"Data Source =  192.168.1.39;
                            Initial Catalog = BD;
                            Integrated Security = False;
                            UID = sa;
                            PWD = qweasd;";

        void Class1()
        {

        }
    }
}

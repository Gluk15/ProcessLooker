using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProccesLooker
{
    class PCinfo
    {
        private string name;
        private string ip;

        public PCinfo(string name, string ip)
        {
            this.name = name;
            this.ip = ip;
        }

        public override string ToString()
        {

            return String.Format("{0} || {1}", name, ip);
        }
    }
}

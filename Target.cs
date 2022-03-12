using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDdos
{
    internal class Target
    {
        public string IpAddress { get; set; }
        public string Port { get; set; }
        public Method Method { get; set; }            
    }
}

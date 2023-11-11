using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drone_Management_Tools.Models
{
    public class CommParm
    {
        private string _comPort;
        public string ComPort
        {
            get { return _comPort; }
            set { _comPort = value; }
        }

        public int _baudrate;
        public int Baudrate
        {
            get { return _baudrate; }
            set { _baudrate = value; }
        }

        public CommParm()
        {
            this.ComPort = "...";
            this.Baudrate = 9600;
        }
    }
}

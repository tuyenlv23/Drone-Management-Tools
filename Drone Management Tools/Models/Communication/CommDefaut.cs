using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Drone_Management_Tools.Models
{
    public class CommDefaut
    {
        private static uint[] _baudrates = { 600, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 38400, 56000, 57600, 115200, 128000, 256000 };
        private static byte[] _dataBits = { 7, 8, 9 };
        private static double[] _stopBits = { 0.5, 1, 1.5, 2 };
        private static double[] _startStopBits = { 1, 2 };

        public static uint Baudrate { get; set; } = _baudrates[4];
        public static byte DataBit { get; set; } = _dataBits[1];
        public static CommParity Parity { get; set; } = CommParity.None;
        public static double StopBit { get; set; } = _stopBits[1];

        public static uint[] Baudrates
        {
            get
            {
                return _baudrates;
            }
            set
            {
                _baudrates = value;
            }
        }

        public static byte[] DataBits
        {
            get
            {
                return _dataBits;
            }
            set
            {
                _dataBits = value;
            }
        }

        public static double[] StopBits
        {
            get
            {
                return _stopBits;
            }
            set
            {
                _stopBits = value;
            }
        }

        public static double[] StartStopBits
        {
            get
            {
                return _startStopBits;
            }
            set
            {
                _startStopBits = value;
            }
        }

        public static byte ConvertStopBit(double stopBit)
        {
            if (stopBit == _stopBits[0])
            {
                return 0;
            }
            else if (stopBit == _stopBits[1])
            {
                return 1;
            }
            if (stopBit == _stopBits[2])
            {
                return 3;
            }
            if (stopBit == _stopBits[3])
            {
                return 2;
            }

            return 1;
        }

        public static double ConvertStopBit(byte stopBit)
        {
            if (stopBit == 0)
            {
                return _stopBits[0];
            }
            else if (stopBit == 1)
            {
                return _stopBits[1];
            }
            if (stopBit == 2)
            {
                return _stopBits[3];
            }
            if (stopBit == 3)
            {
                return _stopBits[2];
            }

            return 1;
        }
    }

    public enum CommParity
    {
        None,
        Even,
        Odd,        
        Mark,
        Space
    }
}

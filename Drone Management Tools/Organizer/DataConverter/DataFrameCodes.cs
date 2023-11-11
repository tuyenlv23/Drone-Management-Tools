using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Drone_Management_Tools.Organizer
{
    public static class DataFrameCodes
    {
        public const string START_FRAME_CODE = "@";
        public const string END_FRAME_CODE = "#";

        public const string RESPONSE_SUCCESS = "40000A"; //@OK#
        public const string RESPONSE_END = "405C23"; //@\#

        public const string READ_CONFIG = "400023"; //@0#
        public const string SCAN_DEVICE = "400123"; //@1#
        public const string REGISTER_DEVICE = "400223"; //@2#
        public const string SEND_CONFIG = "400323"; //@3#

        public static byte MemoryFormat = 0x10;        

        public static byte[] StartCode = new byte[] { 0x40 };   // ~ @
        public static byte[] EndCode = new byte[] { 0x23 };     // ~ #
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drone_Management_Tools.Organizer
{
    public enum DataResponseStates
    {
        None,
        Start,
        Decoding,
        Response_Success,
        Response_Failed,
        Read_Device_Success,
        Scan_Device_Success,
        Register_Device_Success,
        Send_Device_Success,
        Read_Drone_Success,
        Send_Drone_Success,
        Read_Voltage_Success,
        Send_Voltage_Success,
        Send_MemoryFormat_Success,
        End
    }
}

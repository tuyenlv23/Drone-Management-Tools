using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Drone_Management_Tools.Models
{
    public enum UICmdTypes
    {
        None = -1,
        Read_Device_Config = 0,
        Scan_Device = 1,
        Register_New_Device = 2,
        Send_Device_Config = 3,
        Receive_Device_Config = 4,                
        Send_Drone_Config = 5,
        Read_Drone_Config = 6,
        Read_Voltage_Regulator = 7,
        Send_Voltage_Regulator = 8,
        Change_Device_Id = 9,
        Send_Memory_Format = 10
    }
}

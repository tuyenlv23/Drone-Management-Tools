using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drone_Management_Tools.Models
{
    public enum DeviceCommTypes
    {
        None = -1,
        TCP = 0,
        UART = 1,
        I2C = 2,
        CAN = 3,
        TCP_UART = 4,
        TCP_I2C = 5,
        TCP_CAN = 6,
        UART_CAN = 7,
        UART_I2C = 8,
        I2C_CAN = 9,
    }
}

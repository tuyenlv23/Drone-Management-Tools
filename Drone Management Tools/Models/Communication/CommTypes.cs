using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drone_Management_Tools.Models
{
    public enum CommTypes
    {
        None = -1,
        I2C = 0,
        CAN = 1,
        UART = 2,
        TCP = 3
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Drone_Management_Tools.Organizer;

namespace Drone_Management_Tools.Models
{
    public class DroneTCP
    {
        [JsonProperty(PropertyName = "tcp_1")]
        public Protocol_TCP DroneTCP1 { get; set; }

        public DroneTCP()
        {
            this.DroneTCP1 = new Protocol_TCP();
        }

        public void CreateNull()
        {
            this.DroneTCP1 = null;
        }

        public byte[] Encode(EncodeUtils encodeUtils)
        {
            try
            {
                return this.DroneTCP1.Encode(encodeUtils);
            }
            catch
            {
                return null;
            }
        }

        //--- 6 bytes
        public bool Decode(DecodeUtils decodeUtils, byte[] bytes)
        {
            if (bytes.Length >= 6)
            {
                try
                {
                    return this.DroneTCP1.Decode(decodeUtils, bytes);
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}

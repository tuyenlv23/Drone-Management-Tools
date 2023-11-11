using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drone_Management_Tools.Organizer;
using Newtonsoft.Json;

namespace Drone_Management_Tools.Models
{
    public class DroneCAN
    {
        [JsonProperty(PropertyName = "can_1")]
        public Protocol_CAN DroneCAN1 { get; set; }

        public DroneCAN()
        {
            this.DroneCAN1 = new Protocol_CAN();
        }

        public void CreateNull()
        {
            this.DroneCAN1 = null;
        }

        public byte[] Encode(EncodeUtils encodeUtils)
        {
            try
            {
                return this.DroneCAN1.EncodeDrone(encodeUtils);
            }
            catch
            {
                return null;
            }
        }

        //--- 4 bytes
        public bool Decode(DecodeUtils decodeUtils, byte[] bytes)
        {
            if (bytes.Length >= 4)
            {
                try
                {
                    return this.DroneCAN1.DecodeDrone(decodeUtils, bytes);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drone_Management_Tools.Organizer;
using Newtonsoft.Json;

namespace Drone_Management_Tools.Models
{
    public class DroneI2C
    {
        [JsonProperty(PropertyName = "i2c_1")]
        public Protocol_I2C DroneI2C1 { get; set; }

        [JsonProperty(PropertyName = "i2c_2")]
        public Protocol_I2C DroneI2C2 { get; set; }

        public DroneI2C()
        {
            this.DroneI2C1 = new Protocol_I2C();
            this.DroneI2C2 = new Protocol_I2C();
        }

        public void CreateNull()
        {
            this.DroneI2C1 = null;
            this.DroneI2C2 = null;
        }

        public byte[] Encode(EncodeUtils encodeUtils)
        {
            try
            {
                List<byte> _result = new List<byte>();

                var _i2c1Code = this.DroneI2C1.EncodeDrone(encodeUtils);
                _result.AddRange(_i2c1Code);

                var _i2c2Code = this.DroneI2C2.EncodeDrone(encodeUtils);
                _result.AddRange(_i2c2Code);

                return _result.ToArray();
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
                    var _i2c1Bytes = decodeUtils.GetDataFrame(ref bytes, 2);
                    this.DroneI2C1.DecodeDrone(decodeUtils, _i2c1Bytes);

                    var _i2c2Bytes = decodeUtils.GetDataFrame(ref bytes, 2);
                    this.DroneI2C2.DecodeDrone(decodeUtils, _i2c2Bytes);

                    return true;
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

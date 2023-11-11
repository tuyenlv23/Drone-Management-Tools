using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Drone_Management_Tools.Organizer;

namespace Drone_Management_Tools.Models
{
    public class VoltageRegulator
    {
        [JsonProperty(PropertyName = "factor_a")]
        public float FactorA { get; set; }

        [JsonProperty(PropertyName = "factor_b")]
        public float FactorB { get; set;}

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        public VoltageRegulator()
        {
            this.FactorA = 8.253f;
            this.FactorB = -0.9613f;
            this.Description = string.Empty;
        }        

        public byte[] Encode(EncodeUtils encodeUtils)
        {
            try
            {
                List<byte> _result = new List<byte>();

                var _factorACode = encodeUtils.ConvertFloatToHex(this.FactorA);
                _result.AddRange(_factorACode);

                var _factorBCode = encodeUtils.ConvertFloatToHex(this.FactorB);
                _result.AddRange(_factorBCode);

                return _result.ToArray();
            }
            catch
            {
                return null;
            }
        }

        //--- 8 bytes
        public bool Decode(DecodeUtils decodeUtils, ref byte[] bytes)
        {
            if (bytes.Length >= 8)
            {
                try
                {
                    var _factorABytes = decodeUtils.GetDataFrame(ref bytes, 4);
                    this.FactorA = decodeUtils.ConvertHexToFloat(_factorABytes);

                    var _factorBBytes = decodeUtils.GetDataFrame(ref bytes, 4);
                    this.FactorB = decodeUtils.ConvertHexToFloat(_factorBBytes);

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

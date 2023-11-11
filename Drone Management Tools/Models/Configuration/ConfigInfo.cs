using System.ComponentModel;
using Newtonsoft.Json;

namespace Drone_Management_Tools.Models
{
    public class ConfigInfo
    {
        [JsonProperty(PropertyName = "name")]
        [ReadOnly(true)]
        [DisplayName("Name")]
        [Description("Configuration Name")]              
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        [ReadOnly(true)]
        [DisplayName("Description")]
        [Description("Configuration Description")]        
        public string Description { get; set; }

        [JsonProperty(PropertyName = "version")]
        [ReadOnly(true)]
        [DisplayName("Version")]
        [Description("Configuration Version")]        
        public byte Version { get; set; }

        public ConfigInfo()
        {
            this.Name = "Drone_Template";
            this.Description = "Drone_Template";
            this.Version = 0;
        }

        public ConfigInfo Clone()
        {
            return (ConfigInfo)this.MemberwiseClone();
        }
    }
}

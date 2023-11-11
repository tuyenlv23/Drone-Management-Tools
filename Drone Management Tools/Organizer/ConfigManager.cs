using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.Utilities;
using Drone_Management_Tools.Models.Drone;

namespace Drone_Management_Tools.Organizer
{
    public class ConfigManager
    {
        private ConfigModel _model;
        public ConfigModel Model
        {
            get { return _model; }
            set { _model = value; }
        }

        private DroneConfig _droneConfig;
        public DroneConfig DroneConfig
        {
            get { return _droneConfig; }
            set { _droneConfig = value; }
        }

        private VoltageConfig _voltageConfig;
        public VoltageConfig VoltageConfig
        {
            get { return _voltageConfig; }
            set { _voltageConfig = value; }
        }

        private List<byte> _modelIds;
        public List<byte> ModelIds
        {
            get { return _modelIds; }
            set { _modelIds = value; }
        }

        private Dictionary<byte, ConfigModel> _models;
        public Dictionary<byte, ConfigModel> Models
        {
            get { return _models; }
            set { _models = value; }
        }

        public ConfigManager()
        { 
            this._model = new ConfigModel();
            this._droneConfig = new DroneConfig();
            this._voltageConfig = new VoltageConfig();
            this._modelIds = new List<byte>();
            this._models = new Dictionary<byte, ConfigModel>();
        }

        public ConfigModel GetConfig(string configPath)
        {
            try
            {
                ConfigModel _result = new ConfigModel();
                _result.Devices.Clear();
                using (StreamReader sr = File.OpenText(configPath))
                {
                    var obj = sr.ReadToEnd();
                    _result = JsonConvert.DeserializeObject<ConfigModel>(obj);
                }

                if (_result.Devices.Count > 0)
                {
                    foreach (var device in _result.Devices)
                    {
                        if (device.Communications.Count > 0)
                        {
                            foreach (var devComm in device.Communications)
                            {
                                var commStr = devComm.Parameters.ToString();
                                switch (devComm.Type)
                                {
                                    case CommTypes.TCP:
                                        {
                                            devComm.Parameters = JsonConvert.DeserializeObject<Protocol_TCP>(commStr);
                                            break;
                                        }
                                    case CommTypes.UART:
                                        {
                                            devComm.Parameters = JsonConvert.DeserializeObject<Protocol_UART>(commStr);
                                            break;
                                        }
                                    case CommTypes.I2C:
                                        {
                                            devComm.Parameters = JsonConvert.DeserializeObject<Protocol_I2C>(commStr);
                                            break;
                                        }
                                    case CommTypes.CAN:
                                        {
                                            devComm.Parameters = JsonConvert.DeserializeObject<Protocol_CAN>(commStr);
                                            break;
                                        }
                                }
                            }
                        }
                    }
                }

                _result.Devices = _result.Devices.OrderBy(x => x.Id).ToList();
                return _result;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);                
                return null;
            }
        }

        public DroneConfig GetDroneConfig(string configPath)
        {
            try
            {
                DroneConfig _result = new DroneConfig();
                using (StreamReader sr = File.OpenText(configPath))
                {
                    var obj = sr.ReadToEnd();
                    _result = JsonConvert.DeserializeObject<DroneConfig>(obj);
                }

                this._droneConfig = _result;
                return _result;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return null;
            }
        }

        public VoltageConfig GetVoltageConfig(string configPath)
        {
            try
            {
                VoltageConfig _result = new VoltageConfig();
                using (StreamReader sr = File.OpenText(configPath))
                {
                    var obj = sr.ReadToEnd();
                    _result = JsonConvert.DeserializeObject<VoltageConfig>(obj);
                }

                this._voltageConfig = _result;
                return _result;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return null;
            }
        }

        public void GetModels(string configDir)
        {
            this._models.Clear();
            var _configPaths = HelperUtils.GetFileFromFolder(configDir);

            if (_configPaths.Count > 0)
            {
                foreach (var _configPath in _configPaths)
                {
                    var _model = GetConfig(_configPath);
                    if (_model != null)
                    {
                        if (!this._models.ContainsKey(_model.ConfigInfo.Version))
                            this._models.Add(_model.ConfigInfo.Version, _model);
                    }
                }

                if (this._models.Count > 0)
                {
                    this._models = this._models.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                    this._modelIds.Clear();
                    this._modelIds = this.Models.Keys.ToList();
                }
            }
        }

        public void UpdateModel(ConfigModel model)
        {
            if (!this._models.ContainsKey(model.ConfigInfo.Version))
                this._models.Add(_model.ConfigInfo.Version, model);
            else
                this._models[_model.ConfigInfo.Version] = model;

            if (this._models.Count > 0)
            {
                this._modelIds.Clear();
                this._modelIds = this.Models.Keys.ToList();
            }
        }        
    }
}

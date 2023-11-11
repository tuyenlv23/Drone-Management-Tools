using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.UIDesign;
using Drone_Management_Tools.Utilities;

namespace Drone_Management_Tools.Organizer
{
    public class DataServerProcess
    {        
        private ConfigManager _configManager;
        public ConfigManager ConfigManager
        {
            get { return _configManager; }
            set { _configManager = value; }
        }

        private DeviceManager _deviceManager;
        public DeviceManager DeviceManager
        {
            get { return _deviceManager; }
            set { _deviceManager = value; }
        }

        private ConnectionManager _connectionManager;
        public ConnectionManager ConnectionManager
        {
            get { return _connectionManager; }
            set { _connectionManager = value; }
        }

        private DataFrameConverter _dataFrameConverter;
        public DataFrameConverter DataFrameConverter
        {
            get { return _dataFrameConverter; }
            set { _dataFrameConverter = value; }
        }

        public UIMain _uiMain;
        private ManageDeviceForm _manageDeviceForm;
        private ManageDroneForm _manageDroneForm;
        private ScanDeviceForm _scanDeviceForm;        
        private TreeView _treeView;

        private byte _oldId = 0;
        private bool _isChangeId;
        public bool IsChangeId => _isChangeId;
        private Dictionary<string, byte[]> _datas = new Dictionary<string, byte[]>();

        private DataResponseStates _dataResponseState;
        public DataResponseStates DataResponseState => _dataResponseState;

        //private bool _isOverTime;
        System.Timers.Timer _timerWaitResponse;

        public DataServerProcess(UIMain uiMain)
        {
            this._uiMain = uiMain;

            this._configManager = new ConfigManager();
            this._deviceManager = new DeviceManager();
            this._dataFrameConverter = new DataFrameConverter();

            this._timerWaitResponse = new System.Timers.Timer();
            this._timerWaitResponse.Interval = 3000;
            this._timerWaitResponse.Elapsed += TimerWaitResponse_Elapsed;

            InitSerialConnection();
        }        

        public void StartProcess()
        {
            if (CheckDataPath())
            {
                LoadDeviceLibrary();
                LoadConfigLibrary();
                InitConfig();
                //ShowDroneSide();
            }
        }        

        private bool CheckDataPath()
        {
            try
            {
                //--- Create Device Library Path
                if (!File.Exists(PATH_MANAGER.deviceLibraryPath))
                {
                    if (HelperUtils.CreateFilePath(PATH_MANAGER.deviceLibraryPath))
                        HelperUtils.SaveJsonFile(this._deviceManager.DeviceLibrary, PATH_MANAGER.deviceLibraryPath);
                }

                //--- Create Drone Config Path
                if (!File.Exists(PATH_MANAGER.droneConfigPath))
                {
                    if (HelperUtils.CreateFilePath(PATH_MANAGER.droneConfigPath))
                        HelperUtils.SaveJsonFile(this._configManager.DroneConfig, PATH_MANAGER.droneConfigPath);
                }

                //--- Create Voltage Config Path
                if (!File.Exists(PATH_MANAGER.voltageConfigPath))
                {
                    if (HelperUtils.CreateFilePath(PATH_MANAGER.voltageConfigPath))
                        HelperUtils.SaveJsonFile(this._configManager.VoltageConfig, PATH_MANAGER.voltageConfigPath);
                }

                //--- Create Device Configs Directory
                if (!Directory.Exists(PATH_MANAGER.deviceConfigDir))
                    HelperUtils.CreateDirectory(PATH_MANAGER.deviceConfigDir);

                return true;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return false;
            }
        }

        private void LoadDeviceLibrary()
        {
            this._deviceManager.GetDeviceLibrary(PATH_MANAGER.deviceLibraryPath);
            this._deviceManager.DeviceLibrary.VirtualLibraryInfo = this._deviceManager.DeviceLibrary.LibraryInfo.Clone();
        }

        private void LoadConfigLibrary()
        {
            this._configManager.GetModels(PATH_MANAGER.deviceConfigDir);
            this._configManager.GetDroneConfig(PATH_MANAGER.droneConfigPath);
            this._configManager.GetVoltageConfig(PATH_MANAGER.voltageConfigPath);
        }

        private void InitConfig()
        {
            var _configFiles = HelperUtils.GetFileFromFolder(PATH_MANAGER.activeConfigDir);
            if (_configFiles.Count == 1)
            {
                OpenConfig(_configFiles[0]);
                BuildTreeNodeDevices(this._uiMain.trvRootDeviceConfig, this._configManager.Model, true);
            }
            else
            {
                AddNewConfig();
                BuildTreeNodeDevices(this._uiMain.trvRootDeviceConfig, this._configManager.Model, true);
            }
        }

        private void ShowDroneSide()
        {
            this._uiMain.ShowDroneSide(this._configManager);
        }

        #region Serial Communication
        private void InitSerialConnection()
        {
            if (this._connectionManager == null)
            {
                this._connectionManager = new ConnectionManager();
                this._connectionManager.EventWhenOpened += ConnectionManager_EventWhenOpened;
                this._connectionManager.EventWhenClosed += ConnectionManager_EventWhenClosed;
                this._connectionManager.EventWhenDataReceived += ConnectionManager_EventWhenDataReceived;
            }
        }

        private void StartTimerResponse()
        {
            this._timerWaitResponse.Start();
        }

        private void StopTimerResponse()
        {
            this._timerWaitResponse.Stop();
        }

        private void TimerWaitResponse_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            StopTimerResponse();
            switch (this._dataFrameConverter.DecodeUtils.UiCmdType)
            {
                case UICmdTypes.Read_Device_Config:
                    {
                        if (this._dataResponseState == DataResponseStates.None)
                        {
                            var _msg = "Read config: Data is not responding";
                            LogUtils.AddLog(_msg, LogTypes.Info);
                            MessageBox.Show(_msg, "Read Config", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        break;
                    }
                case UICmdTypes.Scan_Device:
                    {
                        if (this._dataResponseState == DataResponseStates.None)
                        {
                            var _msg = "Scan device: Data is not responding";
                            LogUtils.AddLog(_msg, LogTypes.Info);
                            MessageBox.Show(_msg, "Scan Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        break;
                    }
                case UICmdTypes.Register_New_Device:
                    {
                        if (this._dataResponseState == DataResponseStates.None)
                        {
                            var _msg = "Add new device: Data is not responding";
                            LogUtils.AddLog(_msg, LogTypes.Info);
                            MessageBox.Show(_msg, "Add New Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        break;
                    }                
                case UICmdTypes.Send_Device_Config:
                    {
                        if (this._dataResponseState == DataResponseStates.None)
                        {
                            var _msg = "Send config: Data is not responding";
                            LogUtils.AddLog(_msg, LogTypes.Info);
                            MessageBox.Show(_msg, "Send Config", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        break;
                    }
                case UICmdTypes.Read_Drone_Config:
                    {
                        if (this._dataResponseState == DataResponseStates.None)
                        {
                            var _msg = "Read drone config: Data is not responding";
                            LogUtils.AddLog(_msg, LogTypes.Info);
                            MessageBox.Show(_msg, "Read Drone Config", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        break;
                    }
                case UICmdTypes.Send_Drone_Config:
                    {
                        if (this._dataResponseState == DataResponseStates.None)
                        {
                            var _msg = "Send drone config: Data is not responding";
                            LogUtils.AddLog(_msg, LogTypes.Info);
                            MessageBox.Show(_msg, "Send Drone Config", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        break;
                    }
                case UICmdTypes.Read_Voltage_Regulator:
                    {
                        if (this._dataResponseState == DataResponseStates.None)
                        {
                            var _msg = "Read voltage regulator: Data is not responding";
                            LogUtils.AddLog(_msg, LogTypes.Info);
                            MessageBox.Show(_msg, "Read Voltage Regulator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        break;
                    }
                case UICmdTypes.Send_Voltage_Regulator:
                    {
                        if (this._dataResponseState == DataResponseStates.None)
                        {
                            var _msg = "Send voltage regulator: Data is not responding";
                            LogUtils.AddLog(_msg, LogTypes.Info);
                            MessageBox.Show(_msg, "Send Voltage Regulator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        break;
                    }
                case UICmdTypes.Send_Memory_Format:
                    {
                        if (this._dataResponseState == DataResponseStates.None)
                        {
                            var _msg = "Send memory format: Data is not responding";
                            LogUtils.AddLog(_msg, LogTypes.Info);
                            MessageBox.Show(_msg, "Send Memory Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        break;
                    }
            }

            this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);
        }

        public void ConnectMCU()
        {
            UartConnectionForm _form = new UartConnectionForm(this._connectionManager);
            _form.ShowDialog();
        }

        public void DisconnectMCU()
        {
            if (this._connectionManager != null)
                this._connectionManager.DisconnectSerial();
        }

        private void ConnectionManager_EventWhenOpened(object sender, string e)
        {
            this._uiMain.ShowCommunicationStatus(true);
        }

        private void ConnectionManager_EventWhenClosed(object sender, string e)
        {
            this._uiMain.ShowCommunicationStatus(false);
        }

        private void ConnectionManager_EventWhenDataReceived(object sender, byte[] e)
        {
            DecodeDataReceived(e);
        }

        private void DecodeDataReceived(byte[] e)
        {
            try
            {
                this._dataResponseState = DataResponseStates.Decoding;
#if DEBUG
                LogUtils.AddLog($"Data received: {BitConverter.ToString(e)}", LogTypes.Info);
#endif
                switch (this._dataFrameConverter.DecodeUtils.UiCmdType)
                {
                    case UICmdTypes.Read_Device_Config:
                        {
                            DecodeReadDeviceConfig(e);
                            break;
                        }
                    case UICmdTypes.Scan_Device:
                        {
                            DecodeScanDevice(e);
                            break;
                        }
                    case UICmdTypes.Register_New_Device:
                        {
                            DecodeRegisterDevice(e);
                            break;
                        }
                    case UICmdTypes.Change_Device_Id:
                        {
                            DecodeChangeDeviceId(e);
                            break;
                        }
                    case UICmdTypes.Send_Device_Config:
                        {
                            DecodeSendDeviceConfig(e);
                            break;
                        }
                    case UICmdTypes.Read_Drone_Config:
                        {
                            DecodeReadDroneConfig(e);
                            break;
                        }
                    case UICmdTypes.Send_Drone_Config:
                        {
                            DecodeSendDroneConfig(e);
                            break;
                        }
                    case UICmdTypes.Read_Voltage_Regulator:
                        {
                            DecodeReadVoltageRegulator(e);
                            break;
                        }
                    case UICmdTypes.Send_Voltage_Regulator:
                        {
                            DecodeSendVoltageRegulator(e);
                            break;
                        }
                    case UICmdTypes.Send_Memory_Format:
                        {
                            DecodeSendMemoryFormat(e);
                            break;
                        }
                    default:
                        {
                            DecodeUARTData(e);
                            break;
                        }
                }
            }
            catch
            {
                this._dataResponseState = DataResponseStates.None;
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);
            }
        }

        private void DecodeReadDeviceConfig(byte[] e)
        {
            try
            {

                this._dataFrameConverter.DecodeUtils.DecodeDeviceConfig(e);
                this._dataResponseState = DataResponseStates.Read_Device_Success;
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);
                if (this._dataFrameConverter.DecodeUtils.ConfigModel.Devices.Count > 0)
                {
                    var _msg = "Read device config success";
                    LogUtils.AddLog(_msg, LogTypes.Info);
                    MessageBox.Show(_msg, "Read Config", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this._uiMain.ShowDeviceResult(this._dataFrameConverter.DecodeUtils.ConfigModel, this._configManager);                    
                }
                else
                {
                    var _msg = "Read config: The config is null or wrong format";
                    LogUtils.AddLog(_msg, LogTypes.Info);
                    MessageBox.Show(_msg, "Read Config", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                this._dataResponseState = DataResponseStates.Response_Failed;
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);
            }
        }

        private void DecodeScanDevice(byte[] e)
        {
            if (this._dataFrameConverter.DecodeUtils.DecodeScanData(e, this._deviceManager))
            {
                this._dataResponseState = DataResponseStates.Scan_Device_Success;
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);

                if (this._deviceManager.DeviceScan != null)
                {
                    if (this._deviceManager.DeviceScan.Devices[0] != null)
                    {
                        var _id1 = this._deviceManager.DeviceScan.Devices[0].Id;
                        LogUtils.AddLog($"Device [id: {_id1}] is scanned", LogTypes.Info);
                    }

                    if (this._deviceManager.DeviceScan.Devices[1] != null)
                    {
                        var _id2 = this._deviceManager.DeviceScan.Devices[1].Id;
                        LogUtils.AddLog($"Device [id: {_id2}] is scanned", LogTypes.Info);
                    }
                }               

                BuildTreeNodeDeviceScan(this._treeView);
            }
            else
            {
                this._dataResponseState = DataResponseStates.Response_Failed;
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);
                var _msg = "Scan device failed!\n Not device is scanned";
                LogUtils.AddLog(_msg, LogTypes.Info);
                MessageBox.Show(_msg, "Scan Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this._uiMain.ShowScanDevice(this._deviceManager);
            }
        }        

        private void DecodeRegisterDevice(byte[] e)
        {
            var _reponseState = this._dataFrameConverter.DecodeUtils.ValidateResponseState(ref e, 3);
            if (_reponseState == DataResponseStates.Response_Success)
            {
                this._dataResponseState = DataResponseStates.Register_Device_Success;
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);

                this._deviceManager.DeviceLibrary.UpdateDevice(this._deviceManager.NewDevice);
                LogUtils.AddLog($"Device [name: {this._deviceManager.NewDevice.Name} | id: {this._deviceManager.NewDevice.Id}] has been registered", LogTypes.Info);

                if (this._treeView.InvokeRequired)
                {
                    this._treeView.Invoke(new Action(() =>
                    {
                        RebuildTreeNodeDevices(this._treeView, this._deviceManager.NewDevice, ActionModes.Add_Device);
                    }));
                }
                else
                    RebuildTreeNodeDevices(this._treeView, this._deviceManager.NewDevice, ActionModes.Add_Device);
            }
            else
            {
                this._dataResponseState = DataResponseStates.Response_Failed;
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);

                var _msg = "Add new device failed";
                LogUtils.AddLog(_msg, LogTypes.Info);
                MessageBox.Show(_msg, "Add New Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DecodeChangeDeviceId(byte[] e)
        {
            var _reponseState = this._dataFrameConverter.DecodeUtils.ValidateResponseState(ref e, 3);
            if (_reponseState == DataResponseStates.Response_Success)
            {
                this._dataResponseState = DataResponseStates.Register_Device_Success;
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);

                //--- Update Library
                if (this._deviceManager.DeviceLibrary.DeviceIds.Contains(this._oldId))
                    this._deviceManager.DeviceLibrary.RemoveDevice(this._oldId);

                if (this._deviceManager.DeviceLibrary.DeviceIds.Contains(this._deviceManager.NewDevice.Id))
                    this._deviceManager.DeviceLibrary.RemoveDevice(this._deviceManager.NewDevice.Id);

                this._deviceManager.DeviceLibrary.UpdateDevice(this._deviceManager.NewDevice);
                this._deviceManager.DeviceLibrary.Save();

                //--- Update Scan list
                if (this._deviceManager.DeviceScan.DeviceIds.Contains(this._oldId))
                    this._deviceManager.DeviceScan.RemoveDevice(this._oldId);

                if (this._deviceManager.DeviceScan.DeviceIds.Contains(this._deviceManager.NewDevice.Id))
                    this._deviceManager.DeviceScan.RemoveDevice(this._deviceManager.NewDevice.Id);

                this._deviceManager.DeviceScan.UpdateDevice(this._deviceManager.NewDevice);
                this._deviceManager.DeviceScan.Save();

                if (this._scanDeviceForm != null)
                {
                    if (this._treeView.InvokeRequired)
                    {
                        this._treeView.Invoke(new Action(() =>
                        {
                            BuildTreeNodeDeviceScan(this._treeView);
                            this._scanDeviceForm.RefreshDeviceScan();
                            this._scanDeviceForm.RefreshLibrary();
                            this._scanDeviceForm = null;
                        }));
                    }
                    else
                    {
                        BuildTreeNodeDeviceScan(this._treeView);
                        this._scanDeviceForm.RefreshDeviceScan();
                        this._scanDeviceForm.RefreshLibrary();
                        this._scanDeviceForm = null;
                    }               
                }

                if (this._manageDeviceForm != null)
                {
                    if (this._treeView.InvokeRequired)
                    {
                        this._treeView.Invoke(new Action(() =>
                        {
                            RebuildTreeNodeDevices(this._treeView, this._deviceManager.NewDevice, ActionModes.Edit_Device);
                            this._manageDeviceForm.RefreshDeviceLibrary();
                            this._manageDeviceForm = null;
                        }));
                    }
                    else
                    {
                        RebuildTreeNodeDevices(this._treeView, this._deviceManager.NewDevice, ActionModes.Edit_Device);
                        this._manageDeviceForm.RefreshDeviceLibrary();
                        this._manageDeviceForm = null;
                    }                    
                }

                LogUtils.AddLog($"Device [name: {this._deviceManager.NewDevice.Name}] has been edited from [id: {this._oldId}] to [id: {this._deviceManager.NewDevice.Id}]", LogTypes.Info);
            }
            else
            {
                this._dataResponseState = DataResponseStates.Response_Failed;
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);

                var _msg = $"Edit device [id: {this._oldId}] failed";
                LogUtils.AddLog(_msg, LogTypes.Info);
                MessageBox.Show(_msg, "Edit Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DecodeSendDeviceConfig(byte[] e)
        {
            var _reponseState = this._dataFrameConverter.DecodeUtils.ValidateResponseState(ref e, 3);
            if (_reponseState == DataResponseStates.Response_Success)
            {
                StopTimerResponse();
                this._dataResponseState = DataResponseStates.Send_Device_Success;
                LogUtils.AddLog($"Send device {this._datas.First().Key} success", LogTypes.Info);
                this._datas.Remove(this._datas.First().Key);                
                if (this._datas.Count > 0)
                {
                    this._connectionManager.WriteData(this._datas.First().Value);
                    StartTimerResponse();
                }
                else
                    this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);
            }
            else
            {
                this._dataResponseState = DataResponseStates.Response_Failed;
                var _msg = "Send config failed";
                LogUtils.AddLog(_msg, LogTypes.Info);
                MessageBox.Show(_msg, "Send Config", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DecodeReadDroneConfig(byte[] e)
        {
            try
            {
                this._dataFrameConverter.DecodeUtils.DecodeDroneConfig(e);
                this._dataResponseState = DataResponseStates.Read_Drone_Success;
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);
                if (this._dataFrameConverter.DecodeUtils.DroneModel != null)
                {
                    if (this._manageDroneForm != null)
                    {
                        this._manageDroneForm.ShowParameters(this._dataFrameConverter.DecodeUtils.DroneModel);
                        this._manageDroneForm = null;
                        var _msg = "Read drone config success";
                        LogUtils.AddLog(_msg, LogTypes.Info);
                        MessageBox.Show(_msg, "Read Drone Config", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    var _msg = "Read drone config: The drone config is null or wrong format";
                    LogUtils.AddLog(_msg, LogTypes.Info);
                    MessageBox.Show(_msg, "Read Drone Config", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                this._dataResponseState = DataResponseStates.Response_Failed;
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);
            }
        }

        private void DecodeSendDroneConfig(byte[] e)
        {
            var _reponseState = this._dataFrameConverter.DecodeUtils.ValidateResponseState(ref e, 3);
            if (_reponseState == DataResponseStates.Response_Success)
            {
                StopTimerResponse();
                this._dataResponseState = DataResponseStates.Send_Drone_Success;
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);
                var _msg = "Send drone config success";
                LogUtils.AddLog(_msg, LogTypes.Info);
                MessageBox.Show(_msg, "Send Drone Config", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this._dataResponseState = DataResponseStates.Response_Failed;
                var _msg = "Send drone config failed";
                LogUtils.AddLog(_msg, LogTypes.Info);
                MessageBox.Show(_msg, "Send Drone Config", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DecodeReadVoltageRegulator(byte[] e)
        {
            try
            {
                this._dataFrameConverter.DecodeUtils.DecodeVoltageRegulator(e);
                this._dataResponseState = DataResponseStates.Read_Voltage_Success;
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);
                if (this._dataFrameConverter.DecodeUtils.VoltageRegualtor != null)
                {
                    if (this._manageDroneForm != null)
                    {
                        this._manageDroneForm.ShowParameters(this._dataFrameConverter.DecodeUtils.DroneModel);
                        this._manageDroneForm = null;
                        var _msg = "Read voltage regulator success";
                        LogUtils.AddLog(_msg, LogTypes.Info);
                        MessageBox.Show(_msg, "Read Voltage Regulator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    var _msg = "Read voltage regulator: The voltage regulator config is null or wrong format";
                    LogUtils.AddLog(_msg, LogTypes.Info);
                    MessageBox.Show(_msg, "Read Voltage Regulator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                this._dataResponseState = DataResponseStates.Response_Failed;
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);
            }
        }

        private void DecodeSendVoltageRegulator(byte[] e)
        {
            var _reponseState = this._dataFrameConverter.DecodeUtils.ValidateResponseState(ref e, 3);
            if (_reponseState == DataResponseStates.Response_Success)
            {
                StopTimerResponse();
                this._dataResponseState = DataResponseStates.Send_Voltage_Success;
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);
                SaveVoltageConfig();
                var _msg = "Send voltage regulator success";
                LogUtils.AddLog(_msg, LogTypes.Info);
                MessageBox.Show(_msg, "Send Voltage Regulator", MessageBoxButtons.OK, MessageBoxIcon.Information);                
            }
            else
            {
                this._dataResponseState = DataResponseStates.Response_Failed;
                var _msg = "Send voltage regulator failed";
                LogUtils.AddLog(_msg, LogTypes.Info);
                MessageBox.Show(_msg, "Send Voltage Regulator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DecodeSendMemoryFormat(byte[] e)
        {
            var _reponseState = this._dataFrameConverter.DecodeUtils.ValidateResponseState(ref e, 3);
            if (_reponseState == DataResponseStates.Response_Success)
            {
                StopTimerResponse();
                this._dataResponseState = DataResponseStates.Send_MemoryFormat_Success;
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.None);
                var _msg = "Send memory format success";
                LogUtils.AddLog(_msg, LogTypes.Info);
                MessageBox.Show(_msg, "Send Memory Format", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this._dataResponseState = DataResponseStates.Response_Failed;
                var _msg = "Send memory format failed";
                LogUtils.AddLog(_msg, LogTypes.Info);
                MessageBox.Show(_msg, "Send Memory Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DecodeUARTData(byte[] e)
        {
            byte[] _bytes = this._dataFrameConverter.DecodeUtils.GetDataFrame(ref e, 1);
            if (this._dataFrameConverter.DecodeUtils.ConvertHexToByte(_bytes[0]) == 254)
            {
                LogUtils.AddLog($"UART received: {BitConverter.ToString(e)}", LogTypes.Info);
            }
            else
            {
                //LogUtils.AddLog("UART received: data is wrong format", LogTypes.Info);
            }
        }
        #endregion

        #region Tree Node Control
        public void BuildTreeNodeDevices(TreeView treeView, TreeNode treeNode)
        {
            try
            {
                if (treeView.Nodes.Count > 0)
                    treeView.Nodes.Clear();

                var _treeNode = (TreeNode)treeNode.Clone();
                treeView.Nodes.Add(_treeNode);
                RefreshTreeNode(treeView);
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Info);
            }
        }

        public void BuildTreeNodeDevices(TreeView treeView, ConfigModel configModel, bool isAddEndNode)
        {
            try
            {
                if (treeView.Nodes.Count > 0)
                    treeView.Nodes.Clear();

                if (configModel != null)
                {
                    TreeNode _rootNode = new TreeNode();
                    _rootNode.Text = configModel.ConfigInfo?.Name + $"_v{configModel.ConfigInfo?.Version.ToString("D3")}";
                    _rootNode.Tag = configModel.ConfigInfo;
                    _rootNode.ImageKey = TreeNodeDefine.CONFIG_NODE_NAME;
                    _rootNode.SelectedImageKey = TreeNodeDefine.CONFIG_NODE_NAME;

                    //--- Device Single Comm
                    if (configModel.SingleCommDevices != null && configModel.SingleCommDevices.Count > 0)
                    {
                        foreach (var singleDevice in configModel.SingleCommDevices.Values.ToList())
                        {
                            TreeNode _devNode = new TreeNode();
                            _devNode.Text = $"[{singleDevice.Id}]. {singleDevice.Name}";
                            _devNode.Name = singleDevice.Id.ToString();
                            _devNode.Tag = singleDevice;
                            _devNode.ImageKey = TreeNodeDefine.DEVICE_NODE_NAME;
                            _devNode.SelectedImageKey = TreeNodeDefine.DEVICE_NODE_NAME;

                            _rootNode.Nodes.Add(_devNode);
                        }
                    }

                    //--- Device Double Comm
                    if (configModel.DoubleCommDevices != null && configModel.DoubleCommDevices.Count > 0)
                    {
                        foreach (var doubleDevice in configModel.DoubleCommDevices.Values.ToList())
                        {
                            TreeNode _devNode = new TreeNode();
                            _devNode.Text = $"[{doubleDevice.Id}]. {doubleDevice.Name}";
                            _devNode.Name = doubleDevice.Id.ToString();
                            _devNode.Tag = doubleDevice;
                            _devNode.ImageKey = TreeNodeDefine.DEVICE_NODE_NAME;
                            _devNode.SelectedImageKey = TreeNodeDefine.DEVICE_NODE_NAME;

                            _rootNode.Nodes.Add(_devNode);
                        }
                    }

                    if (isAddEndNode)
                    {
                        //--- Add End Node
                        TreeNode _endNode = new TreeNode();
                        _endNode.Text = $"...";
                        _endNode.Tag = null;
                        _endNode.ImageKey = TreeNodeDefine.END_NODE_NAME;
                        _endNode.SelectedImageKey = TreeNodeDefine.END_NODE_NAME;
                        _rootNode.Nodes.Add(_endNode);
                    }

                    treeView.Nodes.Add(_rootNode);
                }

                RefreshTreeNode(treeView);
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Info);
            }
        }                

        public void BuildTreeNodeDevices(TreeView treeView, DeviceLibrary deviceLibrary)
        {
            try
            {
                if (treeView.Nodes.Count > 0)
                    treeView.Nodes.Clear();

                if (deviceLibrary != null)
                {
                    TreeNode _rootNode = new TreeNode();
                    _rootNode.Text = deviceLibrary.LibraryInfo.Name;
                    _rootNode.Tag = deviceLibrary.LibraryInfo;
                    _rootNode.ImageKey = TreeNodeDefine.LIBRARY_NODE_NAME;
                    _rootNode.SelectedImageKey = TreeNodeDefine.LIBRARY_NODE_NAME;

                    //--- Device Single Comm
                    if (deviceLibrary.SingleCommDevices != null && deviceLibrary.SingleCommDevices.Count > 0)
                    {
                        foreach (var singleDevice in deviceLibrary.SingleCommDevices.Values.ToList())
                        {
                            TreeNode _devNode = new TreeNode();
                            _devNode.Text = $"[{singleDevice.Id}]. {singleDevice.Name}";
                            _devNode.Name = singleDevice.Id.ToString();
                            _devNode.Tag = singleDevice;
                            _devNode.ImageKey = TreeNodeDefine.DEVICE_NODE_NAME;
                            _devNode.SelectedImageKey = TreeNodeDefine.DEVICE_NODE_NAME;

                            _rootNode.Nodes.Add(_devNode);
                        }
                    }

                    //--- Device Double Comm
                    if (deviceLibrary.DoubleCommDevices != null && deviceLibrary.DoubleCommDevices.Count > 0)
                    {
                        foreach (var doubleDevice in deviceLibrary.DoubleCommDevices.Values.ToList())
                        {
                            TreeNode _devNode = new TreeNode();
                            _devNode.Text = $"[{doubleDevice.Id}]. {doubleDevice.Name}";
                            _devNode.Name = doubleDevice.Id.ToString();
                            _devNode.Tag = doubleDevice;
                            _devNode.ImageKey = TreeNodeDefine.DEVICE_NODE_NAME;
                            _devNode.SelectedImageKey = TreeNodeDefine.DEVICE_NODE_NAME;

                            _rootNode.Nodes.Add(_devNode);
                        }
                    }

                    treeView.Nodes.Add(_rootNode);
                }

                RefreshTreeNode(treeView);
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Info);
            }
        }        

        public void BuildTreeNodeDeviceScan(TreeView treeView)
        {
            ConfigModel _newCfgModel = new ConfigModel();
            _newCfgModel.ConfigInfo.Name = "List of scanned devices";
            _newCfgModel.VirtualConfigInfo = _newCfgModel.ConfigInfo.Clone();
            if (this._deviceManager.DeviceScan != null)
                _newCfgModel.Devices = this._deviceManager.DeviceScan.Devices;
            _newCfgModel.ValidateDevices();

            if (treeView.InvokeRequired)
            {
                treeView.Invoke(new Action(() =>
                {
                    BuildTreeNodeDeviceScan(treeView, _newCfgModel);
                }));
            }
            else
                BuildTreeNodeDeviceScan(treeView, _newCfgModel);

            this._uiMain.ShowScanDevice(this._deviceManager);
        }

        public void BuildTreeNodeDeviceScan(TreeView treeView, ConfigModel configModel)
        {
            try
            {
                if (treeView.Nodes.Count > 0)
                    treeView.Nodes.Clear();

                if (configModel != null)
                {
                    TreeNode _rootNode = new TreeNode();
                    _rootNode.Text = configModel.ConfigInfo?.Name;
                    _rootNode.Tag = configModel.ConfigInfo;
                    _rootNode.ImageKey = TreeNodeDefine.CONFIG_NODE_NAME;
                    _rootNode.SelectedImageKey = TreeNodeDefine.CONFIG_NODE_NAME;

                    if (this._deviceManager.DeviceScan != null)
                    {
                        //--- Bracket 1
                        if (this._deviceManager.DeviceScan.Devices.Count > 0 && this._deviceManager.DeviceScan.Devices[0] != null)
                        {
                            var _device = this._deviceManager.DeviceScan.Devices[0];
                            if (_device.Communications.Count > 1)
                            {
                                var _doubleDevice = _device.GetDoubleCommDevice();

                                TreeNode _devNode = new TreeNode();
                                _devNode.Text = $"[{_doubleDevice.Id}]. {_doubleDevice.Name} ({DevicePositions.Bracket_1})";
                                _devNode.Name = _doubleDevice.Id.ToString();
                                _devNode.Tag = _doubleDevice;
                                _devNode.ImageKey = TreeNodeDefine.DEVICE_NODE_NAME;
                                _devNode.SelectedImageKey = TreeNodeDefine.DEVICE_NODE_NAME;

                                _rootNode.Nodes.Add(_devNode);
                            }
                            else
                            {
                                var _singleDevice = _device.GetSingleCommDevice();

                                TreeNode _devNode = new TreeNode();
                                _devNode.Text = $"[{_singleDevice.Id}]. {_singleDevice.Name} ({DevicePositions.Bracket_1})";
                                _devNode.Name = _singleDevice.Id.ToString();
                                _devNode.Tag = _singleDevice;
                                _devNode.ImageKey = TreeNodeDefine.DEVICE_NODE_NAME;
                                _devNode.SelectedImageKey = TreeNodeDefine.DEVICE_NODE_NAME;

                                _rootNode.Nodes.Add(_devNode);
                            }
                        }

                        //--- Bracket 2
                        if (this._deviceManager.DeviceScan.Devices.Count > 1 && this._deviceManager.DeviceScan.Devices[1] != null)
                        {
                            var _device = this._deviceManager.DeviceScan.Devices[1];
                            if (_device.Communications.Count > 1)
                            {
                                var _doubleDevice = _device.GetDoubleCommDevice();

                                TreeNode _devNode = new TreeNode();
                                _devNode.Text = $"[{_doubleDevice.Id}]. {_doubleDevice.Name} ({DevicePositions.Bracket_2})";
                                _devNode.Name = _doubleDevice.Id.ToString();
                                _devNode.Tag = _doubleDevice;
                                _devNode.ImageKey = TreeNodeDefine.DEVICE_NODE_NAME;
                                _devNode.SelectedImageKey = TreeNodeDefine.DEVICE_NODE_NAME;

                                _rootNode.Nodes.Add(_devNode);
                            }
                            else
                            {
                                var _singleDevice = _device.GetSingleCommDevice();

                                TreeNode _devNode = new TreeNode();
                                _devNode.Text = $"[{_singleDevice.Id}]. {_singleDevice.Name} ({DevicePositions.Bracket_2})";
                                _devNode.Name = _singleDevice.Id.ToString();
                                _devNode.Tag = _singleDevice;
                                _devNode.ImageKey = TreeNodeDefine.DEVICE_NODE_NAME;
                                _devNode.SelectedImageKey = TreeNodeDefine.DEVICE_NODE_NAME;

                                _rootNode.Nodes.Add(_devNode);
                            }
                        }
                    }

                    treeView.Nodes.Add(_rootNode);
                }

                RefreshTreeNode(treeView);
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Info);
            }
        }

        public void RebuildTreeNodeDeviceScan(TreeView treeView, DeviceConfig deviceConfig, ActionModes actionMode)
        {
            if (treeView.Nodes.Count > 0)
            {
                var _rootNode = treeView.Nodes[0];
                switch (actionMode)
                {
                    case ActionModes.Edit_Device:
                        {
                            for (int i = 0; i < _rootNode.Nodes.Count; i++)
                            {
                                var _devNode = _rootNode.Nodes[i];
                                if (_devNode.Name == deviceConfig.Id.ToString())
                                {
                                    if (this._deviceManager.DeviceScan != null)
                                    {
                                        if (this._deviceManager.DeviceScan.Devices.Count > 0 && this._deviceManager.DeviceScan.Devices[0] != null
                                            && this._deviceManager.DeviceScan.Devices[0].Id == deviceConfig.Id)
                                            _devNode.Text = $"[{deviceConfig.Id}]. {deviceConfig.Name} ({DevicePositions.Bracket_1})*";

                                        if (this._deviceManager.DeviceScan.Devices.Count > 1 && this._deviceManager.DeviceScan.Devices[1] != null
                                            && this._deviceManager.DeviceScan.Devices[1].Id == deviceConfig.Id)
                                            _devNode.Text = $"[{deviceConfig.Id}]. {deviceConfig.Name} ({DevicePositions.Bracket_2})*";

                                        if (deviceConfig.Communications.Count > 1)
                                            _devNode.Tag = deviceConfig.GetDoubleCommDevice();
                                        else
                                            _devNode.Tag = deviceConfig.GetSingleCommDevice();

                                        break;
                                    }
                                }
                            }

                            break;
                        }
                    case ActionModes.Delete_Device:
                        {
                            for (int i = 0; i < _rootNode.Nodes.Count; i++)
                            {
                                var _devNode = _rootNode.Nodes[i];
                                if (_devNode.Name == deviceConfig.Id.ToString())
                                {
                                    _rootNode.Nodes.Remove(_devNode);
                                    break;
                                }
                            }

                            break;
                        }
                }

                RefreshTreeNode(treeView);
            }
        }

        public void RebuildTreeNodeDevices(TreeView treeView, string rootName, ActionModes actionMode)
        {
            if (treeView.Nodes.Count > 0)
            {
                var _rootNode = treeView.Nodes[0];
                switch (actionMode)
                {
                    case ActionModes.Edit_Root:
                        {
                            _rootNode.Text = rootName + "*";
                            break;
                        }
                }

                RefreshTreeNode(treeView);
            }
        }

        public void RebuildTreeNodeDevices(TreeView treeView, DeviceConfig deviceConfig, ActionModes actionMode)
        {
            if (treeView.Nodes.Count > 0)
            {
                var _rootNode = treeView.Nodes[0];
                switch (actionMode)
                {                    
                    case ActionModes.Add_Device:
                        {
                            TreeNode _devNode = new TreeNode();
                            _devNode.Text = $"[{deviceConfig.Id}]. {deviceConfig.Name}*";
                            _devNode.Name = deviceConfig.Id.ToString();

                            if (deviceConfig.Communications.Count > 1)
                                _devNode.Tag = deviceConfig.GetDoubleCommDevice();
                            else
                                _devNode.Tag = deviceConfig.GetSingleCommDevice();

                            _devNode.ImageKey = TreeNodeDefine.DEVICE_NODE_NAME;
                            _devNode.SelectedImageKey = TreeNodeDefine.DEVICE_NODE_NAME;

                            _rootNode.Nodes.Add(_devNode);

                            break;
                        }
                    case ActionModes.Edit_Device:
                        {
                            for (int i = 0; i < _rootNode.Nodes.Count; i++)
                            {
                                var _devNode = _rootNode.Nodes[i];
                                if (_devNode.Name == deviceConfig.Id.ToString())
                                {
                                    _devNode.Text = $"[{deviceConfig.Id}]. {deviceConfig.Name}*";
                                    if (deviceConfig.Communications.Count > 1)
                                        _devNode.Tag = deviceConfig.GetDoubleCommDevice();
                                    else
                                        _devNode.Tag = deviceConfig.GetSingleCommDevice();

                                    break;
                                }
                            }

                            break;
                        }
                    case ActionModes.Delete_Device:
                        {
                            for (int i = 0; i < _rootNode.Nodes.Count; i++)
                            {
                                var _devNode = _rootNode.Nodes[i];
                                if (_devNode.Name == deviceConfig.Id.ToString())
                                {
                                    _rootNode.Nodes.Remove(_devNode);
                                    break;
                                }
                            }

                            break;
                        }
                }

                RefreshTreeNode(treeView);
            }
        }

        public void RemoveNodeFromTree(TreeView treeView, DeviceConfig nodeDeviceConfig)
        {
            try
            {
                if (treeView != null && treeView.Nodes.Count > 0)
                {
                    var _nodes = treeView.Nodes.Cast<TreeNode>().ToList();
                    if (_nodes.Count > 0)
                    {
                        var _node = _nodes[0].Nodes.Cast<TreeNode>().ToList().Find(x => x.Name == nodeDeviceConfig.Id.ToString());
                        if (_node != null)
                        {
                            treeView.Nodes.Remove(_node);
                            RefreshTreeNode(treeView);
                        }
                    }
                }
            }
            catch
            {
                LogUtils.AddLog($"Delete device [{nodeDeviceConfig.Name}] failed", LogTypes.Info);
            }
        }

        private void RefreshTreeNode(TreeView treeView)
        {
            treeView.Refresh();
            treeView.ExpandAll();            
            //treeView.Sort();
        }
        #endregion        

        #region Config Management
        public bool EditConfigInfo(TreeView treeView)
        {
            var _oldName = $"{this._configManager.Model.VirtualConfigInfo.Name}_v{this._configManager.Model.VirtualConfigInfo.Version.ToString("D3")}";
            ConfigDeviceForm _frm = new ConfigDeviceForm(this._configManager.Model.VirtualConfigInfo, this._configManager);
            if (_frm.ShowDialog() == DialogResult.OK)
            {
                var _newName = $"{this._configManager.Model.VirtualConfigInfo.Name}_v{this._configManager.Model.VirtualConfigInfo.Version.ToString("D3")}";
                if (_oldName != _newName)
                {
                    RebuildTreeNodeDevices(treeView, _newName, ActionModes.Edit_Root);

                    if (File.Exists(PATH_MANAGER.activeConfigDir + _oldName))
                        File.Delete(PATH_MANAGER.activeConfigDir + _oldName);

                    if (File.Exists(PATH_MANAGER.deviceConfigDir + _oldName))
                        File.Delete(PATH_MANAGER.deviceConfigDir + _oldName);

                    return true;
                }               
            }

            return false;
        }

        public void AddNewConfig()
        {
            try
            {
                this._configManager.Model = new ConfigModel();
                //this._configManager.Model.InitDevices();
                //ShowDroneSide();

                LogUtils.AddLog($"Add new device config [name: {this._configManager.Model.VirtualConfigInfo.Name} | " +
                    $"version: {this._configManager.Model.VirtualConfigInfo.Version}] successfully", LogTypes.Info);
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
            }
        }

        public bool OpenManageConfig()
        {
            ManageConfigForm _frm = new ManageConfigForm(this._configManager);
            if (_frm.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(_frm.ConfigPath))
                    OpenConfig(_frm.ConfigPath);
                else
                    OpenConfig(this.ConfigManager.Model);

                //ShowDroneSide();

                return true;
            }

            return false;
        }

        private void OpenConfig(string configPath)
        {
            try
            {               
                if (!string.IsNullOrEmpty(configPath))
                {
                    this._configManager.Model = this._configManager.GetConfig(configPath);
                    this._configManager.Model.VirtualConfigInfo = this._configManager.Model.ConfigInfo.Clone();
                    this._configManager.Model.ValidateDevices();
                    if (this._configManager != null)
                        LogUtils.AddLog($"The device config [name: {this._configManager.Model.VirtualConfigInfo.Name} | " +
                                $"version: {this._configManager.Model.VirtualConfigInfo.Version}] is opened", LogTypes.Info);
                    else
                        LogUtils.AddLog("The device config is null or wrong format", LogTypes.Info);
                }
                else
                    LogUtils.AddLog("The device config is not selected", LogTypes.Info);
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
            }
        }        

        private void OpenConfig(ConfigModel configModel)
        {
            try
            {
                if (configModel != null)
                {
                    this._configManager.Model = configModel;
                    this._configManager.Model.VirtualConfigInfo = this._configManager.Model.ConfigInfo.Clone();
                    this._configManager.Model.ValidateDevices();
                    if (this._configManager != null)
                        LogUtils.AddLog($"The device config [name: {this._configManager.Model.VirtualConfigInfo.Name} | " +
                                $"version: {this._configManager.Model.VirtualConfigInfo.Version}] is opened", LogTypes.Info);
                    else
                        LogUtils.AddLog("The device config is null or wrong format", LogTypes.Info);
                }
                else
                    LogUtils.AddLog("The device config is not selected", LogTypes.Info);
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
            }
        }

        public bool SaveConfig()
        {
            try
            {
                if (this._configManager != null)
                {
                    if (this._configManager.Model.VirtualConfigInfo.Version == 0)
                    {
                        MessageBox.Show("The config version must be greater than 0", "Save Config", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    if (this._configManager.Model.ConfigInfo.Version != this._configManager.Model.VirtualConfigInfo.Version && 
                        this._configManager.ModelIds.Contains(this._configManager.Model.VirtualConfigInfo.Version))
                    {
                        var _dialogResult = MessageBox.Show($"The config version [{this._configManager.Model.VirtualConfigInfo.Version}] is existed, " +
                                $"Do you want to replace?", "Save Config", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (_dialogResult == DialogResult.No)
                            return false;
                    }

                    var _fileName = $"{this._configManager.Model.VirtualConfigInfo.Name}_v{this._configManager.Model.VirtualConfigInfo.Version.ToString("D3")}.json";
                    string _savePath = PATH_MANAGER.activeConfigDir + _fileName;
                    string _destFile = PATH_MANAGER.deviceConfigDir + _fileName;

                    if (!string.IsNullOrEmpty(_savePath))
                    {
                        if (ValidateDeviceConfigs(this._configManager.Model.VirtualConfigInfo.Version.ToString("D3")))
                        {
                            this._configManager.Model.Save();
                            this._configManager.UpdateModel(this._configManager.Model);
                            HelperUtils.SaveJsonFile(this._configManager.Model, _savePath);
                            HelperUtils.TransferFile(_savePath, _destFile);
                            LoadConfigLibrary();
                            //ShowDroneSide();

                            LogUtils.AddLog($"The device config [{_fileName}] is saved", LogTypes.Info);
                            return true;
                        }
                    }

                    LogUtils.AddLog($"The device config [{_fileName}] is not saved", LogTypes.Info);
                }
                else
                    LogUtils.AddLog("The device config is null or wrong format", LogTypes.Info);
                
                return false;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return false;
            }
        }

        public bool SaveAsConfig()
        {
            try
            {
                if (this._configManager != null)
                {
                    var _fileName = $"{this._configManager.Model.VirtualConfigInfo.Name}_v{this._configManager.Model.VirtualConfigInfo.Version.ToString("D3")}.json";
                    string _savePath = HelperUtils.GetSavePath("Save Configuration", FilterTypes.JSON_FORMAT, _fileName);

                    if (!string.IsNullOrEmpty(_savePath))
                    {
                        ConfigModel _newModel = new ConfigModel();
                        _newModel.ConfigInfo = this._configManager.Model.ConfigInfo;
                        _newModel.Devices = this._configManager.Model.VirtualDevices.Values.ToList();
                        HelperUtils.SaveJsonFile(_newModel, _savePath);
                        LogUtils.AddLog($"The device config [{_fileName}] is saved to {Path.GetDirectoryName(_savePath)}", LogTypes.Info);

                        return true;
                    }

                    LogUtils.AddLog($"The device config [{_fileName}] is not saved", LogTypes.Info);
                }
                else
                    LogUtils.AddLog("The device config is null or wrong format", LogTypes.Info);

                return false;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return false;
            }
        }

        private bool ValidateDeviceConfigs(string version)
        {
            try
            {
                if (Directory.Exists(PATH_MANAGER.activeConfigDir) && Directory.Exists(PATH_MANAGER.deviceConfigDir))
                {
                    DirectoryInfo _di = new DirectoryInfo(PATH_MANAGER.activeConfigDir);
                    var _activeCfgPaths = _di.GetFiles();
                    if (_activeCfgPaths.Length > 0)
                    {
                        foreach (var file in _activeCfgPaths)
                        {
                            file.Delete();
                        }
                    }

                    var _filePaths = Directory.GetFiles(PATH_MANAGER.deviceConfigDir);
                    if (_filePaths.Length > 0)
                    {
                        foreach (var file in _filePaths)
                        {
                            if (file.Contains(version))
                                File.Delete(file);
                        }
                    }

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }        

        public void RefreshDeviceConfig(TreeView treeView, bool isAddEndNode)
        {
            this._configManager.Model.ValidateDevices();
            this._configManager.Model.VirtualConfigInfo = this._configManager.Model.ConfigInfo.Clone();
            BuildTreeNodeDevices(treeView, this._configManager.Model, isAddEndNode);
        }
        #endregion        

        #region Device Management
        public void EditLibraryInfo(TreeView treeView)
        {
            LibraryDeviceForm _frm = new LibraryDeviceForm(this._deviceManager.DeviceLibrary);
            if (_frm.ShowDialog() == DialogResult.OK)
                RebuildTreeNodeDevices(treeView, this._deviceManager.DeviceLibrary.VirtualLibraryInfo.Name, ActionModes.Edit_Root);
        }

        public void AddNewDevice(TreeView treeView)
        {
            try
            {
                if (this._connectionManager.IsOpen)
                {
                    AddDeviceForm _frm = new AddDeviceForm(this._deviceManager);
                    if (_frm.ShowDialog() == DialogResult.OK)
                        RegisterNewDevice(treeView);
                }
                else
                {
                    MessageBox.Show("Disconnected to mainboard!\nPlease make a connection before operating", "Add New Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
            }
        }

        public DeviceConfig EditDevice(TreeView treeView, DeviceConfig deviceConfig, SourceDeviceModes srcDeviceMode)
        {
            try
            {
                if (deviceConfig != null)
                {
                    bool _isEnableEditId = false;
                    if (this._deviceManager.DeviceScan != null && this._deviceManager.DeviceScan.Devices.Count > 0 && this._deviceManager.DeviceScan.Devices[0] != null)
                        if (this._deviceManager.DeviceScan.Devices[0].Id == deviceConfig.Id)
                            _isEnableEditId = true;

                    this._isChangeId = false;
                    EditDeviceForm _frm = new EditDeviceForm(this._deviceManager, deviceConfig, _isEnableEditId);
                    if (_frm.ShowDialog() == DialogResult.OK)
                    {
                        this._oldId = _frm.OldId;
                        switch (srcDeviceMode)
                        {
                            case SourceDeviceModes.Library:
                                {
                                    if (_frm.EditedDevice.Id != this._oldId)
                                    {
                                        this._isChangeId = true;
                                        this._deviceManager.NewDevice = _frm.EditedDevice;
                                        ChangeDeviceId(treeView);                               
                                    }
                                    else
                                    {
                                        this._deviceManager.DeviceLibrary.UpdateDevice(_frm.EditedDevice);
                                        RebuildTreeNodeDevices(treeView, _frm.EditedDevice, ActionModes.Edit_Device);
                                    }

                                    break;
                                }
                            case SourceDeviceModes.Config:
                                {
                                    this._configManager.Model.UpdateDevice(_frm.EditedDevice);
                                    RebuildTreeNodeDevices(treeView, _frm.EditedDevice, ActionModes.Edit_Device);

                                    break;
                                }
                            case SourceDeviceModes.Scan:
                                {
                                    if (_frm.EditedDevice.Id != this._oldId)
                                    {
                                        this._isChangeId = true;
                                        this._deviceManager.NewDevice = _frm.EditedDevice;
                                        ChangeDeviceId(treeView);
                                    }
                                    else
                                    {
                                        this._deviceManager.DeviceScan.UpdateDevice(_frm.EditedDevice);
                                        RebuildTreeNodeDeviceScan(treeView, _frm.EditedDevice, ActionModes.Edit_Device);
                                    }

                                    break;
                                }
                        }
                        
                        return _frm.EditedDevice;
                    }                        
                }
                else
                {
                    MessageBox.Show("Device is not selected");
                }

                return null;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return null;
            }
        }        

        public bool DeleteDevice(TreeView treeView, DeviceConfig deviceConfig, SourceDeviceModes srcDeviceMode)
        {
            try
            {
                if (deviceConfig != null)
                {
                    var _devInfo = $"name: {deviceConfig.Name} | id: {deviceConfig.Id}";
                    var _dialogResult = MessageBox.Show($"Do you want to delete [{_devInfo}]?", "Delete Device", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (_dialogResult == DialogResult.Yes)
                    {
                        var _msg = "";
                        switch (srcDeviceMode)
                        {
                            case SourceDeviceModes.Library:
                                {
                                    this._deviceManager.DeviceLibrary.RemoveDevice(deviceConfig);
                                    _msg = $"[Library]_Device [{_devInfo}] has been deleted";                                    
                                    SaveLibrary();

                                    break;
                                }
                            case SourceDeviceModes.Config:
                                {
                                    this._configManager.Model.RemoveDevice(deviceConfig);
                                    _msg = $"[Config]_Device [{_devInfo}] has been deleted";
                                    SaveConfig();

                                    break;
                                }
                            case SourceDeviceModes.Scan:
                                {
                                    this._deviceManager.DeviceScan.RemoveDevice(deviceConfig);
                                    _msg = $"[Scan Device]_Device [{_devInfo}] has been deleted";
                                    this._deviceManager.DeviceScan.Save();

                                    break;
                                }
                        }

                        LogUtils.AddLog(_msg, LogTypes.Info);
                        RebuildTreeNodeDevices(treeView, deviceConfig, ActionModes.Delete_Device);
                        MessageBox.Show(_msg, "Delete Device", MessageBoxButtons.OK, MessageBoxIcon.Information);                        
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("Device is not selected");
                }

                return false;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return false;
            }
        }

        public bool DeleteDevice(TreeView treeView, List<DeviceConfig> deviceConfigs, SourceDeviceModes srcDeviceMode)
        {
            try
            {
                if (deviceConfigs != null && deviceConfigs.Count > 0)
                {
                    var _dialogResult = MessageBox.Show($"Do you want to delete all selected devices?", "Delete Device", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (_dialogResult == DialogResult.Yes)
                    {
                        foreach (var device in deviceConfigs)
                        {
                            var _devInfo = $"name: {device.Name} | id: {device.Id}";
                            switch (srcDeviceMode)
                            {
                                case SourceDeviceModes.Library:
                                    {
                                        this._deviceManager.DeviceLibrary.RemoveDevice(device);
                                        var _msg = $"[Library]_Device [{_devInfo}] has been deleted";
                                        LogUtils.AddLog(_msg, LogTypes.Info);

                                        break;
                                    }
                                case SourceDeviceModes.Config:
                                    {
                                        this._configManager.Model.RemoveDevice(device);
                                        var _msg = $"[Config]_Device [{_devInfo}] has been deleted";
                                        LogUtils.AddLog(_msg, LogTypes.Info);

                                        break;
                                    }
                                case SourceDeviceModes.Scan:
                                    {
                                        this._deviceManager.DeviceScan.RemoveDevice(device);
                                        var _msg = $"[Scan Device]_Device [{_devInfo}] has been deleted";
                                        LogUtils.AddLog(_msg, LogTypes.Info);

                                        break;
                                    }
                            }                                                                                                         
                        }
                        
                        switch (srcDeviceMode)
                        {
                            case SourceDeviceModes.Library:
                                {
                                    BuildTreeNodeDevices(treeView, this._deviceManager.DeviceLibrary);
                                    SaveLibrary();

                                    break;
                                }
                            case SourceDeviceModes.Config:
                                {
                                    BuildTreeNodeDevices(treeView, this._configManager.Model, false);
                                    SaveConfig();

                                    break;
                                }
                            case SourceDeviceModes.Scan:
                                {
                                    BuildTreeNodeDeviceScan(treeView);
                                    this._deviceManager.DeviceScan.Save();

                                    break;
                                }
                        }

                        MessageBox.Show("Done", "Delete Device", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }                        
                }
                else
                {
                    MessageBox.Show("Device is not selected");
                }

                return false;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return false;
            }
        }

        public void OpenManageDevice(TreeNode treeNode)
        {
            ManageDeviceForm _frm = new ManageDeviceForm(this, treeNode);
            _frm.ShowDialog();
        }

        public void OpenScanDevice(TreeNode treeNode)
        {
            ScanDeviceForm _frm = new ScanDeviceForm(this, treeNode);
            _frm.ShowDialog();                       
        }

        public bool ValidateDevices()
        {
            try
            {
                var _libraryNotConfig = this._deviceManager.DeviceLibrary.Devices.Except(this._configManager.Model.Devices).ToList();
                //if (this._configManager.Model.Devices.Count > 0)
                //{
                //    foreach (var device in this._configManager.Model.Devices)
                //    {

                //    }
                //}
                var _configNotLibrary = this._configManager.Model.Devices.Except(this._deviceManager.DeviceLibrary.Devices).ToList();

                return false;
            }
            catch
            {
                return false;
            }
        }

        public void SaveLibrary()
        {
            try
            {
                if (this._deviceManager != null)
                {
                    string _savePath = PATH_MANAGER.deviceLibraryPath;

                    if (!string.IsNullOrEmpty(_savePath))
                    {
                        this._deviceManager.DeviceLibrary.Save();
                        HelperUtils.SaveJsonFile(this._deviceManager.DeviceLibrary, _savePath);

                        LogUtils.AddLog($"The device library [{this._deviceManager.DeviceLibrary.LibraryInfo.Name}] is saved", LogTypes.Info);
                    }
                    else
                        LogUtils.AddLog($"The device library [{this._deviceManager.DeviceLibrary.LibraryInfo.Name}] is not saved", LogTypes.Info);
                }
                else
                    LogUtils.AddLog("The device library is null or wrong format", LogTypes.Info);
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
            }
        }

        public void SaveAsLibrary()
        {
            try
            {
                if (this._deviceManager != null)
                {
                    string _savePath = HelperUtils.GetSavePath("Save Configuration", FilterTypes.JSON_FORMAT, this._deviceManager.DeviceLibrary.LibraryInfo.Name);

                    if (!string.IsNullOrEmpty(_savePath))
                    {
                        this._deviceManager.DeviceLibrary.Save();
                        HelperUtils.SaveJsonFile(this._deviceManager.DeviceLibrary, _savePath);

                        LogUtils.AddLog($"The device library [{this._deviceManager.DeviceLibrary.LibraryInfo.Name}] is saved", LogTypes.Info);
                    }
                    else
                        LogUtils.AddLog($"The device library [{this._deviceManager.DeviceLibrary.LibraryInfo.Name}] is not saved", LogTypes.Info);
                }
                else
                    LogUtils.AddLog("The device library is null or wrong format", LogTypes.Info);
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
            }
        }

        public void RefreshDeviceLibrary(TreeView treeView)
        {
            this._deviceManager.DeviceLibrary.ValidateDevices();
            this._deviceManager.DeviceLibrary.VirtualLibraryInfo = this._deviceManager.DeviceLibrary.LibraryInfo.Clone();
            BuildTreeNodeDevices(treeView, this._deviceManager.DeviceLibrary);
        }
        #endregion

        #region Utilities Function
        public void OpenManageDrone()
        {
            ManageDroneForm _frm = new ManageDroneForm(this);
            _frm.ShowDialog();
        }

        private void SaveVoltageConfig()
        {
            try
            {
                if (this._configManager.VoltageConfig != null)
                {
                    string _savePath = PATH_MANAGER.voltageConfigPath;

                    if (!string.IsNullOrEmpty(_savePath))
                    {
                        HelperUtils.SaveJsonFile(this._configManager.VoltageConfig, _savePath);

                        LogUtils.AddLog($"The voltage regulator configuration is saved", LogTypes.Info);
                    }
                    else
                        LogUtils.AddLog($"The voltage regulator configuration is not saved", LogTypes.Info);
                }
                else
                    LogUtils.AddLog("The voltage regulator configuration is null or wrong format", LogTypes.Info);
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
            }
        }

        public void OpenVoltageRegulator()
        {
            VoltageRegulatorForm __frm = new VoltageRegulatorForm(this);
            __frm.ShowDialog();
        }
        #endregion

        #region Data Exchange
        public void ScanDevice(TreeView treeView)
        {
            if (this._connectionManager.IsOpen)
            {
                this._dataResponseState = DataResponseStates.None;
                this._treeView = treeView;
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.Scan_Device);
                var _uiCmdCode = this._dataFrameConverter.EncodeUtils.ConvertUICmdTypesToHex(UICmdTypes.Scan_Device);
                var _datas = new byte[] { DataFrameCodes.StartCode[0], _uiCmdCode, DataFrameCodes.EndCode[0] };
                if (_datas.Length > 0)
                {
                    this._connectionManager.WriteData(_datas);
                    StartTimerResponse();
                }
            }
            else
            {
                MessageBox.Show("Disconnected to mainboard!\nPlease make a connection before operating", "Scan Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }            
        }

        public void RegisterNewDevice(TreeView treeView)
        {            
            this._dataResponseState = DataResponseStates.None;
            this._treeView = treeView;
            if (this._deviceManager.NewDevice != null)
            {
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.Register_New_Device);
                var _datas = this._dataFrameConverter.EncodeUtils.Encode(this._deviceManager.NewDevice.Id);
                if (_datas.Length > 0)
                {
                    this._connectionManager.WriteData(_datas);
                    StartTimerResponse();
                }
            }         
        }

        public void ChangeDeviceId(TreeView treeView)
        {
            this._dataResponseState = DataResponseStates.None;
            this._treeView = treeView;
            if (this._deviceManager.NewDevice != null)
            {
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.Change_Device_Id);
                var _datas = this._dataFrameConverter.EncodeUtils.Encode(this._deviceManager.NewDevice.Id);
                if (_datas.Length > 0)
                {
                    this._connectionManager.WriteData(_datas);
                    StartTimerResponse();
                }
            }
        }

        public void ReadDeviceConfig()
        {
            this._dataResponseState = DataResponseStates.None;
            if (this._connectionManager.IsOpen)
            {
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.Read_Device_Config);
                var _uiCmdCode = this._dataFrameConverter.EncodeUtils.ConvertUICmdTypesToHex(UICmdTypes.Read_Device_Config);
                var _datas = new byte[] { DataFrameCodes.StartCode[0], _uiCmdCode, DataFrameCodes.EndCode[0] };
                if (_datas.Length > 0)
                {
                    this._connectionManager.WriteData(_datas);
                    StartTimerResponse();
                }
            }
            else
            {
                MessageBox.Show("Disconnected to mainboard!\nPlease make a connection before operating", "Read Config", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }            
        }

        public void SendDeviceConfig()
        {
            this._dataResponseState = DataResponseStates.None;
            if (this._connectionManager.IsOpen)
            {
                if (SyncDevices())
                {
                    this._dataFrameConverter.UpdateUICmd(UICmdTypes.Send_Device_Config);
                    this._datas = this._dataFrameConverter.EncodeUtils.Encode(this._configManager);                    
                    if (this._datas != null && this._datas.Count > 0)
                    {
                        this._connectionManager.WriteData(this._datas.First().Value);
                        StartTimerResponse();                        
                    }
                }                
            }
            else
            {
                MessageBox.Show("Disconnected to mainboard!\nPlease make a connection before operating", "Send Config", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void ReadDroneConfig()
        {
            this._dataResponseState = DataResponseStates.None;
            if (this._connectionManager.IsOpen)
            {
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.Read_Drone_Config);
                var _uiCmdCode = this._dataFrameConverter.EncodeUtils.ConvertUICmdTypesToHex(UICmdTypes.Read_Drone_Config);
                var _datas = new byte[] { DataFrameCodes.StartCode[0], _uiCmdCode, DataFrameCodes.EndCode[0] };
                if (_datas.Length > 0)
                {
                    this._connectionManager.WriteData(_datas);
                    StartTimerResponse();
                }
            }
            else
            {
                MessageBox.Show("Disconnected to mainboard!\nPlease make a connection before operating", "Read Drone Config", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void SendDroneConfig(DroneModel droneModel)
        {
            this._dataResponseState = DataResponseStates.None;
            if (this._connectionManager.IsOpen)
            {
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.Send_Drone_Config);
                var _datas = this._dataFrameConverter.EncodeUtils.Encode(droneModel);
                if (_datas != null && _datas.Length > 0)
                {
                    this._connectionManager.WriteData(_datas);
                    StartTimerResponse();
                }
            }
            else
            {
                MessageBox.Show("Disconnected to mainboard!\nPlease make a connection before operating", "Send Drone Config", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void ReadVoltageRegulator()
        {
            this._dataResponseState = DataResponseStates.None;
            if (this._connectionManager.IsOpen)
            {
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.Read_Voltage_Regulator);
                var _uiCmdCode = this._dataFrameConverter.EncodeUtils.ConvertUICmdTypesToHex(UICmdTypes.Read_Voltage_Regulator);
                var _datas = new byte[] { DataFrameCodes.StartCode[0], _uiCmdCode, DataFrameCodes.EndCode[0] };
                if (_datas.Length > 0)
                {
                    this._connectionManager.WriteData(_datas);
                    StartTimerResponse();
                }
            }
            else
            {
                MessageBox.Show("Disconnected to mainboard!\nPlease make a connection before operating", "Read Voltage Regulator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void SendVoltageRegulator(VoltageRegulator voltageRegulator)
        {
            this._dataResponseState = DataResponseStates.None;
            if (this._connectionManager.IsOpen)
            {
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.Send_Voltage_Regulator);
                var _datas = this._dataFrameConverter.EncodeUtils.Encode(voltageRegulator);
                if (_datas != null && _datas.Length > 0)
                {
                    this._connectionManager.WriteData(_datas);
                    StartTimerResponse();
                }
            }
            else
            {
                MessageBox.Show("Disconnected to mainboard!\nPlease make a connection before operating", "Send Voltage Regulator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void SendMemoryFormat()
        {
            this._dataResponseState = DataResponseStates.None;
            if (this._connectionManager.IsOpen)
            {
                this._dataFrameConverter.UpdateUICmd(UICmdTypes.Send_Memory_Format);
                var _datas = this._dataFrameConverter.EncodeUtils.EncodeMemoryFormat();
                if (_datas != null && _datas.Length > 0)
                {
                    this._connectionManager.WriteData(_datas);
                    StartTimerResponse();
                }
            }
            else
            {
                MessageBox.Show("Disconnected to mainboard!\nPlease make a connection before operating", "Send Memory Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool SyncDevices()
        {
            var _dialogResult = MessageBox.Show("The config must be synchronized with the library, Do you want to continue?", 
                "Send Config", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (_dialogResult == DialogResult.Yes)
            {
                if (this._configManager.Model.Devices.Count > 0)
                {
                    foreach (var device in  _configManager.Model.Devices)
                    {
                        this._deviceManager.DeviceLibrary.UpdateDevice(device);
                    }

                    this._deviceManager.DeviceLibrary.Save();
                    LogUtils.AddLog($"The synchronization process is completed", LogTypes.Info);

                    return true;
                }
            }

            return false;
        }
        #endregion

        public void UpdateManageDevice(ManageDeviceForm manageDeviceForm)
        {
            this._manageDeviceForm = manageDeviceForm;
        }

        public void UpdateManageDrone(ManageDroneForm manageDroneForm)
        {
            this._manageDroneForm = manageDroneForm;
        }

        public void UpdateScanDevice(ScanDeviceForm scanDeviceForm)
        {
            this._scanDeviceForm = scanDeviceForm;
        }

        private void WaitDataResponse(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
            };

            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }
    }
}

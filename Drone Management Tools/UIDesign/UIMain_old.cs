using System;
using System.Data;
using System.Linq;
using System.IO;
using System.Timers;
using System.Drawing;
using System.Threading;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using Bunifu.UI.WinForms;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.Utilities;
using Drone_Management_Tools.Organizer;
using Drone_Management_Tools.UIDesign;

namespace Drone_Management_Tools.UIDesign
{
    public partial class UIMain_old : Form
    {
        public delegate void DelegateGetEventLogs(LogContent logContent);
        public delegate void DelegateUpdateTime();

        public DataServerProcess _dsProcess;

        System.Timers.Timer timerPrintLog;
        private int _fixSplitterDistance;

        public UIMain_old()
        {
            InitializeComponent();
        }

        private void UIMain_Load(object sender, EventArgs e)
        {
            this.timerPrintLog = new System.Timers.Timer();
            this.timerPrintLog.Interval = 1000;
            this.timerPrintLog.Elapsed += TimerPrintLog_Elapsed;
            this.timerPrintLog.Start();

            Thread th1 = new Thread(() => ShowSystemTime());
            th1.Start();
            
            Thread th2 = new Thread(() => StartDataServerProcess());
            th2.Start();
        }

        private void TimerPrintLog_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.timerPrintLog.Stop();

            foreach (var logContent in LogUtils.GetLogs())
            {
                GetLogs(logContent);
            }

            this.timerPrintLog.Start();
        }        

        private void ShowSystemTime()
        {
            if (this.InvokeRequired)
            {
                DelegateUpdateTime d = new DelegateUpdateTime(ShowSystemTime);
                this.Invoke(d, new object[] { });
            }
            else
            {
                timerSystem.Interval = 1000;
                timerSystem.Tick += timerSystem_Tick;
                timerSystem.Enabled = true;
            }
        }

        private void timerSystem_Tick(object sender, EventArgs e)
        {
            this.sttSystemTime.Text = $"{string.Format("{0:dd-MMM-yyyy HH:mm:ss}", DateTime.Now)}";
        }        

        private void StartDataServerProcess()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    InitUIControl();
                    //this._dsProcess = new DataServerProcess(this);
                    this._dsProcess.StartProcess();
                }));
            }
            else
            {
                InitUIControl();
                //this._dsProcess = new DataServerProcess(this);
                this._dsProcess.StartProcess();
            }    
        }

        private void InitUIControl()
        {
            this._fixSplitterDistance = splitContainerMain.SplitterDistance;
            lblSystemInfo.Text = $"System communication | UART | ... | ...bps [{CommStates.Disconnected}]";
            pnlState.BackColor = Color.LightGray;
            trvRootDeviceConfig.TreeViewNodeSorter = new TreeViewNodeSorter();
            if (Directory.Exists(PATH_MANAGER.appImageDir))
            {
                var _images = Directory.GetFiles(PATH_MANAGER.appImageDir);
                if (_images.Length > 0 )
                {
                    foreach (var image in _images)
                    {
                        if (Path.GetFileName(image).ToLower().Contains("drone"))
                            picDrone.Image = Image.FromFile(image);

                        if (Path.GetFileName(image).ToLower().Contains("mcu"))
                            picMCU.Image = Image.FromFile(image);
                    }    
                }
                
            }            
        }

        #region File Tab Menu
        private void fileTab_NewConfig_Click(object sender, EventArgs e)
        {
            AddNewConfig();
        }

        private void filetab_OpenConfig_Click(object sender, EventArgs e)
        {
            OpenConfig();
        }

        private void fileTab_SaveConfig_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void fileTab_SaveAsConfig_Click(object sender, EventArgs e)
        {
            SaveAsConfig();
        }

        private void fileTab_Exit_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }
        #endregion

        #region Communication Tab Function
        private void commTab_ConnectMCU_Click(object sender, EventArgs e)
        {
            ConnectMCU();
        }

        private void commTab_DisconnectMCU_Click(object sender, EventArgs e)
        {
            DisconnectMCU();
        }
        #endregion  

        #region Devices Tab Menu
        private void devicesTab_Scan_Click(object sender, EventArgs e)
        {
            OpenScanDevice();
        }

        private void devicesTab_Manage_Click(object sender, EventArgs e)
        {
            OpenManageDevice();
        }
        #endregion

        #region Utilities Tab Menu
        private void utilitiesTab_VoltRegulator_Click(object sender, EventArgs e)
        {
            this._dsProcess.OpenVoltageRegulator();
        }

        private void utilitiesTab_DroneConfig_Click(object sender, EventArgs e)
        {
            this._dsProcess.OpenManageDrone();
        }
        #endregion

        #region Firmware Tab Menu
        private void aboutFirmwareToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void firmwareTab_Update_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Config Popup Menu
        private void rootConfigMenu_ShowInfo_Click(object sender, EventArgs e)
        {
            EditConfigInfo();
        }

        private void rootConfigMenu_Read_Click(object sender, EventArgs e)
        {
            this._dsProcess.ReadDeviceConfig();
        }

        private void rootConfigMenu_Send_Click(object sender, EventArgs e)
        {
            this._dsProcess.SendDeviceConfig();
        }

        private void rootConfigMenu_Open_Click(object sender, EventArgs e)
        {
            OpenConfig();
        }

        private void rootConfigMenu_Save_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void rootConfigMenu_SaveAs_Click(object sender, EventArgs e)
        {
            SaveAsConfig();
        }

        private void rootConfigMenu_Refresh_Click(object sender, EventArgs e)
        {
            RefreshDeviceConfig();
        }

        private void devConfigMenu_Edit_Click(object sender, EventArgs e)
        {
            EditDevice(this._dsProcess.DeviceManager.Selected, SourceDeviceModes.Config);
        }

        private void devConfigMenu_Delete_Click(object sender, EventArgs e)
        {
            DeleteDevice(this._dsProcess.DeviceManager.Selected, SourceDeviceModes.Config);
        }

        private void eventLogsMenu_Clear_Click(object sender, EventArgs e)
        {
            this.lsvEventLogs.Items.Clear();
        }

        private void changeImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeImage("Drone", picDrone);
        }

        private void changeImageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ChangeImage("Mcu", picMCU);
        }

        private void changeColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeColor(pnlMCU);
        }
        #endregion

        #region Control Event
        private void rootConfigPopupMenu_Opening(object sender, CancelEventArgs e)
        {
            if (trvRootDeviceConfig.Nodes.Count == 0)
            {
                rootConfigMenu_Read.Enabled = false;
                rootConfigMenu_Send.Enabled = false;
                rootConfigMenu_Open.Enabled = true;
                rootConfigMenu_Save.Enabled = false;
            }
            else
            {
                rootConfigMenu_Read.Enabled = true;
                rootConfigMenu_Send.Enabled = true;
                rootConfigMenu_Open.Enabled = true;
                rootConfigMenu_Save.Enabled = true;
            }
        }

        private void trvRootDeviceConfig_AfterSelect(object sender, TreeViewEventArgs e)
        {            
            if (e.Node.ImageKey == TreeNodeDefine.CONFIG_NODE_NAME)
            {
                this._dsProcess.DeviceManager.Selected = null;
                pgvConfigProperties.SelectedObject = e.Node.Tag;                

                trvRootDeviceConfig.ContextMenuStrip = rootConfigPopupMenu;
            }
            else if (e.Node.ImageKey == TreeNodeDefine.DEVICE_NODE_NAME)
            {
                var _nodeId = Convert.ToByte(e.Node.Name);
                if (this._dsProcess.ConfigManager.Model.VirtualDevices.ContainsKey(_nodeId))
                {
                    this._dsProcess.DeviceManager.Selected = this._dsProcess.ConfigManager.Model.VirtualDevices[_nodeId];
                    pgvConfigProperties.SelectedObject = e.Node.Tag;
                }

                trvRootDeviceConfig.ContextMenuStrip = devConfigPoupMenu;
            }
            else
            {
                trvRootDeviceConfig.ContextMenuStrip = null;
            }    
        }

        private bool isDoubleClick = false;

        private void trvRootDeviceConfig_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (isDoubleClick && e.Action == TreeViewAction.Collapse)
                e.Cancel = true;
        }

        private void trvRootDeviceConfig_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {

        }

        private void trvRootDeviceConfig_MouseDown(object sender, MouseEventArgs e)
        {
            isDoubleClick = e.Clicks > 1;
        }

        private void trvRootDeviceConfig_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.ImageKey == TreeNodeDefine.DEVICE_NODE_NAME)
            {
                var _nodeId = Convert.ToByte(e.Node.Name);
                if (this._dsProcess.ConfigManager.Model.VirtualDevices.ContainsKey(_nodeId))
                {
                    this._dsProcess.DeviceManager.Selected = this._dsProcess.ConfigManager.Model.VirtualDevices[_nodeId];
                    EditDevice(this._dsProcess.DeviceManager.Selected, SourceDeviceModes.Config);

                }                
            }
            else if (e.Node.ImageKey == TreeNodeDefine.END_NODE_NAME)
            {
                OpenManageDevice();                
            }
            else
            {
                this._dsProcess.DeviceManager.Selected = null;
                EditConfigInfo();
            }    
        }

        private void UIMain_SizeChanged(object sender, EventArgs e)
        {
            AutoResizeAreaLogs();
        }

        private void splitContainerMain_SplitterMoved(object sender, SplitterEventArgs e)
        {
            AutoResizeAreaLogs();
        }

        private void splitContainerMain_DoubleClick(object sender, EventArgs e)
        {
            splitContainerMain.SplitterDistance = this._fixSplitterDistance;
        }

        private void splitContainerLeft_DoubleClick(object sender, EventArgs e)
        {
            splitContainerLeft.SplitterDistance = splitContainerRight.SplitterDistance;
        }

        private void splitContainerRight_DoubleClick(object sender, EventArgs e)
        {
            splitContainerRight.SplitterDistance = splitContainerLeft.SplitterDistance;
        }

        private void splitContainerLeft_SplitterMoved(object sender, SplitterEventArgs e)
        {
            
        }

        private void splitContainerRight_SplitterMoved(object sender, SplitterEventArgs e)
        {
            
        }

        private void UIMain_KeyDown(object sender, KeyEventArgs e)
        {
            //--- New Config
            if (e.Control && (e.KeyCode == Keys.N))
                AddNewConfig();

            //--- Open Config
            if (e.Control && (e.KeyCode == Keys.O))
                OpenConfig();

            //--- Save Config
            if (e.Control && (e.KeyCode == Keys.S))
                SaveConfig();

            //--- Save As Config
            if (e.Control && e.Shift && (e.KeyCode == Keys.S))
                SaveAsConfig();
        }

        private void btnReadConfig_Click(object sender, EventArgs e)
        {
            this._dsProcess.ReadDeviceConfig();
        }

        private void btnSendConfig_Click(object sender, EventArgs e)
        {
            this._dsProcess.SendDeviceConfig();
        }

        private void btnScanDevice_Click(object sender, EventArgs e)
        {
            DeviceResult cc = new DeviceResult(new ConfigModel(), new ConfigManager());
            cc.ShowDialog();
            //OpenScanDevice();
        }
        #endregion

        #region Utilities Functions
        public void GetLogs(LogContent logContent)
        {
            if (this.InvokeRequired)
            {
                DelegateGetEventLogs d = new DelegateGetEventLogs(GetLogs);
                this.Invoke(d, logContent);
            }
            else
            {
                LogUtils.ShowLog(logContent.EventLog);
                this.lsvEventLogs.Items.Add(logContent.EventLog);
                this.lsvEventLogs.Items[this.lsvEventLogs.Items.Count - 1].EnsureVisible();
            }
        }

        public void ShowCommunicationStatus(bool status)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {                    
                    Color _color = status ? Color.LimeGreen : Color.LightGray;
                    string _state = status ? CommStates.Connected.ToString() : CommStates.Disconnected.ToString();
                    pnlState.BackColor = _color;
                    if (this._dsProcess.ConnectionManager.CommParm != null)
                    {
                        var _port = this._dsProcess.ConnectionManager.CommParm.ComPort.ToString();
                        var _baudrate = this._dsProcess.ConnectionManager.CommParm.Baudrate.ToString();
                        lblSystemInfo.Text = $"System communication | UART | {_port} | {_baudrate}bps [{_state}]";
                    }
                }));
            }
            else
            {
                Color _color = status ? Color.LimeGreen : Color.LightGray;
                string _state = status ? CommStates.Connected.ToString() : CommStates.Disconnected.ToString();
                pnlState.BackColor = _color;
                if (this._dsProcess.ConnectionManager.CommParm != null)
                {
                    var _port = this._dsProcess.ConnectionManager.CommParm.ComPort.ToString();
                    var _baudrate = this._dsProcess.ConnectionManager.CommParm.Baudrate.ToString();
                    lblSystemInfo.Text = $"System communication | UART | {_port} | {_baudrate}bps [{_state}]";
                }
            }
        }

        public void ShowDeviceResult(ConfigModel model, ConfigManager configManager)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    DeviceResult _frm = new DeviceResult(model, configManager);
                    if (_frm.ShowDialog() == DialogResult.OK)
                    {
                        this._dsProcess.BuildTreeNodeDevices(trvRootDeviceConfig, this._dsProcess.ConfigManager.Model, true);
                    }
                }));
            }
            else
            {
                DeviceResult _frm = new DeviceResult(model, configManager);
                if (_frm.ShowDialog() == DialogResult.OK)
                {
                    this._dsProcess.BuildTreeNodeDevices(trvRootDeviceConfig, this._dsProcess.ConfigManager.Model, true);
                }
            }
        }

        public void ShowDroneSide(ConfigManager configManager)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    if (configManager.Model.VirtualDevices.ContainsKey(1))
                        txtDroneSide1.Text = $"Id: {configManager.Model.VirtualDevices[1].Id} | {configManager.Model.VirtualDevices[1].DeviceCommType}";
                    else
                        txtDroneSide1.Text = $"Invalid";

                    if (configManager.Model.VirtualDevices.ContainsKey(2))
                        txtDroneSide2.Text = $"Id: {configManager.Model.VirtualDevices[2].Id} | {configManager.Model.VirtualDevices[2].DeviceCommType}";
                    else
                        txtDroneSide2.Text = $"Invalid";
                }));
            }
            else
            {
                if (configManager.Model.VirtualDevices.ContainsKey(1))
                    txtDroneSide1.Text = $"Id: {configManager.Model.VirtualDevices[1].Id} | {configManager.Model.VirtualDevices[1].DeviceCommType}";
                else
                    txtDroneSide1.Text = $"Invalid";

                if (configManager.Model.VirtualDevices.ContainsKey(2))
                    txtDroneSide2.Text = $"Id: {configManager.Model.VirtualDevices[2].Id} | {configManager.Model.VirtualDevices[2].DeviceCommType}";
                else
                    txtDroneSide2.Text = $"Invalid";
            }
        }

        public void ShowScanDevice(DeviceManager deviceManager)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => ShowScanDeviceConfig(deviceManager)));
            else
                ShowScanDeviceConfig(deviceManager);
        }

        private void ShowScanDeviceConfig(DeviceManager deviceManager)
        {
            if (deviceManager.DeviceScan != null)
            {
                if (deviceManager.DeviceScan.Devices.Count > 0 && deviceManager.DeviceScan.Devices[0] != null)
                {
                    var _devOnboard1 = deviceManager.DeviceScan.Devices[0];
                    txtDev1Name.Text = _devOnboard1.Name;
                    txtDev1Id.Text = _devOnboard1.Id.ToString();
                    txtDev1Volt.Text = _devOnboard1.Voltage.ToString();
                    txtDev1Pwm.Text = _devOnboard1.Pwm.ToString();
                    txtDev1Comm.Text = _devOnboard1.DeviceCommType.ToString();
                    txtDev1Info.Text = DevicePositions.On_Board.ToString();
                }
                else
                {
                    txtDev1Name.Text = "";
                    txtDev1Id.Text = "";
                    txtDev1Volt.Text = "";
                    txtDev1Pwm.Text = "";
                    txtDev1Comm.Text = "";
                    txtDev1Info.Text = DevicePositions.Out_Board.ToString();
                }

                if (deviceManager.DeviceScan.Devices.Count > 1 && deviceManager.DeviceScan.Devices[1] != null)
                {
                    var _devOnboard2 = deviceManager.DeviceScan.Devices[1];
                    txtDev2Name.Text = _devOnboard2.Name;
                    txtDev2Id.Text = _devOnboard2.Id.ToString();
                    txtDev2Volt.Text = _devOnboard2.Voltage.ToString();
                    txtDev2Pwm.Text = _devOnboard2.Pwm.ToString();
                    txtDev2Comm.Text = _devOnboard2.DeviceCommType.ToString();
                    txtDev2Info.Text = DevicePositions.On_Board.ToString();
                }
                else
                {
                    txtDev2Name.Text = "";
                    txtDev2Id.Text = "";
                    txtDev2Volt.Text = "";
                    txtDev2Pwm.Text = "";
                    txtDev2Comm.Text = "";
                    txtDev2Info.Text = DevicePositions.Out_Board.ToString();
                }
            }
            
        }

        private void ExitApplication()
        {
            try
            {
                var result = MessageBox.Show(this, "Do you want to exit Drone Application", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                    Application.Exit();
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
            }
        }

        private void AddNewWorkspace()
        {
            try
            {
                trvRootDeviceConfig.Nodes.Clear();
                pgvConfigProperties.SelectedObject = null;
                TreeViewBehavior();
                LogUtils.AddLog($"Add new workspace successfully", LogTypes.Info);
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
            }
        }

        private void TreeViewBehavior()
        {
            if (trvRootDeviceConfig.Nodes.Count == 0)
            {
                trvRootDeviceConfig.ContextMenuStrip = null;
                trvRootDeviceConfig.ContextMenuStrip = this.rootConfigPopupMenu;
            }
        }           

        private void AutoResizeAreaLogs()
        {
            this.lsvEventLogs.Columns[0].Width = this.lsvEventLogs.Width - 22;
        }

        private void EditConfigInfo()
        {
            this._dsProcess.EditConfigInfo(trvRootDeviceConfig);   
        }

        private void AddNewConfig()
        {
            this._dsProcess.AddNewConfig();
            this._dsProcess.BuildTreeNodeDevices(trvRootDeviceConfig, this._dsProcess.ConfigManager.Model, true);
        }

        private void OpenConfig()
        {
            if (this._dsProcess.OpenManageConfig())
                this._dsProcess.BuildTreeNodeDevices(trvRootDeviceConfig, this._dsProcess.ConfigManager.Model, true);
        }

        private void SaveConfig()
        {
            if (this._dsProcess.SaveConfig())
                this._dsProcess.BuildTreeNodeDevices(trvRootDeviceConfig, this._dsProcess.ConfigManager.Model, true);
        }

        private void SaveAsConfig()
        {
            this._dsProcess.SaveAsConfig();
        }

        private void RefreshDeviceConfig()
        {
            this._dsProcess.RefreshDeviceConfig(trvRootDeviceConfig, true);
        }

        private void EditDevice(DeviceConfig deviceConfig, SourceDeviceModes srcDeviceMode)
        {
            var _device = this._dsProcess.EditDevice(trvRootDeviceConfig, deviceConfig, srcDeviceMode);
            if (_device != null)
                pgvConfigProperties.SelectedObject = _device;
        }

        private void DeleteDevice(DeviceConfig deviceConfig, SourceDeviceModes srcDeviceMode)
        {
            this._dsProcess.DeleteDevice(trvRootDeviceConfig, deviceConfig, srcDeviceMode);
        }

        private void OpenManageDevice()
        {
            var _node = trvRootDeviceConfig.Nodes[0];
            this._dsProcess.OpenManageDevice(_node);
        }

        private void OpenScanDevice()
        {
            var _node = trvRootDeviceConfig.Nodes[0];
            this._dsProcess.OpenScanDevice(_node);
        }

        private void ConnectMCU()
        {
            this._dsProcess.ConnectMCU();
        }

        private void DisconnectMCU()
        {
            this._dsProcess.DisconnectMCU();
        }

        private void ChangeImage(string name, PictureBox pictureBox)
        {
            try
            {
                var _imageFile = HelperUtils.GetFilePath("Change Picture", FilterTypes.IMAGE_FORMAT);
                if (!string.IsNullOrEmpty(_imageFile))
                {
                    pictureBox.Image = Image.FromFile(_imageFile);
                    var _imageName = Path.GetFileName(_imageFile);
                    var _imageNewName = $"{name}{Path.GetExtension(_imageName)}";

                    if (!Directory.Exists(PATH_MANAGER.appImageDir))
                        Directory.CreateDirectory(PATH_MANAGER.appImageDir);

                    if (Directory.Exists(PATH_MANAGER.appImageDir))
                    {
                        var _imageSavePath = PATH_MANAGER.appImageDir + _imageNewName;
                        pictureBox.Image.Save(_imageSavePath);
                    }
                }
            }
            catch { }
        }

        private void ChangeColor(BunifuShadowPanel uiControl)
        {
            ColorDialog _cld = new ColorDialog();
            if (_cld.ShowDialog() == DialogResult.OK)
                uiControl.PanelColor = _cld.Color;
        }
        #endregion        
    }
}

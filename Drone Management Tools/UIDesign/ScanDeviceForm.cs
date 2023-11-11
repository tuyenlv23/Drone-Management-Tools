using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.Organizer;
using Drone_Management_Tools.Utilities;

namespace Drone_Management_Tools.UIDesign
{
    public partial class ScanDeviceForm : DevExpress.XtraEditors.XtraForm
    {
        private DataServerProcess _dsProcess;
        private ConfigManager _configManager;
        private DeviceManager _deviceManager;

        private DeviceConfig _scanSelected;
        private Dictionary<byte, DeviceConfig> _scanSelecteds;
        TreeNode _scanNode;

        private TreeNode _parentNode;
        private bool _isLibraryManager;

        private int _fixSplitterDistanceVertical;
        private int _fixSplitterDistanceHorizontal;

        public ScanDeviceForm()
        {
            InitializeComponent();
        }

        public ScanDeviceForm(DataServerProcess dataServerProcess, TreeNode parentNode)
        {
            InitializeComponent();

            CalculateFormSize();

            this._dsProcess = dataServerProcess;
            this._parentNode = (TreeNode)parentNode.Clone();
            this._configManager = this._dsProcess.ConfigManager;
            this._deviceManager = this._dsProcess.DeviceManager;
            this._scanSelecteds = new Dictionary<byte, DeviceConfig>();
        }

        private void ScanDeviceForm_Load(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => InitData()));
            else
                InitData();
        }

        private void InitData()
        {            
            trvConfigManager.TreeViewNodeSorter = new TreeViewNodeSorter();
            trvLibraryManager.TreeViewNodeSorter = new TreeViewNodeSorter();
            
            tabControl.SelectedIndex = 0;
            this._isLibraryManager = true;
            btnAddToList.Enabled = false;
            BuildTreeNodeDeviceScan(trvDeviceScan);
            ValidateTreeNodeConfig(this._parentNode);
            BuildTreeNodeDeviceConfig(trvConfigManager, this._parentNode);
            BuildTreeNodeDeviceLibrary(trvLibraryManager, this._deviceManager.DeviceLibrary);

            this._fixSplitterDistanceVertical = splitContainerTop.SplitterDistance;
            this._fixSplitterDistanceHorizontal = splitContainerConfig.SplitterDistance;
        }

        private void CalculateFormSize()
        {
            var winScale = HelperUtils.GetScalingFactor();
            var frmScaleWidth = (int)Math.Round(this.Width / winScale);
            var frmScaleHeight = (int)Math.Round(this.Height / winScale);
            this.Size = new System.Drawing.Size(frmScaleWidth, frmScaleHeight);
        }

        #region Tree Node event
        public void BuildTreeNodeDeviceScan(TreeView treeView)
        {
            this._dsProcess.BuildTreeNodeDeviceScan(treeView);
        }

        private void ValidateTreeNodeConfig(TreeNode parentNode)
        {
            TreeNode _endNode = new TreeNode();
            if (parentNode.Nodes.Count > 0)
                _endNode = parentNode.Nodes[parentNode.Nodes.Count - 1];

            if (_endNode.Text.Contains("..."))
                parentNode.Nodes.Remove(_endNode);
            else
            {
                //--- Add End Node
                TreeNode _newEndNode = new TreeNode();
                _newEndNode.Text = $"...";
                _newEndNode.Tag = null;
                _newEndNode.ImageKey = TreeNodeDefine.END_NODE_NAME;
                _newEndNode.SelectedImageKey = TreeNodeDefine.END_NODE_NAME;
                parentNode.Nodes.Add(_newEndNode);
            }
        }

        private void UpdateTreeNodeConfigToMain()
        {
            if (trvConfigManager.Nodes.Count > 0)
            {
                var _treeNode = (TreeNode)trvConfigManager.Nodes[0].Clone();
                ValidateTreeNodeConfig(_treeNode);
                BuildTreeNodeDeviceConfig(this._dsProcess._uiMain.trvRootDeviceConfig, _treeNode);
            }
        }

        private void BuildTreeNodeDeviceConfig(TreeView treeView, TreeNode treeNode)
        {
            this._dsProcess.BuildTreeNodeDevices(treeView, treeNode);
        }

        private void BuildTreeNodeDeviceConfig(TreeView treeView, ConfigModel configModel, bool isAddEndNod)
        {
            this._dsProcess.BuildTreeNodeDevices(treeView, configModel, isAddEndNod);
        }

        private void BuildTreeNodeDeviceLibrary(TreeView treeView, DeviceLibrary deviceLibrary)
        {
            this._dsProcess.BuildTreeNodeDevices(treeView, deviceLibrary);
        }

        private void RebuildTreeNodeDevices(TreeView treeView, DeviceConfig deviceConfig, ActionModes actionMode)
        {
            this._dsProcess.RebuildTreeNodeDevices(treeView, deviceConfig, actionMode);
        }
        #endregion        

        #region Scan Control Event
        private void rootScanMenu_UncheckAll_Click(object sender, EventArgs e)
        {
            if (trvDeviceScan.Nodes.Count > 0)
            {
                var _rootNode = trvDeviceScan.Nodes[0];
                _rootNode.Checked = false;
                //if (_rootNode.Nodes.Count > 0)
                //{
                //    foreach (var node in _rootNode.Nodes)
                //    {
                //        var _devNode = (TreeNode)node;
                //        _devNode.Checked = false;
                //    }
                //}
            }
        }

        private void rootScanMenu_Refresh_Click(object sender, EventArgs e)
        {
            RefreshDeviceScan();
        }

        private void rootScanMenu_Save_Click(object sender, EventArgs e)
        {
            SaveDeviceScan();
        }

        private void btnSaveDeviceScan_Click(object sender, EventArgs e)
        {
            SaveDeviceScan();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            ScanDevice();
        }

        private void deviceScanMenu_Edit_Click(object sender, EventArgs e)
        {
            EditDevice(trvDeviceScan, this._scanSelected, SourceDeviceModes.Scan);
        }

        private void btnRefreshScan_Click(object sender, EventArgs e)
        {
            RefreshDeviceScan();
        }

        private void btnEditDeviceScan_Click(object sender, EventArgs e)
        {
            EditDevice(trvDeviceScan, this._scanSelected, SourceDeviceModes.Scan);
        }

        private void deviceScanMenu_Delete_Click(object sender, EventArgs e)
        {
            DeleteDevice(trvDeviceScan);
        }

        private void btnDeleteDeviceScan_Click(object sender, EventArgs e)
        {
            DeleteDevice(trvDeviceScan);
        }

        private void trvDeviceScan_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                if (e.Node.ImageKey == TreeNodeDefine.CONFIG_NODE_NAME)
                {
                    if (e.Node.Checked)
                    {
                        if (e.Node.Nodes.Count > 0)
                        {
                            foreach (var node in e.Node.Nodes)
                            {
                                var _devNode = (TreeNode)node;
                                _devNode.Checked = true;
                            }
                        }
                    }
                    else
                    {
                        if (e.Node.Nodes.Count > 0)
                        {
                            foreach (var node in e.Node.Nodes)
                            {
                                var _devNode = (TreeNode)node;
                                _devNode.Checked = false;
                            }
                        }
                    }
                }
                else
                {
                    var _nodeId = Convert.ToByte(e.Node.Name);
                    if (e.Node.Checked)
                    {
                        if (!this._scanSelecteds.ContainsKey(_nodeId))
                        {
                            var _devConfig = this._deviceManager.DeviceScan.VirtualDevices[_nodeId];
                            this._scanSelecteds.Add(_nodeId, _devConfig);
                        }
                    }
                    else
                    {
                        if (this._scanSelecteds.ContainsKey(_nodeId))
                            this._scanSelecteds.Remove(_nodeId);
                    }
                }
            }
        }

        private void trvDeviceScan_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                if (e.Node.ImageKey == TreeNodeDefine.CONFIG_NODE_NAME)
                {
                    this._scanSelected = null;
                    trvDeviceScan.ContextMenuStrip = rootScanMenu;
                }
                else
                {
                    var _nodeId = Convert.ToByte(e.Node.Name);
                    if (this._deviceManager.DeviceScan.VirtualDevices.ContainsKey(_nodeId))
                        this._scanSelected = this._deviceManager.DeviceScan.VirtualDevices[_nodeId];

                    if (this._deviceManager.DeviceLibrary.VirtualDevices.ContainsKey(_nodeId))
                        btnAddToList.Enabled = false;
                    else
                        btnAddToList.Enabled = true;

                    prgDeviceScan.SelectedObject = e.Node.Tag;
                    trvDeviceScan.ContextMenuStrip = deviceScanMenu;
                    this._scanNode = e.Node;
                }
            }
            else
                this._scanSelected = null;
        }

        private void trvDeviceScan_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.ImageKey == TreeNodeDefine.DEVICE_NODE_NAME)
            {
                var _nodeId = Convert.ToByte(e.Node.Name);
                if (this._deviceManager.DeviceScan.VirtualDevices.ContainsKey(_nodeId))
                {
                    this._scanSelected = this._deviceManager.DeviceScan.VirtualDevices[_nodeId];
                    EditDevice(trvDeviceScan, this._scanSelected, SourceDeviceModes.Scan);
                }
            }
            else
            {
                this._scanSelected = null;
            }
        }

        private void trvDeviceScan_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
        }
        #endregion

        #region Library Control Event
        private void rootLibraryMenu_Refresh_Click(object sender, EventArgs e)
        {
            RefreshLibrary();
        }

        private void rootLibraryMenu_Save_Click(object sender, EventArgs e)
        {
            SaveLibrary();
        }       

        private void trvLibraryManager_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                if (e.Node.ImageKey == TreeNodeDefine.LIBRARY_NODE_NAME)
                {
                    trvLibraryManager.ContextMenuStrip = rootLibraryMenu;
                }
                else
                {
                    prgConfigLibrary.SelectedObject = e.Node.Tag;
                    trvLibraryManager.ContextMenuStrip = null;
                }
            }
        }

        private void trvLibraryManager_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            btnAddToList.Enabled = false;
        }

        private void btnRefreshLibrary_Click(object sender, EventArgs e)
        {
            RefreshLibrary();
        }

        private void btnSaveLibrary_Click(object sender, EventArgs e)
        {
            SaveLibrary();
        }
        #endregion

        #region Config Control Event
        private void rootConfigMenu_Refresh_Click(object sender, EventArgs e)
        {
            RefreshConfig();
        }

        private void rootConfigMenu_Save_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void trvConfigManager_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                if (e.Node.ImageKey == TreeNodeDefine.CONFIG_NODE_NAME)
                {
                    trvConfigManager.ContextMenuStrip = rootConfigMenu;
                }
                else
                {
                    prgConfigLibrary.SelectedObject = e.Node.Tag;
                    trvConfigManager.ContextMenuStrip = null;
                }
            }
        }

        private void trvConfigManager_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            btnAddToList.Enabled = false;
        }

        private void btnRefreshConfig_Click(object sender, EventArgs e)
        {
            RefreshConfig();
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }
        #endregion

        #region Control Event
        private void splitContainerConfig_DoubleClick(object sender, EventArgs e)
        {
            splitContainerConfig.SplitterDistance = this._fixSplitterDistanceHorizontal;
        }

        private void splitContainerConfig_SplitterMoved(object sender, SplitterEventArgs e)
        {
            splitContainerLibrary.SplitterDistance = splitContainerConfig.SplitterDistance;
        }

        private void splitContainerLibrary_DoubleClick(object sender, EventArgs e)
        {
            splitContainerLibrary.SplitterDistance = this._fixSplitterDistanceHorizontal;
        }

        private void splitContainerLibrary_SplitterMoved(object sender, SplitterEventArgs e)
        {
            splitContainerConfig.SplitterDistance = splitContainerLibrary.SplitterDistance;
        }

        private void splitContainerTop_DoubleClick(object sender, EventArgs e)
        {
            splitContainerTop.SplitterDistance = this._fixSplitterDistanceVertical;
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
                this._isLibraryManager = true;
            else
                this._isLibraryManager = false;
        }

        private void btnAddToList_Click(object sender, EventArgs e)
        {
            AddDeviceToList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //this._configManager.Model.ValidateDevices();
            this._deviceManager.DeviceLibrary.ValidateDevices();
            this.Close();
        }
        #endregion

        #region Utilities Function
        private void ScanDevice()
        {
            this._dsProcess.ScanDevice(trvDeviceScan);
        }

        public void RefreshDeviceScan()
        {
            this._deviceManager.DeviceScan.ValidateDevices();
            BuildTreeNodeDeviceScan(trvDeviceScan);
        }

        private void SaveDeviceScan()
        {
            if (this._deviceManager.DeviceScan != null)
                this._deviceManager.DeviceScan.Save();
            BuildTreeNodeDeviceScan(trvDeviceScan);
        }

        private void AddDeviceToList()
        {
            if (this._isLibraryManager)
                AddDeviceToLibrary();
            else
                AddDeviceToConfig();
        }

        private void AddDeviceToLibrary()
        {
            if (this._scanSelecteds.Count > 0)
            {
                foreach (var device in this._scanSelecteds.Values)
                {
                    if (device != null)
                    {
                        if (!this._deviceManager.DeviceLibrary.DeviceIds.Contains(device.Id))
                            TransferDeviceToLibrary(device, ActionModes.Add_Device);
                        else
                        {
                            var _dialogResult = MessageBox.Show($"The device [name: {device.Name} | id: {device.Id}] is existed!\n" +
                                $"Do you want to overwrite?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (_dialogResult == DialogResult.Yes)
                                TransferDeviceToLibrary(device, ActionModes.Edit_Device);
                        }
                    }
                }
            }
            else
            {
                if (this._scanSelected != null)
                {
                    if (!this._deviceManager.DeviceLibrary.DeviceIds.Contains(this._scanSelected.Id))
                        TransferDeviceToLibrary(this._scanSelected, ActionModes.Add_Device);
                    else
                    {
                        var _dialogResult = MessageBox.Show($"The device [name: {this._scanSelected.Name} | id: {this._scanSelected.Id}] is existed!\n" +
                            $"Do you want to overwrite?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (_dialogResult == DialogResult.Yes)
                            TransferDeviceToLibrary(this._scanSelected, ActionModes.Edit_Device);
                    }
                }
            }
        }
       
        private void TransferDeviceToLibrary(DeviceConfig device, ActionModes actionMode)
        {
            this._deviceManager.DeviceLibrary.UpdateDevice(device);
            RebuildTreeNodeDevices(trvLibraryManager, device, actionMode);
            var _msg = $"The device [name: {device.Name} | id: {device.Id}] has been updated to library " +
                $"[name: {this._deviceManager.DeviceLibrary.VirtualLibraryInfo.Name}]";
            //MessageBox.Show(_msg);
            LogUtils.AddLog(_msg, LogTypes.Info);
        }

        public void RefreshLibrary()
        {
            this._dsProcess.RefreshDeviceLibrary(trvLibraryManager);
        }

        private void SaveLibrary()
        {
            this._dsProcess.SaveLibrary();
            BuildTreeNodeDeviceLibrary(trvLibraryManager, this._deviceManager.DeviceLibrary);
        }        

        private void AddDeviceToConfig()
        {
            if (this._scanSelecteds.Count > 0)
            {
                foreach (var device in this._scanSelecteds.Values)
                {
                    if (device != null)
                    {
                        if (!this._configManager.Model.DeviceIds.Contains(device.Id))
                            TransferDeviceToConfig(device, ActionModes.Add_Device);
                        else
                        {
                            var _dialogResult = MessageBox.Show($"The device [name: {device.Name} | id: {device.Id}] is existed!\n" +
                                $"Do you want to overwrite?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (_dialogResult == DialogResult.Yes)
                                TransferDeviceToConfig(device, ActionModes.Edit_Device);
                        }
                    }
                }
            }
            else
            {
                if (this._scanSelected != null)
                {
                    if (!this._configManager.Model.DeviceIds.Contains(this._scanSelected.Id))
                        TransferDeviceToConfig(this._scanSelected, ActionModes.Add_Device);
                    else
                    {
                        var _dialogResult = MessageBox.Show($"The device [name: {this._scanSelected.Name} | id: {this._scanSelected.Id}] is existed!\n" +
                            $"Do you want to overwrite?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (_dialogResult == DialogResult.Yes)
                            TransferDeviceToConfig(this._scanSelected, ActionModes.Edit_Device);
                    }
                }
            }
        }

        private void TransferDeviceToConfig(DeviceConfig device, ActionModes actionMode)
        {
            this._configManager.Model.UpdateDevice(device);
            RebuildTreeNodeDevices(trvConfigManager, device, actionMode);
            UpdateTreeNodeConfigToMain();
            var _msg = $"The device [name: {device.Name} | id: {device.Id}] has been updated to config " +
                $"[name: {this._configManager.Model.VirtualConfigInfo.Name} | version: {this._configManager.Model.VirtualConfigInfo.Version}]";
            //MessageBox.Show(_msg);
            LogUtils.AddLog(_msg, LogTypes.Info);
        }

        private void RefreshConfig()
        {
            this._dsProcess.RefreshDeviceConfig(trvConfigManager, false);
            UpdateTreeNodeConfigToMain();
        }

        private void SaveConfig()
        {
            if (this._dsProcess.SaveConfig())
            {
                BuildTreeNodeDeviceConfig(trvConfigManager, this._configManager.Model, false);
                UpdateTreeNodeConfigToMain();
            }
        }

        private void EditDevice(TreeView treeView, DeviceConfig deviceConfig, SourceDeviceModes srcDeviceMode)
        {
            this._dsProcess.UpdateScanDevice(this);
            var _device = this._dsProcess.EditDevice(treeView, deviceConfig, srcDeviceMode);            
            if (_device != null)
            {
                this._scanSelected = _device;
                prgDeviceScan.SelectedObject = this._scanSelected;
                if (_device != null)
                {
                    this._scanSelected = _device;
                    prgDeviceScan.SelectedObject = this._scanSelected;
                }
            }
        }

        private void DeleteDevice(TreeView treeView)
        {
            if (this._scanSelecteds.Count > 0)
                this._dsProcess.DeleteDevice(treeView, this._scanSelecteds.Values.ToList(), SourceDeviceModes.Scan);
            else
                this._dsProcess.DeleteDevice(treeView, this._scanSelected, SourceDeviceModes.Scan);
        }
        #endregion        
    }
}
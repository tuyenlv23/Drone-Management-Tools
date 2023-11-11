using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.Organizer;
using Drone_Management_Tools.Utilities;

namespace Drone_Management_Tools.UIDesign
{
    public partial class ManageDeviceForm : DevExpress.XtraEditors.XtraForm
    {
        private DataServerProcess _dsProcess;
        private ConfigManager _configManager;
        private DeviceManager _deviceManager;

        private DeviceConfig _configSelected;
        private Dictionary<byte, DeviceConfig> _configSelecteds;
        private DeviceConfig _librarySelected;
        private Dictionary<byte, DeviceConfig> _librarySelecteds;

        private TreeNode _parentNode;
        private bool isDoubleClick = false;

        private int _fixSplitterDistanceVertical;
        private int _fixSplitterDistanceHorizontal;

        public ManageDeviceForm()
        {
            InitializeComponent();
        }

        public ManageDeviceForm(DataServerProcess dataServerProcess, TreeNode parentNode)
        {
            InitializeComponent();

            CalculateFormSize();

            this._parentNode = (TreeNode)parentNode.Clone();
            this._dsProcess = dataServerProcess;
            this._configManager = this._dsProcess.ConfigManager;
            this._deviceManager = this._dsProcess.DeviceManager;
            this._configSelecteds = new Dictionary<byte, DeviceConfig>();
            this._librarySelecteds = new Dictionary<byte, DeviceConfig>();
        }

        private void ManageDeviceForm_Load(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => InitData()));
            else
                InitData();
        }

        private void InitData()
        {            
            trvDeviceConfig.TreeViewNodeSorter = new TreeViewNodeSorter();
            trvDeviceLibrary.TreeViewNodeSorter = new TreeViewNodeSorter();

            ValidateTreeNodeConfig(this._parentNode);
            BuildTreeNodeDeviceConfig(trvDeviceConfig, this._parentNode);
            BuildTreeNodeDeviceLibrary(trvDeviceLibrary, this._deviceManager.DeviceLibrary);

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
            if (trvDeviceConfig.Nodes.Count > 0)
            {
                var _treeNode = (TreeNode)trvDeviceConfig.Nodes[0].Clone();
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

        #region Config Control Event
        private void rootConfigMenu_ShowInfo_Click(object sender, EventArgs e)
        {
            EditConfigInfo(trvDeviceConfig);
        }

        private void rootConfigMenu_UncheckAll_Click(object sender, EventArgs e)
        {
            if (trvDeviceConfig.Nodes.Count > 0)
            {
                var _rootNode = trvDeviceConfig.Nodes[0];
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

        private void rootConfigMenu_Refresh_Click(object sender, EventArgs e)
        {
            RefreshDeviceConfig();
        }

        private void rootConfigMenu_Save_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }              

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void btnAddToConfig_Click(object sender, EventArgs e)
        {
            AddDeviceToConfig();
        }

        private void deviceConfigMenu_Edit_Click(object sender, EventArgs e)
        {
            EditDevice(trvDeviceConfig, this._configSelected, SourceDeviceModes.Config);
        }

        private void btnEditDeviceConfig_Click(object sender, EventArgs e)
        {
            EditDevice(trvDeviceConfig, this._configSelected, SourceDeviceModes.Config);
        }

        private void deviceConfigMenu_Delete_Click(object sender, EventArgs e)
        {
            DeleteDevice(trvDeviceConfig, SourceDeviceModes.Config);
        }

        private void btnDeleteDeviceConfig_Click(object sender, EventArgs e)
        {
            DeleteDevice(trvDeviceConfig, SourceDeviceModes.Config);
        }

        private void trvDeviceConfig_AfterCheck(object sender, TreeViewEventArgs e)
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
                        if (!this._configSelecteds.ContainsKey(_nodeId))
                        {
                            var _devConfig = this._configManager.Model.VirtualDevices[_nodeId];
                            this._configSelecteds.Add(_nodeId, _devConfig);
                        }
                    }
                    else
                    {
                        if (this._configSelecteds.ContainsKey(_nodeId))
                            this._configSelecteds.Remove(_nodeId);
                    }
                }
            }
        }

        private void trvDeviceConfig_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                if (e.Node.ImageKey == TreeNodeDefine.CONFIG_NODE_NAME)
                {
                    this._configSelected = null;
                    trvDeviceConfig.ContextMenuStrip = rootConfigMenu;
                }
                else
                {
                    var _nodeId = Convert.ToByte(e.Node.Name);
                    if (this._configManager.Model.VirtualDevices.ContainsKey(_nodeId))
                        this._configSelected = this._configManager.Model.VirtualDevices[_nodeId];

                    prgConfig.SelectedObject = e.Node.Tag;
                    trvDeviceConfig.ContextMenuStrip = deviceConfigMenu;
                }
            }
            else
                this._configSelected = null;
        }

        private void trvDeviceConfig_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.ImageKey == TreeNodeDefine.DEVICE_NODE_NAME)
            {
                var _nodeId = Convert.ToByte(e.Node.Name);
                if (this._configManager.Model.VirtualDevices.ContainsKey(_nodeId))
                {
                    this._configSelected = this._configManager.Model.VirtualDevices[_nodeId];
                    EditDevice(trvDeviceConfig, this._configSelected, SourceDeviceModes.Config);
                }
            }
            else
            {
                this._configSelected = null;
                EditConfigInfo(trvDeviceConfig);
            }
        }

        private void trvDeviceConfig_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            btnAddToLibrary.Enabled = true;
            btnAddToConfig.Enabled = false;
        }

        private void trvDeviceConfig_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (isDoubleClick && e.Action == TreeViewAction.Collapse)
                e.Cancel = true;
        }

        private void trvDeviceConfig_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (isDoubleClick && e.Action == TreeViewAction.Expand)
                e.Cancel = true;
        }

        private void trvDeviceConfig_MouseDown(object sender, MouseEventArgs e)
        {
            isDoubleClick = e.Clicks > 1;
        }
        #endregion

        #region Library Control Event
        private void rootLibraryMenu_ShowInfo_Click(object sender, EventArgs e)
        {
            EditLibaryInfo(trvDeviceLibrary);
        }

        private void rootLibraryMenu_UncheckAll_Click(object sender, EventArgs e)
        {
            if (trvDeviceLibrary.Nodes.Count > 0)
            {
                var _rootNode = trvDeviceLibrary.Nodes[0];
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

        private void rootLibraryMenu_Refresh_Click(object sender, EventArgs e)
        {
            RefreshDeviceLibrary();
        }

        private void rootLibraryMenu_Save_Click(object sender, EventArgs e)
        {
            SaveLibrary();
        }

        private void btnSaveLibrary_Click(object sender, EventArgs e)
        {
            SaveLibrary();
        }

        private void btnAddToLibrary_Click(object sender, EventArgs e)
        {
            AddDeviceToLibrary();
        }

        private void rootLibraryMenu_AddDevice_Click(object sender, EventArgs e)
        {
            AddNewDevice();
        }

        private void btnAddDeviceLibrary_Click(object sender, EventArgs e)
        {
            AddNewDevice();
        }                       

        private void deviceLibraryMenu_Edit_Click(object sender, EventArgs e)
        {
            EditDevice(trvDeviceLibrary, this._librarySelected, SourceDeviceModes.Library);
        }

        private void btnEditDeviceLibrary_Click(object sender, EventArgs e)
        {
            EditDevice(trvDeviceLibrary, this._librarySelected, SourceDeviceModes.Library);
        }

        private void deviceLibraryMenu_Delete_Click(object sender, EventArgs e)
        {
            DeleteDevice(trvDeviceLibrary, SourceDeviceModes.Library);
        }

        private void btnDeleteDeviceLibrary_Click(object sender, EventArgs e)
        {
            DeleteDevice(trvDeviceLibrary, SourceDeviceModes.Library);
        }

        private void trvDeviceLibrary_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {                
                if (e.Node.ImageKey == TreeNodeDefine.LIBRARY_NODE_NAME)
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
                        if (!this._librarySelecteds.ContainsKey(_nodeId))
                        {
                            var _devConfig = this._deviceManager.DeviceLibrary.VirtualDevices[_nodeId];
                            this._librarySelecteds.Add(_nodeId, _devConfig);
                        }
                    }
                    else
                    {
                        if (this._librarySelecteds.ContainsKey(_nodeId))
                            this._librarySelecteds.Remove(_nodeId);
                    }
                }                                
            }
        }        

        private void trvDeviceLibrary_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                if (e.Node.ImageKey == TreeNodeDefine.LIBRARY_NODE_NAME)
                {
                    this._librarySelected = null;
                    trvDeviceLibrary.ContextMenuStrip = rootlibraryMenu;
                }
                else
                {
                    var _nodeId = Convert.ToByte(e.Node.Name);
                    if (this._deviceManager.DeviceLibrary.VirtualDevices.ContainsKey(_nodeId))
                        this._librarySelected = this._deviceManager.DeviceLibrary.VirtualDevices[_nodeId];

                    prgLibrary.SelectedObject = e.Node.Tag;
                    trvDeviceLibrary.ContextMenuStrip = deviceLibraryMenu;
                }
            }
            else
                this._librarySelected = null;
        }

        private void trvDeviceLibrary_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.ImageKey == TreeNodeDefine.DEVICE_NODE_NAME)
            {
                var _nodeId = Convert.ToByte(e.Node.Name);
                if (this._deviceManager.DeviceLibrary.VirtualDevices.ContainsKey(_nodeId))
                {
                    this._librarySelected = this._deviceManager.DeviceLibrary.VirtualDevices[_nodeId];
                    EditDevice(trvDeviceLibrary, this._librarySelected, SourceDeviceModes.Library);
                }
            }
            else
            {                
                this._librarySelected = null;
                EditLibaryInfo(trvDeviceLibrary);
            }
        }

        private void trvDeviceLibrary_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            btnAddToLibrary.Enabled = false;
            btnAddToConfig.Enabled = true;
        }

        private void trvDeviceLibrary_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (isDoubleClick && e.Action == TreeViewAction.Collapse)
                e.Cancel = true;
        }

        private void trvDeviceLibrary_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (isDoubleClick && e.Action == TreeViewAction.Expand)
                e.Cancel = true;
        }

        private void trvDeviceLibrary_MouseDown(object sender, MouseEventArgs e)
        {
            isDoubleClick = e.Clicks > 1;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this._deviceManager.DeviceLibrary.ValidateDevices();
            this.Close();
        }
        #endregion

        #region Utilities Function
        private void EditConfigInfo(TreeView treeView)
        {
            if (this._dsProcess.EditConfigInfo(treeView))
                UpdateTreeNodeConfigToMain();
        }                

        private void AddDeviceToConfig()
        {
            if (this._librarySelecteds.Count > 0)
            {
                foreach (var device in this._librarySelecteds.Values)
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

                UpdateTreeNodeConfigToMain();
            }
            else
            {
                if (this._librarySelected != null)
                {
                    if (!this._configManager.Model.DeviceIds.Contains(this._librarySelected.Id))
                        TransferDeviceToConfig(this._librarySelected, ActionModes.Add_Device);
                    else
                    {
                        var _dialogResult = MessageBox.Show($"The device [name: {this._librarySelected.Name} | id: {this._librarySelected.Id}] is existed!\n" +
                            $"Do you want to overwrite?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (_dialogResult == DialogResult.Yes)
                            TransferDeviceToConfig(this._librarySelected, ActionModes.Edit_Device);
                    }

                    UpdateTreeNodeConfigToMain();
                }
            }            
        }

        private void TransferDeviceToConfig(DeviceConfig device, ActionModes actionMode)
        {
            this._configManager.Model.UpdateDevice(device);
            RebuildTreeNodeDevices(trvDeviceConfig, device, actionMode);            
            var _msg = $"The device [name: {device.Name} | id: {device.Id}] has been updated to config " +
                $"[name: {this._configManager.Model.VirtualConfigInfo.Name} | version: {this._configManager.Model.VirtualConfigInfo.Version}]";
            //MessageBox.Show(_msg);
            LogUtils.AddLog(_msg, LogTypes.Info);
        }        
        
        private void SaveConfig()
        {
            if (this._dsProcess.SaveConfig())
            {
                BuildTreeNodeDeviceConfig(trvDeviceConfig, this._configManager.Model, false);
                UpdateTreeNodeConfigToMain();
            }            
        }

        private void RefreshDeviceConfig()
        {
            this._dsProcess.RefreshDeviceConfig(trvDeviceConfig, false);
            UpdateTreeNodeConfigToMain();
        }

        private void EditLibaryInfo(TreeView treeView)
        {
            this._dsProcess.EditLibraryInfo(treeView);
        }

        private void AddDeviceToLibrary()
        {
            if (this._configSelecteds.Count > 0)
            {
                foreach (var device in this._configSelecteds.Values)
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
                if (this._configSelected != null)
                {
                    if (!this._deviceManager.DeviceLibrary.DeviceIds.Contains(this._configSelected.Id))
                        TransferDeviceToLibrary(this._configSelected, ActionModes.Add_Device);
                    else
                    {
                        var _dialogResult = MessageBox.Show($"The device [name: {this._configSelected.Name} | id: {this._configSelected.Id}] is existed!\n" +
                            $"Do you want to overwrite?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (_dialogResult == DialogResult.Yes)
                            TransferDeviceToLibrary(this._configSelected, ActionModes.Edit_Device);
                    }
                }
            }
        }

        private void TransferDeviceToLibrary(DeviceConfig device, ActionModes actionMode)
        {
            this._deviceManager.DeviceLibrary.UpdateDevice(device);
            RebuildTreeNodeDevices(trvDeviceLibrary, device, actionMode);
            var _msg = $"The device [name: {device.Name} | id: {device.Id}] has been updated to library " +
                $"[name: {this._deviceManager.DeviceLibrary.VirtualLibraryInfo.Name}]";
            //MessageBox.Show(_msg);
            LogUtils.AddLog(_msg, LogTypes.Info);
        }

        private void SaveLibrary()
        {
            this._dsProcess.SaveLibrary();
            BuildTreeNodeDeviceLibrary(trvDeviceLibrary, this._deviceManager.DeviceLibrary);
        }        

        public void RefreshDeviceLibrary()
        {
            this._dsProcess.RefreshDeviceLibrary(trvDeviceLibrary);
        }

        private void AddNewDevice()
        {
            this._dsProcess.AddNewDevice(trvDeviceLibrary);
        }

        private void EditDevice(TreeView treeView, DeviceConfig deviceConfig, SourceDeviceModes srcDeviceMode)
        {
            this._dsProcess.UpdateManageDevice(this);
            var _device = this._dsProcess.EditDevice(treeView, deviceConfig, srcDeviceMode);
            if (_device != null)
            {
                switch (srcDeviceMode)
                {
                    case SourceDeviceModes.Library:
                        {
                            this._librarySelected = _device;
                            prgLibrary.SelectedObject = this._librarySelected;

                            break;
                        }
                    case SourceDeviceModes.Config:
                        {
                            this._configSelected = _device;
                            prgConfig.SelectedObject = this._configSelected;
                            UpdateTreeNodeConfigToMain();

                            break;
                        }
                }
            }
        }

        private void DeleteDevice(TreeView treeView, SourceDeviceModes srcDeviceMode)
        {
            switch (srcDeviceMode)
            {
                case SourceDeviceModes.Library:
                    {
                        if (this._librarySelecteds.Count > 0)
                            this._dsProcess.DeleteDevice(treeView, this._librarySelecteds.Values.ToList(), SourceDeviceModes.Library);
                        else
                            this._dsProcess.DeleteDevice(treeView, this._librarySelected, SourceDeviceModes.Library);

                        break;
                    }
                case SourceDeviceModes.Config:
                    {
                        if (this._configSelecteds.Count > 0)
                        {
                            if (this._dsProcess.DeleteDevice(treeView, this._configSelecteds.Values.ToList(), SourceDeviceModes.Config))
                                UpdateTreeNodeConfigToMain();
                        }
                        else
                        {
                            if (this._dsProcess.DeleteDevice(treeView, this._configSelected, SourceDeviceModes.Config))
                                UpdateTreeNodeConfigToMain();
                        }

                        break;
                    }
            }          
        }
        #endregion                     
    }
}
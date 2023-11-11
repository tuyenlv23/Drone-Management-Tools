using System;
using System.Data;
using System.Windows.Forms;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.Organizer;

namespace Drone_Management_Tools.UIDesign
{
    public partial class ConfigDeviceForm : DevExpress.XtraEditors.XtraForm
    {
        private ConfigInfo _configInfo;
        public ConfigInfo ConfigInfo => _configInfo;
        private ConfigManager _configManager;
        private byte _oldVersion;

        public ConfigDeviceForm(ConfigInfo cfgInfo, ConfigManager cfgManager)
        {
            InitializeComponent();
            this._configInfo = cfgInfo;
            this._configManager = cfgManager;
        }

        private void ConfigDeviceForm_Load(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => InitParameters()));
            else
                InitParameters();
        }

        private void InitParameters()
        {
            this._oldVersion = this._configInfo.Version;
            txtConfigVersion.Text = this._configInfo.Version.ToString();
            txtConfigName.Text = this._configInfo.Name;
            txtConfigDesc.Text = this._configInfo.Description;

            grvConfigLibrary.DataSource = GetConfigTable();
        }

        private DataTable GetConfigTable()
        {
            try
            {
                if (this._configManager.Models.Count > 0)
                {
                    DataTable _dt = new DataTable();
                    _dt.Columns.Add(new DataColumn("No."));
                    _dt.Columns.Add(new DataColumn("Id"));
                    _dt.Columns.Add(new DataColumn("Name"));

                    int _count = 0;
                    foreach (var model in this._configManager.Models)
                    {
                        _count++;
                        DataRow _dr = _dt.NewRow();
                        _dr[0] = _count;
                        _dr[1] = model.Key;
                        _dr[2] = model.Value.ConfigInfo.Name;

                        _dt.Rows.Add(_dr);
                    }

                    return _dt;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private void GetConfigInfo()
        {
            if (!string.IsNullOrEmpty(txtConfigVersion.Text))
            {
                byte _cfgVersion = Convert.ToByte(txtConfigVersion.Text);                
                if (_cfgVersion <= 0)
                {
                    MessageBox.Show("The config version must be greater than 0!", "Edit Config", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_cfgVersion != this._oldVersion)
                {
                    if (!this._configManager.ModelIds.Contains(_cfgVersion))
                    {
                        this._configInfo.Version = _cfgVersion;
                    }
                    else
                    {
                        var _dialogResult = MessageBox.Show($"The config [id: {_cfgVersion}] is existed!\nDo you want to continue?", 
                            "Edit Config", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (_dialogResult == DialogResult.Yes)
                        {
                            this._configInfo.Version = _cfgVersion;
                        }
                    }
                }
            }

            this._configInfo.Name = txtConfigName.Text;
            this._configInfo.Description = txtConfigDesc.Text;

            DialogResult = DialogResult.OK;
        }        

        private void btnChange_Click(object sender, EventArgs e)
        {
            GetConfigInfo();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void txtConfigVersion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }
    }
}
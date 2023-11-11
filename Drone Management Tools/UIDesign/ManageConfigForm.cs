using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using Drone_Management_Tools.Organizer;
using Drone_Management_Tools.Utilities;

namespace Drone_Management_Tools.UIDesign
{
    public partial class ManageConfigForm : DevExpress.XtraEditors.XtraForm
    {
        private ConfigManager _cfgManager;
        private string _configPath = "";
        public string ConfigPath => _configPath;

        public ManageConfigForm(ConfigManager cfgManager)
        {
            InitializeComponent();
            CalculateFormSize();
            this._cfgManager = cfgManager;
        }

        private void ManageConfigForm_Load(object sender, EventArgs e)
        {
            grvConfigList.DataSource = GetConfigTable();
        }

        private void CalculateFormSize()
        {
            var winScale = HelperUtils.GetScalingFactor();
            var frmScaleWidth = (int)Math.Round(this.Width / winScale);
            var frmScaleHeight = (int)Math.Round(this.Height / winScale);
            this.Size = new System.Drawing.Size(frmScaleWidth, frmScaleHeight);
        }

        private DataTable GetConfigTable()
        {
            try
            {
                if (this._cfgManager.Models.Count > 0)
                {
                    DataTable _dt = new DataTable();
                    _dt.Columns.Add(new DataColumn("No."));
                    _dt.Columns.Add(new DataColumn("Name"));
                    _dt.Columns.Add(new DataColumn("Id"));                    
                    _dt.Columns.Add(new DataColumn("Total Devices"));

                    int _count = 0;
                    foreach (var model in this._cfgManager.Models)
                    {
                        _count++;
                        DataRow _dr = _dt.NewRow();
                        _dr[0] = _count;
                        _dr[1] = model.Value.ConfigInfo.Name;
                        _dr[2] = model.Key;
                        _dr[3] = model.Value.Devices.Count;

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

        private void btnOpenOtherFile_Click(object sender, EventArgs e)
        {
            this._configPath = HelperUtils.GetFilePath("Select Configuration File", FilterTypes.JSON_FORMAT);
            if (!string.IsNullOrEmpty(this._configPath))
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Cancel;
        }        

        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)
            {
                var _configId = Convert.ToByte(view.GetRowCellValue(info.RowHandle, view.Columns[2]));
                this._cfgManager.Model = this._cfgManager.Models[_configId];
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.Organizer;
using Drone_Management_Tools.Utilities;

namespace Drone_Management_Tools.UIDesign
{
    public partial class DeviceResult : DevExpress.XtraEditors.XtraForm
    {
        private ConfigModel _configModel;
        public ConfigModel ConfigModel
        {
            get { return _configModel; }
            set { _configModel = value; }
        }
        private ConfigManager _configManager;

        public DeviceResult(ConfigModel model, ConfigManager configManager)
        {
            InitializeComponent();

            CalculateFormSize();

            this._configModel = model;
            this._configManager = configManager;
        }

        private void DeviceResult_Load(object sender, EventArgs e)
        {
            grvDeviceResult.DataSource = GetDataTable();
        }

        private void CalculateFormSize()
        {
            var winScale = HelperUtils.GetScalingFactor();
            var frmScaleWidth = (int)Math.Round(this.Width / winScale);
            var frmScaleHeight = (int)Math.Round(this.Height / winScale);
            this.Size = new System.Drawing.Size(frmScaleWidth, frmScaleHeight);
        }

        public DataTable GetDataTable()
        {
            try
            {
                DataTable _dt = new DataTable();
                _dt.Columns.Add(new DataColumn("No."));                     // 0 --- STT
                _dt.Columns.Add(new DataColumn("Name"));                    // 1 --- Name
                _dt.Columns.Add(new DataColumn("Id"));                      // 2 --- Id            
                _dt.Columns.Add(new DataColumn("Voltage"));                 // 3 --- Voltage
                _dt.Columns.Add(new DataColumn("Device Comm"));             // 4 --- Device Comm. Type
                _dt.Columns.Add(new DataColumn("Comm"));                    // 5 --- Comm. Type
                _dt.Columns.Add(new DataColumn("Pwm-1_Freq"));              // 6 --- PWM - Tmin
                _dt.Columns.Add(new DataColumn("Pwm-1_Tmin"));              // 7 --- PWM - Tmin
                _dt.Columns.Add(new DataColumn("Pwm-1_Tmax"));              // 8 --- PWM - Tmax
                _dt.Columns.Add(new DataColumn("Pwm-2_Freq"));              // 9 --- PWM - Tmin
                _dt.Columns.Add(new DataColumn("Pwm-2_Tmin"));              // 10 --- PWM - Tmin
                _dt.Columns.Add(new DataColumn("Pwm-2_Tmax"));              // 11 --- PWM - Tmax
                _dt.Columns.Add(new DataColumn("Pwm-3_Freq"));              // 12 --- PWM - Tmin
                _dt.Columns.Add(new DataColumn("Pwm-3_Tmin"));              // 13 --- PWM - Tmin
                _dt.Columns.Add(new DataColumn("Pwm-3_Tmax"));              // 14 --- PWM - Tmax
                _dt.Columns.Add(new DataColumn("Pwm-4_Freq"));              // 15 --- PWM - Tmin
                _dt.Columns.Add(new DataColumn("Pwm-4_Tmin"));              // 16 --- PWM - Tmin
                _dt.Columns.Add(new DataColumn("Pwm-4_Tmax"));              // 17 --- PWM - Tmax


                if (this._configModel.Devices != null && this._configModel.Devices.Count > 0)
                {
                    int _count = 0;
                    foreach (var device in this._configModel.Devices)
                    {
                        _count++;
                        DataRow _dr = _dt.NewRow();

                        _dr[0] = _count;
                        _dr[1] = device.Name;
                        _dr[2] = device.Id;
                        _dr[3] = device.Voltage;
                        _dr[4] = device.DeviceCommType.ToString();
                        if (device.Communications.Count > 0)
                            _dr[5] = device.Communications[0].Type.ToString();
                        else
                            _dr[5] = "";
                        _dr[6] = device.Pwm.DevicePwm1.Frequency;
                        _dr[7] = device.Pwm.DevicePwm1.Tmin;
                        _dr[8] = device.Pwm.DevicePwm1.Tmax;
                        _dr[9] = device.Pwm.DevicePwm2.Frequency;
                        _dr[10] = device.Pwm.DevicePwm2.Tmin;
                        _dr[11] = device.Pwm.DevicePwm2.Tmax;
                        _dr[12] = device.Pwm.DevicePwm3.Frequency;
                        _dr[13] = device.Pwm.DevicePwm3.Tmin;
                        _dr[14] = device.Pwm.DevicePwm3.Tmax;
                        _dr[15] = device.Pwm.DevicePwm4.Frequency;
                        _dr[16] = device.Pwm.DevicePwm4.Tmin;
                        _dr[17] = device.Pwm.DevicePwm4.Tmax;

                        _dt.Rows.Add(_dr);
                    }
                }

                return _dt;
            }
            catch
            {
                return null;
            }
        }

        private void btnCreateConfig_Click(object sender, EventArgs e)
        {
            ConfigDeviceForm _frm = new ConfigDeviceForm(this._configModel.VirtualConfigInfo, this._configManager);
            if (_frm.ShowDialog() == DialogResult.Cancel)
                return;

            this._configModel.Save();
            this._configModel.ValidateDevices();
            this._configManager.Model = this._configModel;

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
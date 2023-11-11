using System;
using System.Windows.Forms;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.Organizer;
using Drone_Management_Tools.Utilities;

namespace Drone_Management_Tools.UIDesign
{
    public partial class EditDeviceForm2 : DevExpress.XtraEditors.XtraForm
    {
        private DeviceManager _deviceManager;
        private DeviceConfig _deviceConfig;
        public DeviceConfig EditedDevice
        {
            get { return _deviceConfig; }
            set { _deviceConfig = value; }
        }

        private byte _oldId;
        public byte OldId => _oldId;

        private bool _enableEditId;
        private int _fixSplitterDistance;

        public EditDeviceForm2()
        {
            InitializeComponent();
        }

        public EditDeviceForm2(DeviceManager deviceManager, DeviceConfig deviceConfig, bool enableEditId)
        {
            InitializeComponent();

            this._oldId = deviceConfig.Id;
            this._deviceManager = deviceManager;
            this._deviceConfig = deviceConfig;
            this._enableEditId = enableEditId;
        }

        private void EditDeviceForm_Load(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => InitParameters()));
            }
            else
            {
                InitParameters();
            }
        }

        private void InitParameters()
        {
            txtDevId.Text = this._deviceConfig.Id.ToString();
            txtDevId.Enabled = this._enableEditId;
            txtDevName.Text = this._deviceConfig.Name;
            txtDevVoltage.Text = this._deviceConfig.Voltage.ToString();

            txtPwmTmin.Text = this._deviceConfig.Pwm.DevicePwm1.Tmin.ToString();
            txtPwmTmax.Text = this._deviceConfig.Pwm.DevicePwm1.Tmax.ToString();

            cbDevCommType.DataSource = Enum.GetValues(typeof(DeviceCommTypes));
            cbDevCommType.SelectedIndex = (int)this._deviceConfig.DeviceCommType;

            this._fixSplitterDistance = splitContainerBottom.SplitterDistance;
        }

        private void cbDevCommType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var _devCommType = (DeviceCommTypes)cbDevCommType.SelectedIndex;
            CommBehavior(_devCommType);
        }

        private void CommBehavior(DeviceCommTypes devCommType)
        {            
            splitContainerBottom.Panel1.Controls.Clear();
            splitContainerBottom.Panel2.Controls.Clear();
            UI_Comm_Spare _frmSpare = new UI_Comm_Spare();

            switch (devCommType)
            {
                case DeviceCommTypes.TCP:
                    {
                        UI_Comm_TCP _frm = new UI_Comm_TCP(this._deviceConfig);
                        splitContainerBottom.Panel1.Controls.Add(_frm);                        
                        splitContainerBottom.Panel2.Controls.Add(_frmSpare);

                        _frm.Dock = DockStyle.Fill;
                        _frmSpare.Dock = DockStyle.Fill;

                        break;
                    }
                case DeviceCommTypes.UART:
                    {
                        UI_Comm_UART _frm = new UI_Comm_UART(this._deviceConfig);
                        splitContainerBottom.Panel1.Controls.Add(_frm);
                        splitContainerBottom.Panel2.Controls.Add(_frmSpare);

                        _frm.Dock = DockStyle.Fill;
                        _frmSpare.Dock = DockStyle.Fill;

                        break;
                    }
                case DeviceCommTypes.I2C:
                    {
                        UI_Comm_I2C _frm = new UI_Comm_I2C(this._deviceConfig);
                        splitContainerBottom.Panel1.Controls.Add(_frm);
                        splitContainerBottom.Panel2.Controls.Add(_frmSpare);

                        _frm.Dock = DockStyle.Fill;
                        _frmSpare.Dock = DockStyle.Fill;

                        break;
                    }
                case DeviceCommTypes.CAN:
                    {
                        UI_Comm_CAN _frm = new UI_Comm_CAN(this._deviceConfig);
                        splitContainerBottom.Panel1.Controls.Add(_frm);
                        splitContainerBottom.Panel2.Controls.Add(_frmSpare);

                        _frm.Dock = DockStyle.Fill;
                        _frmSpare.Dock = DockStyle.Fill;

                        break;
                    }
                case DeviceCommTypes.TCP_UART:
                    {
                        UI_Comm_TCP _frm1 = new UI_Comm_TCP(this._deviceConfig);
                        splitContainerBottom.Panel1.Controls.Add(_frm1);
                        UI_Comm_UART _frm2 = new UI_Comm_UART(this._deviceConfig);
                        splitContainerBottom.Panel2.Controls.Add(_frm2);

                        _frm1.Dock = DockStyle.Fill;
                        _frm2.Dock = DockStyle.Fill;

                        break;
                    }
                case DeviceCommTypes.TCP_I2C:
                    {
                        UI_Comm_TCP _frm1 = new UI_Comm_TCP(this._deviceConfig);
                        splitContainerBottom.Panel1.Controls.Add(_frm1);
                        UI_Comm_I2C _frm2 = new UI_Comm_I2C(this._deviceConfig);
                        splitContainerBottom.Panel2.Controls.Add(_frm2);

                        _frm1.Dock = DockStyle.Fill;
                        _frm2.Dock = DockStyle.Fill;

                        break;
                    }
                case DeviceCommTypes.TCP_CAN:
                    {
                        UI_Comm_TCP _frm1 = new UI_Comm_TCP(this._deviceConfig);
                        splitContainerBottom.Panel1.Controls.Add(_frm1);
                        UI_Comm_CAN _frm2 = new UI_Comm_CAN(this._deviceConfig);
                        splitContainerBottom.Panel2.Controls.Add(_frm2);

                        _frm1.Dock = DockStyle.Fill;
                        _frm2.Dock = DockStyle.Fill;

                        break;
                    }
                case DeviceCommTypes.UART_CAN:
                    {
                        UI_Comm_UART _frm1 = new UI_Comm_UART(this._deviceConfig);
                        splitContainerBottom.Panel1.Controls.Add(_frm1);
                        UI_Comm_CAN _frm2 = new UI_Comm_CAN(this._deviceConfig);
                        splitContainerBottom.Panel2.Controls.Add(_frm2);

                        _frm1.Dock = DockStyle.Fill;
                        _frm2.Dock = DockStyle.Fill;

                        break;
                    }
                case DeviceCommTypes.UART_I2C:
                    {
                        UI_Comm_UART _frm1 = new UI_Comm_UART(this._deviceConfig);
                        splitContainerBottom.Panel1.Controls.Add(_frm1);
                        UI_Comm_I2C _frm2 = new UI_Comm_I2C(this._deviceConfig);
                        splitContainerBottom.Panel2.Controls.Add(_frm2);

                        _frm1.Dock = DockStyle.Fill;
                        _frm2.Dock = DockStyle.Fill;

                        break;
                    }
                case DeviceCommTypes.I2C_CAN:
                    {
                        UI_Comm_I2C _frm1 = new UI_Comm_I2C(this._deviceConfig);
                        splitContainerBottom.Panel1.Controls.Add(_frm1);
                        UI_Comm_CAN _frm2 = new UI_Comm_CAN(this._deviceConfig);
                        splitContainerBottom.Panel2.Controls.Add(_frm2);

                        _frm1.Dock = DockStyle.Fill;
                        _frm2.Dock = DockStyle.Fill;

                        break;
                    }
                default:
                    {
                        splitContainerBottom.Panel1.Controls.Add(_frmSpare);
                        UI_Comm_Spare _frmSpare2 = new UI_Comm_Spare();
                        splitContainerBottom.Panel2.Controls.Add(_frmSpare2);

                        _frmSpare.Dock = DockStyle.Fill;
                        _frmSpare2.Dock = DockStyle.Fill;
                        break;
                    }
            }
        }

        public DeviceConfig GetDeviceConfig()
        {
            try
            {                
                this._deviceConfig = new DeviceConfig();
                this._deviceConfig.DeviceCommType = (DeviceCommTypes)cbDevCommType.SelectedIndex;
                this._deviceConfig.Communications.Clear();

                var _devId = Convert.ToByte(txtDevId.Text);
                if (_devId <= 0)
                {
                    MessageBox.Show("The device id must be greater than 0!", "Edit Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }

                if (this._oldId != _devId)
                {
                    if (this._deviceManager.DeviceLibrary.DeviceIds.Count > 0)
                    {
                        if (this._deviceManager.DeviceLibrary.DeviceIds.Contains(_devId))
                        {
                            var _dialogResult = MessageBox.Show($"The device [id: {_devId}] is existed!\nDo you want to continue?",
                                "Edit Device", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (_dialogResult == DialogResult.No)
                                return null;
                        }
                    }
                }

                this._deviceConfig.Id = _devId;
                this._deviceConfig.Name = txtDevName.Text;
                this._deviceConfig.Voltage = Convert.ToByte(txtDevVoltage.Text);
                this._deviceConfig.Pwm.DevicePwm1.Tmin = Convert.ToInt32(txtPwmTmin.Text);
                this._deviceConfig.Pwm.DevicePwm1.Tmax = Convert.ToInt32(txtPwmTmax.Text);

                switch (this._deviceConfig.DeviceCommType)
                {
                    case DeviceCommTypes.TCP:
                        {
                            var _frm = (UI_Comm_TCP)splitContainerBottom.Panel1.Controls[0];
                            DeviceComm devComm = new DeviceComm();
                            devComm.Type = CommTypes.TCP;
                            devComm.Parameters = _frm.GetParameters();
                            this._deviceConfig.Communications.Add(devComm);                            

                            break;
                        }
                    case DeviceCommTypes.UART:
                        {
                            var _frm = (UI_Comm_UART)splitContainerBottom.Panel1.Controls[0];
                            DeviceComm devComm = new DeviceComm();
                            devComm.Type = CommTypes.UART;
                            devComm.Parameters = _frm.GetParameters();
                            this._deviceConfig.Communications.Add(devComm);

                            break;
                        }
                    case DeviceCommTypes.I2C:
                        {
                            var _frm = (UI_Comm_I2C)splitContainerBottom.Panel1.Controls[0];
                            DeviceComm devComm = new DeviceComm();
                            devComm.Type = CommTypes.I2C;
                            devComm.Parameters = _frm.GetParameters();
                            this._deviceConfig.Communications.Add(devComm);

                            break;
                        }
                    case DeviceCommTypes.CAN:
                        {
                            var _frm = (UI_Comm_CAN)splitContainerBottom.Panel1.Controls[0];
                            DeviceComm devComm = new DeviceComm();
                            devComm.Type = CommTypes.CAN;
                            devComm.Parameters = _frm.GetParameters();
                            this._deviceConfig.Communications.Add(devComm);

                            break;
                        }
                    case DeviceCommTypes.TCP_UART:
                        {
                            var _frm1 = (UI_Comm_TCP)splitContainerBottom.Panel1.Controls[0];
                            var _frm2 = (UI_Comm_UART)splitContainerBottom.Panel2.Controls[0];

                            DeviceComm devComm1 = new DeviceComm();
                            devComm1.Type = CommTypes.TCP;
                            devComm1.Parameters = _frm1.GetParameters();
                            this._deviceConfig.Communications.Add(devComm1);

                            DeviceComm devComm2 = new DeviceComm();
                            devComm2.Type = CommTypes.UART;
                            devComm2.Parameters = _frm2.GetParameters();
                            this._deviceConfig.Communications.Add(devComm2);

                            break;
                        }
                    case DeviceCommTypes.TCP_I2C:
                        {
                            var _frm1 = (UI_Comm_TCP)splitContainerBottom.Panel1.Controls[0];
                            var _frm2 = (UI_Comm_I2C)splitContainerBottom.Panel2.Controls[0];

                            DeviceComm devComm1 = new DeviceComm();
                            devComm1.Type = CommTypes.TCP;
                            devComm1.Parameters = _frm1.GetParameters();
                            this._deviceConfig.Communications.Add(devComm1);

                            DeviceComm devComm2 = new DeviceComm();
                            devComm2.Type = CommTypes.I2C;
                            devComm2.Parameters = _frm2.GetParameters();
                            this._deviceConfig.Communications.Add(devComm2);

                            break;
                        }
                    case DeviceCommTypes.TCP_CAN:
                        {
                            var _frm1 = (UI_Comm_TCP)splitContainerBottom.Panel1.Controls[0];
                            var _frm2 = (UI_Comm_CAN)splitContainerBottom.Panel2.Controls[0];

                            DeviceComm devComm1 = new DeviceComm();
                            devComm1.Type = CommTypes.TCP;
                            devComm1.Parameters = _frm1.GetParameters();
                            this._deviceConfig.Communications.Add(devComm1);

                            DeviceComm devComm2 = new DeviceComm();
                            devComm2.Type = CommTypes.CAN;
                            devComm2.Parameters = _frm2.GetParameters();
                            this._deviceConfig.Communications.Add(devComm2);

                            break;
                        }
                    case DeviceCommTypes.UART_CAN:
                        {
                            var _frm1 = (UI_Comm_UART)splitContainerBottom.Panel1.Controls[0];
                            var _frm2 = (UI_Comm_CAN)splitContainerBottom.Panel2.Controls[0];

                            DeviceComm devComm1 = new DeviceComm();
                            devComm1.Type = CommTypes.UART;
                            devComm1.Parameters = _frm1.GetParameters();
                            this._deviceConfig.Communications.Add(devComm1);

                            DeviceComm devComm2 = new DeviceComm();
                            devComm2.Type = CommTypes.CAN;
                            devComm2.Parameters = _frm2.GetParameters();
                            this._deviceConfig.Communications.Add(devComm2);

                            break;
                        }
                    case DeviceCommTypes.UART_I2C:
                        {
                            var _frm1 = (UI_Comm_UART)splitContainerBottom.Panel1.Controls[0];
                            var _frm2 = (UI_Comm_I2C)splitContainerBottom.Panel2.Controls[0];

                            DeviceComm devComm1 = new DeviceComm();
                            devComm1.Type = CommTypes.UART;
                            devComm1.Parameters = _frm1.GetParameters();
                            this._deviceConfig.Communications.Add(devComm1);

                            DeviceComm devComm2 = new DeviceComm();
                            devComm2.Type = CommTypes.I2C;
                            devComm2.Parameters = _frm2.GetParameters();
                            this._deviceConfig.Communications.Add(devComm2);

                            break;
                        }
                    case DeviceCommTypes.I2C_CAN:
                        {
                            var _frm1 = (UI_Comm_I2C)splitContainerBottom.Panel1.Controls[0];
                            var _frm2 = (UI_Comm_CAN)splitContainerBottom.Panel2.Controls[0];

                            DeviceComm devComm1 = new DeviceComm();
                            devComm1.Type = CommTypes.I2C;
                            devComm1.Parameters = _frm1.GetParameters();
                            this._deviceConfig.Communications.Add(devComm1);

                            DeviceComm devComm2 = new DeviceComm();
                            devComm2.Type = CommTypes.CAN;
                            devComm2.Parameters = _frm2.GetParameters();
                            this._deviceConfig.Communications.Add(devComm2);

                            break;
                        }
                }

                DialogResult = DialogResult.OK;
                return this._deviceConfig;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return null;
            }
        }

        private void btnEditDevice_Click(object sender, EventArgs e)
        {
            GetDeviceConfig();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void txtDevId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtDevVoltage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtPwmTmin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && (e.KeyChar != '.'))
                e.Handled = true;
        }

        private void txtPwmTmax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && (e.KeyChar != '.'))
                e.Handled = true;
        }

        private void splitContainerBottom_DoubleClick(object sender, EventArgs e)
        {
            splitContainerBottom.SplitterDistance = this._fixSplitterDistance;
        }        
    }
}
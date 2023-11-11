using System;
using System.Windows.Forms;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.Organizer;
using Drone_Management_Tools.Utilities;

namespace Drone_Management_Tools.UIDesign
{
    public partial class AddDeviceForm : DevExpress.XtraEditors.XtraForm
    {
        private DeviceManager _deviceManager;
        private int _fixSplitterDistance;

        public AddDeviceForm(DeviceManager deviceManager)
        {
            InitializeComponent();

            CalculateFormSize();

            this._deviceManager = deviceManager;
            this._deviceManager.NewDevice = new DeviceConfig();
        }

        private void AddDeviceForm_Load(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => InitParameters()));
            else
                InitParameters();
        }

        private void InitParameters()
        {                     
            txtDevId.Text = this._deviceManager.NewDevice.Id.ToString();
            txtDevName.Text = this._deviceManager.NewDevice.Name;
            txtDevVoltage.Text = this._deviceManager.NewDevice.Voltage.ToString();

            //--- PWM-1
            txtPwm1Freq.Text = this._deviceManager.NewDevice.Pwm.DevicePwm1.Frequency.ToString();
            txtPwm1Tmin.Text = this._deviceManager.NewDevice.Pwm.DevicePwm1.Tmin.ToString();
            txtPwm1Tmax.Text = this._deviceManager.NewDevice.Pwm.DevicePwm1.Tmax.ToString();
            //--- PWM-2
            txtPwm2Freq.Text = this._deviceManager.NewDevice.Pwm.DevicePwm2.Frequency.ToString();
            txtPwm2Tmin.Text = this._deviceManager.NewDevice.Pwm.DevicePwm2.Tmin.ToString();
            txtPwm2Tmax.Text = this._deviceManager.NewDevice.Pwm.DevicePwm2.Tmax.ToString();
            //--- PWM-3
            txtPwm3Freq.Text = this._deviceManager.NewDevice.Pwm.DevicePwm3.Frequency.ToString();
            txtPwm3Tmin.Text = this._deviceManager.NewDevice.Pwm.DevicePwm3.Tmin.ToString();
            txtPwm3Tmax.Text = this._deviceManager.NewDevice.Pwm.DevicePwm3.Tmax.ToString();
            //--- PWM-4
            txtPwm4Freq.Text = this._deviceManager.NewDevice.Pwm.DevicePwm4.Frequency.ToString();
            txtPwm4Tmin.Text = this._deviceManager.NewDevice.Pwm.DevicePwm4.Tmin.ToString();
            txtPwm4Tmax.Text = this._deviceManager.NewDevice.Pwm.DevicePwm4.Tmax.ToString();

            cbDevCommType.DataSource = Enum.GetValues(typeof(DeviceCommTypes));

            this._fixSplitterDistance = splitContainerBottom.SplitterDistance;
        }

        private void CalculateFormSize()
        {
            var winScale = HelperUtils.GetScalingFactor();
            var frmScaleWidth = (int)Math.Round(this.Width / winScale);
            var frmScaleHeight = (int)Math.Round(this.Height / winScale) + 50;
            this.Size = new System.Drawing.Size(frmScaleWidth, frmScaleHeight);
        }

        private void cbDevCommType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var _devCommType = (DeviceCommTypes)cbDevCommType.SelectedIndex;
            CommBehavior(_devCommType);
        }

        private void CommBehavior(DeviceCommTypes devCommType)
        {
            this._deviceManager.NewDevice.DeviceCommType = devCommType;
            this._deviceManager.NewDevice.Communications.Clear();
            splitContainerBottom.Panel1.Controls.Clear();
            splitContainerBottom.Panel2.Controls.Clear();            

            switch (devCommType)
            {
                case DeviceCommTypes.TCP:
                    {
                        Behavior_TCP();
                        break;
                    }
                case DeviceCommTypes.UART:
                    {
                        Behavior_UART();
                        break;
                    }
                case DeviceCommTypes.I2C:
                    {
                        Behavior_I2C();
                        break;
                    }
                case DeviceCommTypes.CAN:
                    {
                        Behavior_CAN();
                        break;
                    }
                case DeviceCommTypes.TCP_UART:
                    {
                        Behavior_TCP_UART();
                        break;
                    }
                case DeviceCommTypes.TCP_I2C:
                    {
                        Behavior_TCP_I2C();
                        break;
                    }
                case DeviceCommTypes.TCP_CAN:
                    {
                        Behavior_TCP_CAN();
                        break;
                    }
                case DeviceCommTypes.UART_CAN:
                    {
                        Behavior_UART_CAN();
                        break;
                    }
                case DeviceCommTypes.UART_I2C:
                    {
                        Behavior_UART_I2C();
                        break;
                    }
                case DeviceCommTypes.I2C_CAN:
                    {
                        Behavior_I2C_CAN();
                        break;
                    }
                default:
                    {
                        Behavior_Spare();
                        break;
                    }
            }
        }

        private void Behavior_TCP()
        {            
            DeviceComm _devComm = new DeviceComm();
            _devComm.Type = CommTypes.TCP;
            _devComm.Parameters = new Protocol_TCP();
            this._deviceManager.NewDevice.Communications.Add(_devComm);

            UI_Comm_TCP _frm = new UI_Comm_TCP(this._deviceManager.NewDevice);
            UI_Comm_Spare _frmSpare = new UI_Comm_Spare();
            splitContainerBottom.Panel1.Controls.Add(_frm);
            splitContainerBottom.Panel2.Controls.Add(_frmSpare);

            _frm.Dock = DockStyle.Fill;
            _frmSpare.Dock = DockStyle.Fill;
        }

        private void Behavior_UART()
        {
            DeviceComm _devComm = new DeviceComm();
            _devComm.Type = CommTypes.UART;
            _devComm.Parameters = new Protocol_UART();
            this._deviceManager.NewDevice.Communications.Add(_devComm);

            UI_Comm_UART _frm = new UI_Comm_UART(this._deviceManager.NewDevice);
            UI_Comm_Spare _frmSpare = new UI_Comm_Spare();
            splitContainerBottom.Panel1.Controls.Add(_frm);
            splitContainerBottom.Panel2.Controls.Add(_frmSpare);

            _frm.Dock = DockStyle.Fill;
            _frmSpare.Dock = DockStyle.Fill;
        }

        private void Behavior_I2C()
        {            
            DeviceComm _devComm = new DeviceComm();
            _devComm.Type = CommTypes.I2C;
            _devComm.Parameters = new Protocol_I2C();
            this._deviceManager.NewDevice.Communications.Add(_devComm);

            UI_Comm_I2C _frm = new UI_Comm_I2C(this._deviceManager.NewDevice);
            UI_Comm_Spare _frmSpare = new UI_Comm_Spare();
            splitContainerBottom.Panel1.Controls.Add(_frm);
            splitContainerBottom.Panel2.Controls.Add(_frmSpare);

            _frm.Dock = DockStyle.Fill;
            _frmSpare.Dock = DockStyle.Fill;
        }

        private void Behavior_CAN()
        {            
            DeviceComm _devComm = new DeviceComm();
            _devComm.Type = CommTypes.CAN;
            _devComm.Parameters = new Protocol_CAN();
            this._deviceManager.NewDevice.Communications.Add(_devComm);

            UI_Comm_CAN _frm = new UI_Comm_CAN(this._deviceManager.NewDevice);
            UI_Comm_Spare _frmSpare = new UI_Comm_Spare();
            splitContainerBottom.Panel1.Controls.Add(_frm);
            splitContainerBottom.Panel2.Controls.Add(_frmSpare);

            _frm.Dock = DockStyle.Fill;
            _frmSpare.Dock = DockStyle.Fill;
        }

        private void Behavior_TCP_UART()
        {            
            DeviceComm _devComm1 = new DeviceComm();
            _devComm1.Type = CommTypes.TCP;
            _devComm1.Parameters = new Protocol_TCP();
            this._deviceManager.NewDevice.Communications.Add(_devComm1);

            DeviceComm _devComm2 = new DeviceComm();
            _devComm2.Type = CommTypes.UART;
            _devComm2.Parameters = new Protocol_UART();
            this._deviceManager.NewDevice.Communications.Add(_devComm2);

            UI_Comm_TCP _frm1 = new UI_Comm_TCP(this._deviceManager.NewDevice);
            splitContainerBottom.Panel1.Controls.Add(_frm1);
            UI_Comm_UART _frm2 = new UI_Comm_UART(this._deviceManager.NewDevice);
            splitContainerBottom.Panel2.Controls.Add(_frm2);

            _frm1.Dock = DockStyle.Fill;
            _frm2.Dock = DockStyle.Fill;
        }

        private void Behavior_TCP_I2C()
        {
            DeviceComm _devComm1 = new DeviceComm();
            _devComm1.Type = CommTypes.TCP;
            _devComm1.Parameters = new Protocol_TCP();
            this._deviceManager.NewDevice.Communications.Add(_devComm1);

            DeviceComm _devComm2 = new DeviceComm();
            _devComm2.Type = CommTypes.I2C;
            _devComm2.Parameters = new Protocol_I2C();
            this._deviceManager.NewDevice.Communications.Add(_devComm2);

            UI_Comm_TCP _frm1 = new UI_Comm_TCP(this._deviceManager.NewDevice);
            splitContainerBottom.Panel1.Controls.Add(_frm1);
            UI_Comm_I2C _frm2 = new UI_Comm_I2C(this._deviceManager.NewDevice);
            splitContainerBottom.Panel2.Controls.Add(_frm2);

            _frm1.Dock = DockStyle.Fill;
            _frm2.Dock = DockStyle.Fill;
        }

        private void Behavior_TCP_CAN()
        {
            DeviceComm _devComm1 = new DeviceComm();
            _devComm1.Type = CommTypes.TCP;
            _devComm1.Parameters = new Protocol_TCP();
            this._deviceManager.NewDevice.Communications.Add(_devComm1);

            DeviceComm _devComm2 = new DeviceComm();
            _devComm2.Type = CommTypes.CAN;
            _devComm2.Parameters = new Protocol_CAN();
            this._deviceManager.NewDevice.Communications.Add(_devComm2);

            UI_Comm_TCP _frm1 = new UI_Comm_TCP(this._deviceManager.NewDevice);
            splitContainerBottom.Panel1.Controls.Add(_frm1);
            UI_Comm_CAN _frm2 = new UI_Comm_CAN(this._deviceManager.NewDevice);
            splitContainerBottom.Panel2.Controls.Add(_frm2);

            _frm1.Dock = DockStyle.Fill;
            _frm2.Dock = DockStyle.Fill;
        }

        private void Behavior_UART_CAN()
        {
            DeviceComm _devComm1 = new DeviceComm();
            _devComm1.Type = CommTypes.UART;
            _devComm1.Parameters = new Protocol_UART();
            this._deviceManager.NewDevice.Communications.Add(_devComm1);

            DeviceComm _devComm2 = new DeviceComm();
            _devComm2.Type = CommTypes.CAN;
            _devComm2.Parameters = new Protocol_CAN();
            this._deviceManager.NewDevice.Communications.Add(_devComm2);

            UI_Comm_UART _frm1 = new UI_Comm_UART(this._deviceManager.NewDevice);
            splitContainerBottom.Panel1.Controls.Add(_frm1);
            UI_Comm_CAN _frm2 = new UI_Comm_CAN(this._deviceManager.NewDevice);
            splitContainerBottom.Panel2.Controls.Add(_frm2);

            _frm1.Dock = DockStyle.Fill;
            _frm2.Dock = DockStyle.Fill;
        }

        private void Behavior_UART_I2C()
        {
            DeviceComm _devComm1 = new DeviceComm();
            _devComm1.Type = CommTypes.UART;
            _devComm1.Parameters = new Protocol_UART();
            this._deviceManager.NewDevice.Communications.Add(_devComm1);

            DeviceComm _devComm2 = new DeviceComm();
            _devComm2.Type = CommTypes.I2C;
            _devComm2.Parameters = new Protocol_I2C();
            this._deviceManager.NewDevice.Communications.Add(_devComm2);

            UI_Comm_UART _frm1 = new UI_Comm_UART(this._deviceManager.NewDevice);
            splitContainerBottom.Panel1.Controls.Add(_frm1);
            UI_Comm_I2C _frm2 = new UI_Comm_I2C(this._deviceManager.NewDevice);
            splitContainerBottom.Panel2.Controls.Add(_frm2);

            _frm1.Dock = DockStyle.Fill;
            _frm2.Dock = DockStyle.Fill;
        }

        private void Behavior_I2C_CAN()
        {
            DeviceComm _devComm1 = new DeviceComm();
            _devComm1.Type = CommTypes.I2C;
            _devComm1.Parameters = new Protocol_I2C();
            this._deviceManager.NewDevice.Communications.Add(_devComm1);

            DeviceComm _devComm2 = new DeviceComm();
            _devComm2.Type = CommTypes.CAN;
            _devComm2.Parameters = new Protocol_CAN();
            this._deviceManager.NewDevice.Communications.Add(_devComm2);

            UI_Comm_I2C _frm1 = new UI_Comm_I2C(this._deviceManager.NewDevice);
            splitContainerBottom.Panel1.Controls.Add(_frm1);
            UI_Comm_CAN _frm2 = new UI_Comm_CAN(this._deviceManager.NewDevice);
            splitContainerBottom.Panel2.Controls.Add(_frm2);

            _frm1.Dock = DockStyle.Fill;
            _frm2.Dock = DockStyle.Fill;
        }

        private void Behavior_Spare()
        {
            UI_Comm_Spare _frmSpare1 = new UI_Comm_Spare();
            splitContainerBottom.Panel1.Controls.Add(_frmSpare1);
            UI_Comm_Spare _frmSpare2 = new UI_Comm_Spare();
            splitContainerBottom.Panel2.Controls.Add(_frmSpare2);

            _frmSpare1.Dock = DockStyle.Fill;
            _frmSpare2.Dock = DockStyle.Fill;
        }

        public DeviceConfig GetDeviceConfig()
        {
            try
            {
                if (GetDeviceCommon() != null)
                {
                    GetDevicePwm();
                    GetDeviceComm();
                }
                else
                    return null;
                                
                DialogResult = DialogResult.OK;
                return this._deviceManager.NewDevice;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return null;
            }
        }

        private DeviceConfig GetDeviceCommon()
        {
            var _devId = Convert.ToByte(txtDevId.Text);
            if (_devId <= 0)
            {
                MessageBox.Show("The device id must be greater than 0!", "Add Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            if (this._deviceManager.DeviceLibrary.DeviceIds.Count > 0)
            {
                if (this._deviceManager.DeviceLibrary.DeviceIds.Contains(_devId))
                {
                    MessageBox.Show($"The device [id: {_devId}] is existed!", "Add Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
            }

            this._deviceManager.NewDevice.Id = _devId;
            this._deviceManager.NewDevice.Name = txtDevName.Text;
            this._deviceManager.NewDevice.Voltage = Convert.ToByte(txtDevVoltage.Text);

            return this._deviceManager.NewDevice;
        }

        private void GetDevicePwm()
        {
            //--- PWM-1
            this._deviceManager.NewDevice.Pwm.DevicePwm1.Frequency = Convert.ToInt32(txtPwm1Freq.Text);
            this._deviceManager.NewDevice.Pwm.DevicePwm1.Tmin = Convert.ToInt32(txtPwm1Tmin.Text);
            this._deviceManager.NewDevice.Pwm.DevicePwm1.Tmax = Convert.ToInt32(txtPwm1Tmax.Text);

            //--- PWM-2
            this._deviceManager.NewDevice.Pwm.DevicePwm2.Frequency = Convert.ToInt32(txtPwm2Freq.Text);
            this._deviceManager.NewDevice.Pwm.DevicePwm2.Tmin = Convert.ToInt32(txtPwm2Tmin.Text);
            this._deviceManager.NewDevice.Pwm.DevicePwm2.Tmax = Convert.ToInt32(txtPwm2Tmax.Text);

            //--- PWM-3
            this._deviceManager.NewDevice.Pwm.DevicePwm3.Frequency = Convert.ToInt32(txtPwm3Freq.Text);
            this._deviceManager.NewDevice.Pwm.DevicePwm3.Tmin = Convert.ToInt32(txtPwm3Tmin.Text);
            this._deviceManager.NewDevice.Pwm.DevicePwm3.Tmax = Convert.ToInt32(txtPwm3Tmax.Text);

            //--- PWM-4
            this._deviceManager.NewDevice.Pwm.DevicePwm4.Frequency = Convert.ToInt32(txtPwm4Freq.Text);
            this._deviceManager.NewDevice.Pwm.DevicePwm4.Tmin = Convert.ToInt32(txtPwm4Tmin.Text);
            this._deviceManager.NewDevice.Pwm.DevicePwm4.Tmax = Convert.ToInt32(txtPwm4Tmax.Text);
        }

        private void GetDeviceComm()
        {
            switch (this._deviceManager.NewDevice.DeviceCommType)
            {
                case DeviceCommTypes.TCP:
                    {
                        GetDeviceComm_TCP();
                        break;
                    }
                case DeviceCommTypes.UART:
                    {
                        GetDeviceComm_UART();
                        break;
                    }
                case DeviceCommTypes.I2C:
                    {
                        GetDeviceComm_I2C();
                        break;
                    }
                case DeviceCommTypes.CAN:
                    {
                        GetDeviceComm_CAN();
                        break;
                    }
                case DeviceCommTypes.TCP_UART:
                    {
                        GetDeviceComm_TCP_UART();
                        break;
                    }
                case DeviceCommTypes.TCP_I2C:
                    {
                        GetDeviceComm_TCP_I2C();
                        break;
                    }
                case DeviceCommTypes.TCP_CAN:
                    {
                        GetDeviceComm_TCP_CAN();
                        break;
                    }
                case DeviceCommTypes.UART_CAN:
                    {
                        GetDeviceComm_UART_CAN();
                        break;
                    }
                case DeviceCommTypes.UART_I2C:
                    {
                        GetDeviceComm_UART_I2C();
                        break;
                    }
                case DeviceCommTypes.I2C_CAN:
                    {
                        GetDeviceComm_I2C_CAN();
                        break;
                    }
            }
        }

        private void GetDeviceComm_TCP()
        {
            var _frm = (UI_Comm_TCP)splitContainerBottom.Panel1.Controls[0];
            if (this._deviceManager.NewDevice.Communications.Count > 0)
            {
                foreach (var devComm in this._deviceManager.NewDevice.Communications)
                {
                    if (devComm.Type == CommTypes.TCP)
                        devComm.Parameters = _frm.GetParameters();
                }
            }
        }

        private void GetDeviceComm_UART()
        {
            var _frm = (UI_Comm_UART)splitContainerBottom.Panel1.Controls[0];
            if (this._deviceManager.NewDevice.Communications.Count > 0)
            {
                foreach (var devComm in this._deviceManager.NewDevice.Communications)
                {
                    if (devComm.Type == CommTypes.UART)
                        devComm.Parameters = _frm.GetParameters();
                }
            }
        }

        private void GetDeviceComm_I2C()
        {
            var _frm = (UI_Comm_I2C)splitContainerBottom.Panel1.Controls[0];
            if (this._deviceManager.NewDevice.Communications.Count > 0)
            {
                foreach (var devComm in this._deviceManager.NewDevice.Communications)
                {
                    if (devComm.Type == CommTypes.I2C)
                        devComm.Parameters = _frm.GetParameters();
                }
            }
        }

        private void GetDeviceComm_CAN()
        {
            var _frm = (UI_Comm_CAN)splitContainerBottom.Panel1.Controls[0];
            if (this._deviceManager.NewDevice.Communications.Count > 0)
            {
                foreach (var devComm in this._deviceManager.NewDevice.Communications)
                {
                    if (devComm.Type == CommTypes.CAN)
                        devComm.Parameters = _frm.GetParameters();
                }
            }
        }

        private void GetDeviceComm_TCP_UART()
        {
            var _frm1 = (UI_Comm_TCP)splitContainerBottom.Panel1.Controls[0];
            var _frm2 = (UI_Comm_UART)splitContainerBottom.Panel2.Controls[0];

            if (this._deviceManager.NewDevice.Communications.Count > 0)
            {
                foreach (var devComm in this._deviceManager.NewDevice.Communications)
                {
                    if (devComm.Type == CommTypes.TCP)
                        devComm.Parameters = _frm1.GetParameters();

                    if (devComm.Type == CommTypes.UART)
                        devComm.Parameters = _frm2.GetParameters();
                }
            }
        }

        private void GetDeviceComm_TCP_I2C()
        {
            var _frm1 = (UI_Comm_TCP)splitContainerBottom.Panel1.Controls[0];
            var _frm2 = (UI_Comm_I2C)splitContainerBottom.Panel2.Controls[0];

            if (this._deviceManager.NewDevice.Communications.Count > 0)
            {
                foreach (var devComm in this._deviceManager.NewDevice.Communications)
                {
                    if (devComm.Type == CommTypes.TCP)
                        devComm.Parameters = _frm1.GetParameters();

                    if (devComm.Type == CommTypes.I2C)
                        devComm.Parameters = _frm2.GetParameters();
                }
            }
        }

        private void GetDeviceComm_TCP_CAN()
        {
            var _frm1 = (UI_Comm_TCP)splitContainerBottom.Panel1.Controls[0];
            var _frm2 = (UI_Comm_CAN)splitContainerBottom.Panel2.Controls[0];

            if (this._deviceManager.NewDevice.Communications.Count > 0)
            {
                foreach (var devComm in this._deviceManager.NewDevice.Communications)
                {
                    if (devComm.Type == CommTypes.TCP)
                        devComm.Parameters = _frm1.GetParameters();

                    if (devComm.Type == CommTypes.CAN)
                        devComm.Parameters = _frm2.GetParameters();
                }
            }
        }

        private void GetDeviceComm_UART_CAN()
        {
            var _frm1 = (UI_Comm_UART)splitContainerBottom.Panel1.Controls[0];
            var _frm2 = (UI_Comm_CAN)splitContainerBottom.Panel2.Controls[0];

            if (this._deviceManager.NewDevice.Communications.Count > 0)
            {
                foreach (var devComm in this._deviceManager.NewDevice.Communications)
                {
                    if (devComm.Type == CommTypes.UART)
                        devComm.Parameters = _frm1.GetParameters();

                    if (devComm.Type == CommTypes.CAN)
                        devComm.Parameters = _frm2.GetParameters();
                }
            }
        }

        private void GetDeviceComm_UART_I2C()
        {
            var _frm1 = (UI_Comm_UART)splitContainerBottom.Panel1.Controls[0];
            var _frm2 = (UI_Comm_I2C)splitContainerBottom.Panel2.Controls[0];

            if (this._deviceManager.NewDevice.Communications.Count > 0)
            {
                foreach (var devComm in this._deviceManager.NewDevice.Communications)
                {
                    if (devComm.Type == CommTypes.UART)
                        devComm.Parameters = _frm1.GetParameters();

                    if (devComm.Type == CommTypes.I2C)
                        devComm.Parameters = _frm2.GetParameters();
                }
            }
        }

        private void GetDeviceComm_I2C_CAN()
        {
            var _frm1 = (UI_Comm_I2C)splitContainerBottom.Panel1.Controls[0];
            var _frm2 = (UI_Comm_CAN)splitContainerBottom.Panel2.Controls[0];

            if (this._deviceManager.NewDevice.Communications.Count > 0)
            {
                foreach (var devComm in this._deviceManager.NewDevice.Communications)
                {
                    if (devComm.Type == CommTypes.I2C)
                        devComm.Parameters = _frm1.GetParameters();

                    if (devComm.Type == CommTypes.CAN)
                        devComm.Parameters = _frm2.GetParameters();
                }
            }
        }

        private void btnRegisterDevice_Click(object sender, EventArgs e)
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

        private void txtPwm1Freq_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtPwm1Tmin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtPwm1Tmax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtPwm2Freq_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtPwm2Tmin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtPwm2Tmax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtPwm3Freq_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtPwm3Tmin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtPwm3Tmax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtPwm4Freq_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtPwm4Tmin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtPwm4Tmax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }        

        private void splitContainerBottom_DoubleClick(object sender, EventArgs e)
        {
            splitContainerBottom.SplitterDistance = this._fixSplitterDistance;
        }
    }
}
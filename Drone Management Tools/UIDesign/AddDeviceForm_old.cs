using System;
using System.Windows.Forms;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.Organizer;
using Drone_Management_Tools.Utilities;

namespace Drone_Management_Tools.UIDesign
{
    public partial class AddDeviceForm_old : DevExpress.XtraEditors.XtraForm
    {
        private DeviceManager _deviceManager;
        private int _fixSplitterDistance;

        public AddDeviceForm_old(DeviceManager deviceManager)
        {
            InitializeComponent();

            this._deviceManager = deviceManager;
            this._deviceManager.NewDevice = new DeviceConfig();
        }

        private void AddDeviceForm_Load(object sender, EventArgs e)
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
            txtDevId.Text = this._deviceManager.NewDevice.Id.ToString();
            txtDevName.Text = this._deviceManager.NewDevice.Name;
            txtDevVoltage.Text = this._deviceManager.NewDevice.Voltage.ToString();

            txtPwmTmin.Text = this._deviceManager.NewDevice.Pwm.DevicePwm1.Tmin.ToString();
            txtPwmTmax.Text = this._deviceManager.NewDevice.Pwm.DevicePwm1.Tmax.ToString();

            cbDevCommType.DataSource = Enum.GetValues(typeof(DeviceCommTypes));

            this._fixSplitterDistance = splitContainerBottom.SplitterDistance;
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
            UI_Comm_Spare _frmSpare = new UI_Comm_Spare();

            switch (devCommType)
            {
                case DeviceCommTypes.TCP:
                    {
                        DeviceComm _devComm = new DeviceComm();
                        _devComm.Type = CommTypes.TCP;
                        _devComm.Parameters = new Protocol_TCP();
                        this._deviceManager.NewDevice.Communications.Add(_devComm);

                        UI_Comm_TCP _frm = new UI_Comm_TCP(this._deviceManager.NewDevice);
                        splitContainerBottom.Panel1.Controls.Add(_frm);
                        splitContainerBottom.Panel2.Controls.Add(_frmSpare);

                        _frm.Dock = DockStyle.Fill;
                        _frmSpare.Dock = DockStyle.Fill;

                        break;
                    }
                case DeviceCommTypes.UART:
                    {
                        DeviceComm _devComm = new DeviceComm();
                        _devComm.Type = CommTypes.UART;
                        _devComm.Parameters = new Protocol_UART();
                        this._deviceManager.NewDevice.Communications.Add(_devComm);

                        UI_Comm_UART _frm = new UI_Comm_UART(this._deviceManager.NewDevice);
                        splitContainerBottom.Panel1.Controls.Add(_frm);
                        splitContainerBottom.Panel2.Controls.Add(_frmSpare);

                        _frm.Dock = DockStyle.Fill;
                        _frmSpare.Dock = DockStyle.Fill;

                        break;
                    }
                case DeviceCommTypes.I2C:
                    {
                        DeviceComm _devComm = new DeviceComm();
                        _devComm.Type = CommTypes.I2C;
                        _devComm.Parameters = new Protocol_I2C();
                        this._deviceManager.NewDevice.Communications.Add(_devComm);

                        UI_Comm_I2C _frm = new UI_Comm_I2C(this._deviceManager.NewDevice);
                        splitContainerBottom.Panel1.Controls.Add(_frm);
                        splitContainerBottom.Panel2.Controls.Add(_frmSpare);

                        _frm.Dock = DockStyle.Fill;
                        _frmSpare.Dock = DockStyle.Fill;

                        break;
                    }
                case DeviceCommTypes.CAN:
                    {
                        DeviceComm _devComm = new DeviceComm();
                        _devComm.Type = CommTypes.CAN;
                        _devComm.Parameters = new Protocol_CAN();
                        this._deviceManager.NewDevice.Communications.Add(_devComm);

                        UI_Comm_CAN _frm = new UI_Comm_CAN(this._deviceManager.NewDevice);
                        splitContainerBottom.Panel1.Controls.Add(_frm);
                        splitContainerBottom.Panel2.Controls.Add(_frmSpare);

                        _frm.Dock = DockStyle.Fill;
                        _frmSpare.Dock = DockStyle.Fill;

                        break;
                    }
                case DeviceCommTypes.TCP_UART:
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

                        break;
                    }
                case DeviceCommTypes.TCP_I2C:
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

                        break;
                    }
                case DeviceCommTypes.TCP_CAN:
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

                        break;
                    }
                case DeviceCommTypes.UART_CAN:
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

                        break;
                    }
                case DeviceCommTypes.UART_I2C:
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

                        break;
                    }
                case DeviceCommTypes.I2C_CAN:
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
                this._deviceManager.NewDevice.Pwm.DevicePwm1.Tmin = Convert.ToInt32(txtPwmTmin.Text);
                this._deviceManager.NewDevice.Pwm.DevicePwm1.Tmax = Convert.ToInt32(txtPwmTmax.Text);

                switch (this._deviceManager.NewDevice.DeviceCommType)
                {
                    case DeviceCommTypes.TCP:
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

                            break;
                        }
                    case DeviceCommTypes.UART:
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

                            break;
                        }
                    case DeviceCommTypes.I2C:
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

                            break;
                        }
                    case DeviceCommTypes.CAN:
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

                            break;
                        }
                    case DeviceCommTypes.TCP_UART:
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

                            break;
                        }
                    case DeviceCommTypes.TCP_I2C:
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

                            break;
                        }
                    case DeviceCommTypes.TCP_CAN:
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

                            break;
                        }
                    case DeviceCommTypes.UART_CAN:
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

                            break;
                        }
                    case DeviceCommTypes.UART_I2C:
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

                            break;
                        }
                    case DeviceCommTypes.I2C_CAN:
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

                            break;
                        }
                }

                DialogResult = DialogResult.OK;
                return this._deviceManager.NewDevice;
            }
            catch (Exception ex)
            {
                LogUtils.AddLog(ex.Message, LogTypes.Error);
                return null;
            }
        }

        private void btnAddDevice_Click(object sender, EventArgs e)
        {
            GetDeviceConfig();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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

        private void txtDevId_KeyPress(object sender, KeyPressEventArgs e)
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
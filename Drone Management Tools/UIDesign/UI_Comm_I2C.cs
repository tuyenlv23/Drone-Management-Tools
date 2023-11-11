using System;
using System.Windows.Forms;
using Drone_Management_Tools.Models;

namespace Drone_Management_Tools.UIDesign
{
    public partial class UI_Comm_I2C : DevExpress.XtraEditors.XtraUserControl
    {
        private DeviceConfig _deviceConfig;
        private Protocol_I2C _protocolI2C;

        public UI_Comm_I2C(DeviceConfig deviceConfig)
        {
            InitializeComponent();
            this._deviceConfig = deviceConfig;

            InitParameters();
        }

        public UI_Comm_I2C(Protocol_I2C protocolI2C)
        {
            InitializeComponent();
            this._protocolI2C = protocolI2C;

            InitParameters();
        }

        private void InitParameters()
        {
            cbMasterSlaveMode.DataSource = Enum.GetValues(typeof(MasterSlaveModes));
            cbAckMode.DataSource = Enum.GetValues(typeof(PermissionModes));
            cbStartStopBits.DataSource = (double[])CommDefaut.StartStopBits.Clone();
            cbConditionRepeat.DataSource = Enum.GetValues(typeof(PermissionModes));
        }

        private void UI_Comm_I2C_Load(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => LoadParameters()));
            else
                LoadParameters();
        }

        private void LoadParameters()
        {
            if (this._deviceConfig != null)
            {
                if (this._deviceConfig.Communications.Count > 0)
                {
                    foreach (var devComm in this._deviceConfig.Communications)
                    {
                        ShowParameters(null);
                        if (devComm.Type == CommTypes.I2C)
                        {
                            var _protocol = (Protocol_I2C)devComm.Parameters;
                            ShowParameters(_protocol);
                        }
                    }
                }
            }
            else
            {
                ShowParameters(this._protocolI2C);
            }
        }

        public void ShowParameters(Protocol_I2C protocolI2C)
        {
            if (protocolI2C != null)
            {
                txtAddress.Text = protocolI2C.Address.ToString();
                txtSpeed.Text = protocolI2C.Speed.ToString();
                cbMasterSlaveMode.Text = protocolI2C.MasterSlaveMode.ToString();
                txtDataSize.Text = protocolI2C.DataSize.ToString();
                cbAckMode.Text = protocolI2C.AckMode.ToString();
                cbStartStopBits.Text = protocolI2C.StartStopBit.ToString();
                cbConditionRepeat.Text = protocolI2C.StartConditionRepeat.ToString();
            }
            else
            {
                txtAddress.Text = "0";
                txtSpeed.Text = "0";
                cbMasterSlaveMode.Text = MasterSlaveModes.Master.ToString();
                txtDataSize.Text = "0";
                cbAckMode.Text = PermissionModes.Yes.ToString();
                cbStartStopBits.Text = "8";
                cbConditionRepeat.Text = PermissionModes.Yes.ToString();
            }
        }

        public Protocol_I2C GetParameters()
        {
            Protocol_I2C _result = new Protocol_I2C();

            _result.Address = Convert.ToByte(txtAddress.Text);
            _result.Speed = Convert.ToByte(txtSpeed.Text);
            _result.MasterSlaveMode = (MasterSlaveModes)Enum.Parse(typeof(MasterSlaveModes), cbMasterSlaveMode.Text);
            _result.DataSize = Convert.ToByte(txtDataSize.Text);
            _result.AckMode = (PermissionModes)Enum.Parse(typeof(PermissionModes), cbAckMode.Text);
            _result.StartStopBit = Convert.ToByte(cbStartStopBits.Text);
            _result.StartConditionRepeat = (PermissionModes)Enum.Parse(typeof(PermissionModes), cbConditionRepeat.Text);

            return _result;
        }

        private void txtSpeed_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtDataSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        public void Behavior(bool enable)
        {
            txtAddress.Enabled = enable;
            txtSpeed.Enabled = enable;
            cbMasterSlaveMode.Enabled = enable;
            txtDataSize.Enabled = enable;
            cbAckMode.Enabled = enable;
            cbStartStopBits.Enabled = enable;
            cbConditionRepeat.Enabled = enable;
        }

        public void DroneBehavior(bool enable)
        {
            cbMasterSlaveMode.Enabled = enable;
            txtDataSize.Enabled = enable;
            cbAckMode.Enabled = enable;
            cbStartStopBits.Enabled = enable;
            cbConditionRepeat.Enabled = enable;
        }
    }
}

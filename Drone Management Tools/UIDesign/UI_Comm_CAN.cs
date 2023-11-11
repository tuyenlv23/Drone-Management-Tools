using System;
using Drone_Management_Tools.Models;

namespace Drone_Management_Tools.UIDesign
{
    public partial class UI_Comm_CAN : DevExpress.XtraEditors.XtraUserControl
    {
        private DeviceConfig _deviceConfig;
        private Protocol_CAN _protocolCAN;

        public UI_Comm_CAN(DeviceConfig deviceConfig)
        {
            InitializeComponent();
            this._deviceConfig = deviceConfig;

            InitParameters();
        }

        public UI_Comm_CAN(Protocol_CAN protocolCAN)
        {
            InitializeComponent();
            this._protocolCAN = protocolCAN;

            InitParameters();
        }

        private void InitParameters()
        {
            cbBaudrate.DataSource = (uint[])CommDefaut.Baudrates.Clone();
            cbReadWriteMode.DataSource = Enum.GetValues(typeof(AccessModes));
        }

        private void UI_Comm_CAN_Load(object sender, EventArgs e)
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
                        if (devComm.Type == CommTypes.CAN)
                        {
                            var _protocol = (Protocol_CAN)devComm.Parameters;
                            ShowParameters(_protocol);
                        }
                    }
                }
            }
            else
            {
                ShowParameters(this._protocolCAN);
            }
        }

        public void ShowParameters(Protocol_CAN protocolCAN)
        {
            if (protocolCAN != null)
            {
                cbBaudrate.Text = protocolCAN.Baudrate.ToString();
                cbReadWriteMode.Text = protocolCAN.ReadWriteMode.ToString();
            }
        }

        public Protocol_CAN GetParameters()
        {
            Protocol_CAN _result = new Protocol_CAN();

            _result.Baudrate = Convert.ToUInt32(cbBaudrate.Text);
            _result.ReadWriteMode = (AccessModes)Enum.Parse(typeof(AccessModes), cbReadWriteMode.Text);

            return _result;
        }

        public void Behavior(bool enable)
        {
            cbBaudrate.Enabled = enable;
            cbReadWriteMode.Enabled = enable;
        }

        public void DroneBehavior(bool enable)
        {
            cbReadWriteMode.Enabled = enable;
        }
    }
}

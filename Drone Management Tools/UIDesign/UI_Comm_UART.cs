using System;
using Drone_Management_Tools.Models;

namespace Drone_Management_Tools.UIDesign
{
    public partial class UI_Comm_UART : DevExpress.XtraEditors.XtraUserControl
    {
        private DeviceConfig _deviceConfig;
        private Protocol_UART _protocolUART;

        public UI_Comm_UART(DeviceConfig deviceConfig)
        {
            InitializeComponent();
            this._deviceConfig = deviceConfig;

            InitParameters();
        }

        public UI_Comm_UART(Protocol_UART protocolUART)
        {
            InitializeComponent();
            this._protocolUART = protocolUART;

            InitParameters();
        }

        private void InitParameters()
        {
            cbBaudrate.DataSource = (uint[])CommDefaut.Baudrates.Clone();
            cbDataBits.DataSource = (byte[])CommDefaut.DataBits.Clone();
            cbParity.DataSource = Enum.GetValues(typeof(CommParity));
            cbStopBits.DataSource = (double[])CommDefaut.StopBits.Clone();
        }

        private void UI_Comm_UART_Load(object sender, EventArgs e)
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
                        if (devComm.Type == CommTypes.UART)
                        {
                            var _protocol = (Protocol_UART)devComm.Parameters;
                            ShowParameters(_protocol);
                        }
                    }
                }
            }
            else
            {
                ShowParameters(this._protocolUART);
            }
        }

        public void ShowParameters(Protocol_UART protocolUART)
        {
            if (protocolUART != null)
            {
                cbBaudrate.Text = protocolUART.Baudrate.ToString();
                cbDataBits.Text = protocolUART.DataBit.ToString();
                cbParity.Text = protocolUART.Parity.ToString();
                cbStopBits.Text = protocolUART.StopBit.ToString();
            }
        }

        public Protocol_UART GetParameters()
        {
            Protocol_UART _result = new Protocol_UART();

            _result.Baudrate = Convert.ToUInt32(cbBaudrate.Text);
            _result.DataBit = Convert.ToByte(cbDataBits.Text);
            _result.Parity = (CommParity)Enum.Parse(typeof(CommParity), cbParity.Text);
            _result.StopBit = Convert.ToDouble(cbStopBits.Text);

            return _result;
        }

        public void Behavior(bool enable)
        {
            cbBaudrate.Enabled = enable;
            cbDataBits.Enabled = enable;
            cbParity.Enabled = enable;
            cbStopBits.Enabled = enable;
        }
    }
}

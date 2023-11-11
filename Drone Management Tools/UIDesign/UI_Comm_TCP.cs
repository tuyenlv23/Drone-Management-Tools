using System;
using System.Windows.Forms;
using Drone_Management_Tools.Models;

namespace Drone_Management_Tools.UIDesign
{
    public partial class UI_Comm_TCP : DevExpress.XtraEditors.XtraUserControl
    {
        private DeviceConfig _deviceConfig;
        private Protocol_TCP _protocolTCP;

        public UI_Comm_TCP(DeviceConfig deviceConfig)
        {
            InitializeComponent();
            this._deviceConfig = deviceConfig;

            InitParameters();
        }

        public UI_Comm_TCP(Protocol_TCP protocolTCP)
        {
            InitializeComponent();
            this._protocolTCP = protocolTCP;

            InitParameters();
        }

        private void InitParameters()
        {
            cbServerClientMode.DataSource = Enum.GetValues(typeof(ServerClientModes));
        }

        private void UI_Comm_TCP_Load(object sender, EventArgs e)
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
                        if (devComm.Type == CommTypes.TCP)
                        {
                            var _protocol = (Protocol_TCP)devComm.Parameters;
                            ShowParameters(_protocol);
                        }
                    }
                }
            }
            else
            {
                ShowParameters(this._protocolTCP);
            }
        }

        public void ShowParameters(Protocol_TCP protocolTCP)
        {
            if (protocolTCP != null)
            {
                txtAddress.Text = protocolTCP.Address;
                txtPort.Text = protocolTCP.Port.ToString();
                cbServerClientMode.Text = protocolTCP.ServerClientMode.ToString();
            }
            else
            {
                txtAddress.Text = "127.0.0.1";
                txtPort.Text = "123";
                cbServerClientMode.Text = ServerClientModes.Client.ToString();
            }
        }

        public Protocol_TCP GetParameters()
        {
            Protocol_TCP _result = new Protocol_TCP();

            _result.Address = txtAddress.Text;
            _result.Port = Convert.ToByte(txtPort.Text);
            _result.ServerClientMode = (ServerClientModes)Enum.Parse(typeof(ServerClientModes), cbServerClientMode.Text);

            return _result;
        }

        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        public void Behavior(bool enable)
        {
            txtAddress.Enabled = enable;
            txtPort.Enabled = enable;
            cbServerClientMode.Enabled = enable;
        }
    }
}

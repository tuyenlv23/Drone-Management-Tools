using System;
using System.IO.Ports;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.Organizer;
using Drone_Management_Tools.Utilities;

namespace Drone_Management_Tools.UIDesign
{
    public partial class UartConnectionForm : DevExpress.XtraEditors.XtraForm
    {
        public ConnectionManager _connectionManager;

        public UartConnectionForm()
        {
            InitializeComponent();
        }

        public UartConnectionForm(ConnectionManager connectionManager)
        {
            InitializeComponent();
            //CalculateFormSize();
            this._connectionManager = connectionManager;
        }

        private void UartForm_Load(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                InitParameters();
            }));                   
        }

        private void CalculateFormSize()
        {
            var winScale = HelperUtils.GetScalingFactor();
            var frmScaleWidth = (int)Math.Round(this.Width / winScale);
            var frmScaleHeight = (int)Math.Round(this.Height / winScale);
            this.Size = new System.Drawing.Size(frmScaleWidth, frmScaleHeight);
        }

        void InitParameters()
        {
            if (this._connectionManager != null)
            {
                this._connectionManager.EventWhenOpened -= _connectionManager_EventWhenOpened;
                this._connectionManager.EventWhenClosed -= _connectionManager_EventWhenClosed;
                this._connectionManager.EventWhenOpened += _connectionManager_EventWhenOpened;
                this._connectionManager.EventWhenClosed += _connectionManager_EventWhenClosed;

                var _ports = SerialPort.GetPortNames();
                Array.Sort(_ports);
                cbPort.DataSource = _ports;                
                var _baudrates = (uint[])CommDefaut.Baudrates.Clone();
                cbBaudrate.DataSource = _baudrates;
                cbBaudrate.SelectedIndex = 4;

                if (this._connectionManager.IsOpen)
                {
                    cbPort.SelectedIndex = Array.IndexOf(_ports, this._connectionManager.CommParm.ComPort);
                    cbBaudrate.SelectedIndex = Array.IndexOf(_baudrates, this._connectionManager.CommParm.Baudrate);

                    this._connectionManager.OnOpened();
                }
                else
                    this._connectionManager.OnClosed(CommStates.Closed.ToString());
            }    
            else
            {
                this._connectionManager = new ConnectionManager();
                this._connectionManager.EventWhenOpened += _connectionManager_EventWhenOpened;
                this._connectionManager.EventWhenClosed += _connectionManager_EventWhenClosed;

                cbPort.DataSource = SerialPort.GetPortNames();
                cbBaudrate.DataSource = (uint[])CommDefaut.Baudrates.Clone();
                cbBaudrate.SelectedIndex = 4;
            }    
        }

        private void _connectionManager_EventWhenOpened(object sender, string e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    sttConnectionState.Text = e;
                    //grpParameters.Enabled = false;
                    cbPort.Enabled = false;
                    cbBaudrate.Enabled = false;
                    btnConnect.Enabled = false;
                    btnDisconnect.Enabled = true;
                }));
            }
            else
            {
                sttConnectionState.Text = e;
                //grpParameters.Enabled = false;
                cbPort.Enabled = false;
                cbBaudrate.Enabled = false;
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
            }    
        }

        private void _connectionManager_EventWhenClosed(object sender, string e)
        {            
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    sttConnectionState.Text = e;
                    //grpParameters.Enabled = true;
                    cbPort.Enabled = true;
                    cbBaudrate.Enabled = true;
                    btnConnect.Enabled = true;
                    btnDisconnect.Enabled = false;
                }));
            }
            else
            {
                sttConnectionState.Text = e;
                //grpParameters.Enabled = true;
                cbPort.Enabled = true;
                cbBaudrate.Enabled = true;
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
            }    
        }

        private void GetSerialParameters()
        {
            this.Invoke(new Action(() =>
            {
                this._connectionManager.CommParm.ComPort = cbPort.SelectedValue.ToString();
                this._connectionManager.CommParm.Baudrate = Convert.ToInt32(cbBaudrate.SelectedValue);
            }));
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            GetSerialParameters();
            if (this._connectionManager != null)
                this._connectionManager.ConnectSerial();
        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            if (this._connectionManager != null)
                this._connectionManager.DisconnectSerial();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
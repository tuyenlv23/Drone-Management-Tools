using System;
using System.Windows.Forms;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.Organizer;

namespace Drone_Management_Tools.UIDesign
{
    public partial class ManageDroneForm : DevExpress.XtraEditors.XtraForm
    {
        private DataServerProcess _dsProcess;
        private DroneModel _droneModel;

        public ManageDroneForm()
        {
            InitializeComponent();
        }

        public ManageDroneForm(DataServerProcess dsProcess)
        {
            InitializeComponent();
            this._dsProcess = dsProcess;
            this._droneModel = dsProcess.ConfigManager.DroneConfig.DroneModel;
            //this._droneModel.CreateNull();
        }

        private void ManageDroneForm_Load(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => InitForm()));
            else
                InitForm();
        }

        private void InitForm()
        {
            //--- I2C
            UI_Comm_I2C _i2c1 = new UI_Comm_I2C(this._droneModel.DroneI2C.DroneI2C1);
            _i2c1.DroneBehavior(false);
            grpI2C_1.Controls.Add(_i2c1);
            grpI2C_1.Controls[0].Dock = DockStyle.Fill;
            UI_Comm_I2C _i2c2 = new UI_Comm_I2C(this._droneModel.DroneI2C.DroneI2C2);
            _i2c2.DroneBehavior(false);
            grpI2C_2.Controls.Add(_i2c2);
            grpI2C_2.Controls[0].Dock = DockStyle.Fill;

            //--- UART
            UI_Comm_UART _uart1 = new UI_Comm_UART(this._droneModel.DroneUART.DroneUART1);            
            grpUART_1.Controls.Add(_uart1);
            grpUART_1.Controls[0].Dock = DockStyle.Fill;
            UI_Comm_UART _uart2 = new UI_Comm_UART(this._droneModel.DroneUART.DroneUART2);
            grpUART_2.Controls.Add(_uart2);
            grpUART_2.Controls[0].Dock = DockStyle.Fill;

            //--- CAN
            UI_Comm_CAN _can1 = new UI_Comm_CAN(this._droneModel.DroneCAN.DroneCAN1);
            _can1.DroneBehavior(false);
            grpCAN_1.Controls.Add(_can1);
            grpCAN_1.Controls[0].Dock = DockStyle.Fill;

            //--- TCP
            UI_Comm_TCP _tcp1 = new UI_Comm_TCP(this._droneModel.DroneTCP.DroneTCP1);            
            grpTCP_1.Controls.Add(_tcp1);
            grpTCP_1.Controls[0].Dock = DockStyle.Fill;

            //--- PWM
            UI_Comm_PWM _pwm = new UI_Comm_PWM(this._droneModel.DronePwm);
            pnlPWM.Controls.Add(_pwm);
            pnlPWM.Controls[0].Dock= DockStyle.Fill;
        }        

        public void ShowParameters(DroneModel droneModel)
        {
            //--- I2C
            var _frmI2C_1 = (UI_Comm_I2C)grpI2C_1.Controls[0];
            _frmI2C_1.ShowParameters(droneModel.DroneI2C.DroneI2C1);
            var _frmI2C_2 = (UI_Comm_I2C)grpI2C_2.Controls[0];
            _frmI2C_2.ShowParameters(droneModel.DroneI2C.DroneI2C2);

            //--- UART
            var _frmUART_1 = (UI_Comm_UART)grpUART_1.Controls[0];
            _frmUART_1.ShowParameters(droneModel.DroneUART.DroneUART1);
            var _frmUART_2 = (UI_Comm_UART)grpUART_2.Controls[0];
            _frmUART_2.ShowParameters(droneModel.DroneUART.DroneUART2);

            //--- CAN
            var _frmCAN_1 = (UI_Comm_CAN)grpCAN_1.Controls[0];
            _frmCAN_1.ShowParameters(droneModel.DroneCAN.DroneCAN1);

            //--- TCP
            var _frmTCP_1 = (UI_Comm_TCP)grpTCP_1.Controls[0];
            _frmTCP_1.ShowParameters(droneModel.DroneTCP.DroneTCP1);

            //--- PWM
            var _frmPWM = (UI_Comm_PWM)pnlPWM.Controls[0];
            _frmPWM.ShowParameters(droneModel.DronePwm);
        }

        public bool GetParameters()
        {
            try
            {
                //--- I2C
                var _frmI2C_1 = (UI_Comm_I2C)grpI2C_1.Controls[0];
                this._droneModel.DroneI2C.DroneI2C1 = _frmI2C_1.GetParameters();
                var _frmI2C_2 = (UI_Comm_I2C)grpI2C_2.Controls[0];
                this._droneModel.DroneI2C.DroneI2C2 = _frmI2C_2.GetParameters();

                //--- UART
                var _frmUART_1 = (UI_Comm_UART)grpUART_1.Controls[0];
                this._droneModel.DroneUART.DroneUART1 = _frmUART_1.GetParameters();
                var _frmUART_2 = (UI_Comm_UART)grpUART_2.Controls[0];
                this._droneModel.DroneUART.DroneUART2 = _frmUART_2.GetParameters();

                //--- CAN
                var _frmCAN_1 = (UI_Comm_CAN)grpCAN_1.Controls[0];
                this._droneModel.DroneCAN.DroneCAN1 = _frmCAN_1.GetParameters();

                //--- TCP
                var _frmTCP_1 = (UI_Comm_TCP)grpTCP_1.Controls[0];
                this._droneModel.DroneTCP.DroneTCP1 = _frmTCP_1.GetParameters();

                //--- PWM
                var _frmPWM = (UI_Comm_PWM)pnlPWM.Controls[0];
                this._droneModel.DronePwm = _frmPWM.GetParameters();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void btnReadConfig_Click(object sender, EventArgs e)
        {
            this._dsProcess.UpdateManageDrone(this);
            this._dsProcess.ReadDroneConfig();
        }

        private void btnSendConfig_Click(object sender, EventArgs e)
        {
            if (GetParameters())
                this._dsProcess.SendDroneConfig(this._droneModel);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
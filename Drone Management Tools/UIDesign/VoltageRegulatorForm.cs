using System;
using System.Windows.Forms;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.Organizer;
using Drone_Management_Tools.Utilities;

namespace Drone_Management_Tools.UIDesign
{
    public partial class VoltageRegulatorForm : DevExpress.XtraEditors.XtraForm
    {
        private DataServerProcess _dsProcess;
        private VoltageRegulator _voltageRegulator;

        public VoltageRegulatorForm(DataServerProcess dataServerProcess)
        {
            InitializeComponent();
            //CalculateFormSize();

            this._dsProcess = dataServerProcess;
            this._voltageRegulator = dataServerProcess.ConfigManager.VoltageConfig.VoltageRegulator;
        }

        private void VoltageRegulatorForm_Load(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => InitParameters()));
            else
                InitParameters();
        }

        private void CalculateFormSize()
        {
            var winScale = HelperUtils.GetScalingFactor();
            var frmScaleWidth = (int)Math.Round(this.Width / winScale);
            var frmScaleHeight = (int)Math.Round(this.Height / winScale);
            this.Size = new System.Drawing.Size(frmScaleWidth, frmScaleHeight);
        }

        private void InitParameters()
        {
            txtFactorA.Text = this._voltageRegulator.FactorA.ToString();
            txtFactorB.Text = this._voltageRegulator.FactorB.ToString();
            txtVoltRegDesc.Text = this._voltageRegulator.Description.ToString();
        }

        private VoltageRegulator GetParameters()
        {
            try
            {
                this._voltageRegulator.FactorA = Convert.ToSingle(txtFactorA.Text);
                this._voltageRegulator.FactorB = Convert.ToSingle(txtFactorB.Text);
                this._voltageRegulator.Description = txtVoltRegDesc.Text;

                return this._voltageRegulator;
            }
            catch
            {
                return null;
            }
        }

        private void btnSendConfig_Click(object sender, EventArgs e)
        {
            if (GetParameters() != null)
            {
                this._dsProcess.SendVoltageRegulator(this._voltageRegulator);
                DialogResult = DialogResult.OK;
            }
            else
            {
                var _msg = $"Voltage regulator is wrong format";
                LogUtils.AddLog(_msg, LogTypes.Info);
                MessageBox.Show(_msg, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void txtFactorA_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar.ToString() != "." && !char.IsControl(e.KeyChar) && e.KeyChar.ToString() != "-")
                e.Handled = true;
        }

        private void txtFactorB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar.ToString() != "." && !char.IsControl(e.KeyChar) && e.KeyChar.ToString() != "-")
                e.Handled = true;
        }
    }
}
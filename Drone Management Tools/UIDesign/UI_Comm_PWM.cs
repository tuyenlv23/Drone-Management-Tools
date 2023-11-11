using System;
using System.Windows.Forms;
using Drone_Management_Tools.Models;

namespace Drone_Management_Tools.UIDesign
{
    public partial class UI_Comm_PWM : DevExpress.XtraEditors.XtraUserControl
    {
        private DronePwm _dronePwm;

        public UI_Comm_PWM(DronePwm dronePwm)
        {
            InitializeComponent();
            this._dronePwm = dronePwm;
        }

        private void UI_Comm_PWM_Load(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => ShowParameters(this._dronePwm)));
            else
                ShowParameters(this._dronePwm);
        }

        public void ShowParameters(DronePwm dronePwm)
        {
            if (dronePwm != null)
            {
                //--- PWM-1
                txtPwm1Freq.Text = dronePwm.DronePwm1.Frequency.ToString();
                txtPwm1Tmin.Text = dronePwm.DronePwm1.Tmin.ToString();
                txtPwm1Tmax.Text = dronePwm.DronePwm1.Tmax.ToString();

                //--- PWM-2
                txtPwm2Freq.Text = dronePwm.DronePwm2.Frequency.ToString();
                txtPwm2Tmin.Text = dronePwm.DronePwm2.Tmin.ToString();
                txtPwm2Tmax.Text = dronePwm.DronePwm2.Tmax.ToString();

                //--- PWM-3
                txtPwm3Freq.Text = dronePwm.DronePwm3.Frequency.ToString();
                txtPwm3Tmin.Text = dronePwm.DronePwm3.Tmin.ToString();
                txtPwm3Tmax.Text = dronePwm.DronePwm3.Tmax.ToString();

                //--- PWM-4
                txtPwm4Freq.Text = dronePwm.DronePwm4.Frequency.ToString();
                txtPwm4Tmin.Text = dronePwm.DronePwm4.Tmin.ToString();
                txtPwm4Tmax.Text = dronePwm.DronePwm4.Tmax.ToString();

                //--- PWM-5
                txtPwm5Freq.Text = dronePwm.DronePwm5.Frequency.ToString();
                txtPwm5Tmin.Text = dronePwm.DronePwm5.Tmin.ToString();
                txtPwm5Tmax.Text = dronePwm.DronePwm5.Tmax.ToString();

                //--- PWM-6
                txtPwm6Freq.Text = dronePwm.DronePwm6.Frequency.ToString();
                txtPwm6Tmin.Text = dronePwm.DronePwm6.Tmin.ToString();
                txtPwm6Tmax.Text = dronePwm.DronePwm6.Tmax.ToString();

                //--- PWM-7
                txtPwm7Freq.Text = dronePwm.DronePwm7.Frequency.ToString();
                txtPwm7Tmin.Text = dronePwm.DronePwm7.Tmin.ToString();
                txtPwm7Tmax.Text = dronePwm.DronePwm7.Tmax.ToString();

                //--- PWM-8
                txtPwm8Freq.Text = dronePwm.DronePwm8.Frequency.ToString();
                txtPwm8Tmin.Text = dronePwm.DronePwm8.Tmin.ToString();
                txtPwm8Tmax.Text = dronePwm.DronePwm8.Tmax.ToString();
            }
            else
            {

            }
        }

        public DronePwm GetParameters()
        {
            DronePwm _result = new DronePwm();

            //--- PWM-1
            _result.DronePwm1.Frequency = Convert.ToInt32(txtPwm1Freq.Text);
            _result.DronePwm1.Tmin = Convert.ToInt32(txtPwm1Tmin.Text);
            _result.DronePwm1.Tmax = Convert.ToInt32(txtPwm1Tmax.Text);

            //--- PWM-2
            _result.DronePwm2.Frequency = Convert.ToInt32(txtPwm2Freq.Text);
            _result.DronePwm2.Tmin = Convert.ToInt32(txtPwm2Tmin.Text);
            _result.DronePwm2.Tmax = Convert.ToInt32(txtPwm2Tmax.Text);

            //--- PWM-3
            _result.DronePwm3.Frequency = Convert.ToInt32(txtPwm3Freq.Text);
            _result.DronePwm3.Tmin = Convert.ToInt32(txtPwm3Tmin.Text);
            _result.DronePwm3.Tmax = Convert.ToInt32(txtPwm3Tmax.Text);

            //--- PWM-4
            _result.DronePwm4.Frequency = Convert.ToInt32(txtPwm4Freq.Text);
            _result.DronePwm4.Tmin = Convert.ToInt32(txtPwm4Tmin.Text);
            _result.DronePwm4.Tmax = Convert.ToInt32(txtPwm4Tmax.Text);

            //--- PWM-5
            _result.DronePwm5.Frequency = Convert.ToInt32(txtPwm5Freq.Text);
            _result.DronePwm5.Tmin = Convert.ToInt32(txtPwm5Tmin.Text);
            _result.DronePwm5.Tmax = Convert.ToInt32(txtPwm5Tmax.Text);

            //--- PWM-6
            _result.DronePwm6.Frequency = Convert.ToInt32(txtPwm6Freq.Text);
            _result.DronePwm6.Tmin = Convert.ToInt32(txtPwm6Tmin.Text);
            _result.DronePwm6.Tmax = Convert.ToInt32(txtPwm6Tmax.Text);

            //--- PWM-7
            _result.DronePwm7.Frequency = Convert.ToInt32(txtPwm7Freq.Text);
            _result.DronePwm7.Tmin = Convert.ToInt32(txtPwm7Tmin.Text);
            _result.DronePwm7.Tmax = Convert.ToInt32(txtPwm7Tmax.Text);

            //--- PWM-8
            _result.DronePwm8.Frequency = Convert.ToInt32(txtPwm8Freq.Text);
            _result.DronePwm8.Tmin = Convert.ToInt32(txtPwm8Tmin.Text);
            _result.DronePwm8.Tmax = Convert.ToInt32(txtPwm8Tmax.Text);

            return _result;
        }

        private void txtPwm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }
    }
}

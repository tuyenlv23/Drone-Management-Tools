using System;
using System.Windows.Forms;
using Drone_Management_Tools.Models;
using Drone_Management_Tools.Utilities;

namespace Drone_Management_Tools.UIDesign
{
    public partial class LibraryDeviceForm : DevExpress.XtraEditors.XtraForm
    {
        private DeviceLibrary _deviceLibrary;

        public LibraryDeviceForm(DeviceLibrary deviceLibrary)
        {
            InitializeComponent();

            CalculateFormSize();

            this._deviceLibrary = deviceLibrary;
        }

        private void LibraryDeviceForm_Load(object sender, EventArgs e)
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
            txtLibraryName.Text = this._deviceLibrary.VirtualLibraryInfo.Name;
            txtLibraryDesc.Text = this._deviceLibrary.VirtualLibraryInfo.Description;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            this._deviceLibrary.VirtualLibraryInfo.Name = txtLibraryName.Text;
            this._deviceLibrary.VirtualLibraryInfo.Description = txtLibraryDesc.Text;

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
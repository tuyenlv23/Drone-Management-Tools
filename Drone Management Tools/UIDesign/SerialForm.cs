using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drone_Management_Tools.UIDesign
{
    public partial class SerialForm : DevExpress.XtraEditors.XtraForm
    {
        int[] baudrates = { 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200 };
        int[] dataBits = { };
        int[] stopBits = { };
        int[] parityBits = { };

        public SerialForm()
        {
            InitializeComponent();
        }

        private void SerialForm_Load(object sender, EventArgs e)
        {
            InitParameters();
        }

        void InitParameters()
        {
            cbPort.DataSource = SerialPort.GetPortNames();
            cbBaudrate.DataSource = baudrates;
        }
    }
}
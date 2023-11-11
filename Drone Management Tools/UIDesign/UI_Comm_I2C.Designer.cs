namespace Drone_Management_Tools.UIDesign
{
    partial class UI_Comm_I2C
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpI2C = new System.Windows.Forms.GroupBox();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.cbMasterSlaveMode = new System.Windows.Forms.ComboBox();
            this.txtDataSize = new System.Windows.Forms.TextBox();
            this.cbConditionRepeat = new System.Windows.Forms.ComboBox();
            this.cbAckMode = new System.Windows.Forms.ComboBox();
            this.cbStartStopBits = new System.Windows.Forms.ComboBox();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.grpI2C.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // grpI2C
            // 
            this.grpI2C.AutoSize = true;
            this.grpI2C.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpI2C.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grpI2C.Controls.Add(this.layoutControl1);
            this.grpI2C.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpI2C.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpI2C.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.grpI2C.Location = new System.Drawing.Point(0, 0);
            this.grpI2C.Name = "grpI2C";
            this.grpI2C.Size = new System.Drawing.Size(352, 324);
            this.grpI2C.TabIndex = 0;
            this.grpI2C.TabStop = false;
            this.grpI2C.Text = "I2C Parameters";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.txtAddress);
            this.layoutControl1.Controls.Add(this.txtSpeed);
            this.layoutControl1.Controls.Add(this.cbMasterSlaveMode);
            this.layoutControl1.Controls.Add(this.txtDataSize);
            this.layoutControl1.Controls.Add(this.cbConditionRepeat);
            this.layoutControl1.Controls.Add(this.cbAckMode);
            this.layoutControl1.Controls.Add(this.cbStartStopBits);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(3, 19);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(410, 213, 650, 400);
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(346, 302);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // txtAddress
            // 
            this.txtAddress.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress.Location = new System.Drawing.Point(123, 14);
            this.txtAddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(165, 27);
            this.txtAddress.TabIndex = 0;
            // 
            // txtSpeed
            // 
            this.txtSpeed.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSpeed.Location = new System.Drawing.Point(123, 49);
            this.txtSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(165, 27);
            this.txtSpeed.TabIndex = 1;
            this.txtSpeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSpeed_KeyPress);
            // 
            // cbMasterSlaveMode
            // 
            this.cbMasterSlaveMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMasterSlaveMode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbMasterSlaveMode.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMasterSlaveMode.FormattingEnabled = true;
            this.cbMasterSlaveMode.Location = new System.Drawing.Point(123, 84);
            this.cbMasterSlaveMode.Name = "cbMasterSlaveMode";
            this.cbMasterSlaveMode.Size = new System.Drawing.Size(165, 25);
            this.cbMasterSlaveMode.TabIndex = 2;
            // 
            // txtDataSize
            // 
            this.txtDataSize.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDataSize.Location = new System.Drawing.Point(123, 119);
            this.txtDataSize.Margin = new System.Windows.Forms.Padding(4);
            this.txtDataSize.Name = "txtDataSize";
            this.txtDataSize.Size = new System.Drawing.Size(165, 27);
            this.txtDataSize.TabIndex = 3;
            this.txtDataSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDataSize_KeyPress);
            // 
            // cbConditionRepeat
            // 
            this.cbConditionRepeat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConditionRepeat.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbConditionRepeat.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbConditionRepeat.FormattingEnabled = true;
            this.cbConditionRepeat.Location = new System.Drawing.Point(123, 227);
            this.cbConditionRepeat.Name = "cbConditionRepeat";
            this.cbConditionRepeat.Size = new System.Drawing.Size(165, 25);
            this.cbConditionRepeat.TabIndex = 6;
            // 
            // cbAckMode
            // 
            this.cbAckMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAckMode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbAckMode.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAckMode.FormattingEnabled = true;
            this.cbAckMode.Location = new System.Drawing.Point(123, 154);
            this.cbAckMode.Name = "cbAckMode";
            this.cbAckMode.Size = new System.Drawing.Size(165, 25);
            this.cbAckMode.TabIndex = 4;
            // 
            // cbStartStopBits
            // 
            this.cbStartStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStartStopBits.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbStartStopBits.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbStartStopBits.FormattingEnabled = true;
            this.cbStartStopBits.Location = new System.Drawing.Point(123, 189);
            this.cbStartStopBits.Name = "cbStartStopBits";
            this.cbStartStopBits.Size = new System.Drawing.Size(165, 25);
            this.cbStartStopBits.TabIndex = 5;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.emptySpaceItem2,
            this.emptySpaceItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(346, 302);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.Control = this.txtAddress;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(0, 35);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(200, 35);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(280, 35);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 2, 2);
            this.layoutControlItem1.Text = "Address:";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(99, 16);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem2.Control = this.txtSpeed;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 35);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(0, 35);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(200, 35);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(280, 35);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 2, 2);
            this.layoutControlItem2.Text = "Speed:";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(99, 16);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem3.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem3.Control = this.cbMasterSlaveMode;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 70);
            this.layoutControlItem3.MaxSize = new System.Drawing.Size(0, 35);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(200, 35);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(280, 35);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 2, 2);
            this.layoutControlItem3.Text = "Master/Slave:";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(99, 16);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem4.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem4.Control = this.txtDataSize;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 105);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(0, 35);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(200, 35);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(280, 35);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 2, 2);
            this.layoutControlItem4.Text = "Data Size:";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(99, 16);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem5.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem5.Control = this.cbAckMode;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 140);
            this.layoutControlItem5.MaxSize = new System.Drawing.Size(0, 35);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(200, 35);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(280, 35);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 2, 2);
            this.layoutControlItem5.Text = "Ack Mode:";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(99, 16);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem6.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem6.Control = this.cbStartStopBits;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 175);
            this.layoutControlItem6.MaxSize = new System.Drawing.Size(0, 35);
            this.layoutControlItem6.MinSize = new System.Drawing.Size(200, 35);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(280, 35);
            this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem6.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 2, 2);
            this.layoutControlItem6.Text = "Start/Stop Bits:";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(99, 16);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem7.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem7.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem7.Control = this.cbConditionRepeat;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 210);
            this.layoutControlItem7.MaxSize = new System.Drawing.Size(0, 35);
            this.layoutControlItem7.MinSize = new System.Drawing.Size(200, 35);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(280, 35);
            this.layoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem7.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 2, 2);
            this.layoutControlItem7.Text = "Conditon Repeat:";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(99, 16);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(280, 0);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(46, 282);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 245);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(280, 37);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // UI_Comm_I2C
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpI2C);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UI_Comm_I2C";
            this.Size = new System.Drawing.Size(352, 324);
            this.Load += new System.EventHandler(this.UI_Comm_I2C_Load);
            this.grpI2C.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpI2C;
        private System.Windows.Forms.ComboBox cbAckMode;
        private System.Windows.Forms.ComboBox cbMasterSlaveMode;
        private System.Windows.Forms.TextBox txtSpeed;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.ComboBox cbConditionRepeat;
        private System.Windows.Forms.TextBox txtDataSize;
        private System.Windows.Forms.ComboBox cbStartStopBits;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    }
}

namespace Kevsoft.LiveControl.GUIs
{
    partial class FixturePossitionControl
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
            this.lockXCheckBox = new System.Windows.Forms.CheckBox();
            this.lockYCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.shapeOffSetComboBox = new System.Windows.Forms.ComboBox();
            this.shapeSpeedComboBox = new System.Windows.Forms.ComboBox();
            this.shapeSizeYNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.shapeSizeXNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.shapeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.possition1 = new Kevsoft.LiveControl.WFCL.Possition();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shapeSizeYNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shapeSizeXNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // lockXCheckBox
            // 
            this.lockXCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.lockXCheckBox.Location = new System.Drawing.Point(14, 20);
            this.lockXCheckBox.Name = "lockXCheckBox";
            this.lockXCheckBox.Size = new System.Drawing.Size(26, 26);
            this.lockXCheckBox.TabIndex = 0;
            this.lockXCheckBox.Text = "X";
            this.lockXCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lockXCheckBox.UseVisualStyleBackColor = true;
            this.lockXCheckBox.CheckedChanged += new System.EventHandler(this.lockXCheckBox_CheckedChanged);
            // 
            // lockYCheckBox
            // 
            this.lockYCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.lockYCheckBox.Location = new System.Drawing.Point(46, 20);
            this.lockYCheckBox.Name = "lockYCheckBox";
            this.lockYCheckBox.Size = new System.Drawing.Size(26, 26);
            this.lockYCheckBox.TabIndex = 1;
            this.lockYCheckBox.Text = "Y";
            this.lockYCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lockYCheckBox.UseVisualStyleBackColor = true;
            this.lockYCheckBox.CheckedChanged += new System.EventHandler(this.lockYCheckBox_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lockXCheckBox);
            this.groupBox1.Controls.Add(this.lockYCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(229, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(90, 52);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Locks";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.shapeOffSetComboBox);
            this.groupBox2.Controls.Add(this.shapeSpeedComboBox);
            this.groupBox2.Controls.Add(this.shapeSizeYNumericUpDown);
            this.groupBox2.Controls.Add(this.shapeSizeXNumericUpDown);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.shapeComboBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(229, 61);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(90, 162);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Shape";
            // 
            // shapeOffSetComboBox
            // 
            this.shapeOffSetComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.shapeOffSetComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shapeOffSetComboBox.FormattingEnabled = true;
            this.shapeOffSetComboBox.Location = new System.Drawing.Point(38, 127);
            this.shapeOffSetComboBox.Name = "shapeOffSetComboBox";
            this.shapeOffSetComboBox.Size = new System.Drawing.Size(46, 21);
            this.shapeOffSetComboBox.TabIndex = 9;
            // 
            // shapeSpeedComboBox
            // 
            this.shapeSpeedComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.shapeSpeedComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shapeSpeedComboBox.FormattingEnabled = true;
            this.shapeSpeedComboBox.Location = new System.Drawing.Point(38, 105);
            this.shapeSpeedComboBox.Name = "shapeSpeedComboBox";
            this.shapeSpeedComboBox.Size = new System.Drawing.Size(46, 21);
            this.shapeSpeedComboBox.TabIndex = 7;
            // 
            // shapeSizeYNumericUpDown
            // 
            this.shapeSizeYNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.shapeSizeYNumericUpDown.Location = new System.Drawing.Point(33, 79);
            this.shapeSizeYNumericUpDown.Name = "shapeSizeYNumericUpDown";
            this.shapeSizeYNumericUpDown.Size = new System.Drawing.Size(51, 20);
            this.shapeSizeYNumericUpDown.TabIndex = 5;
            // 
            // shapeSizeXNumericUpDown
            // 
            this.shapeSizeXNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.shapeSizeXNumericUpDown.Location = new System.Drawing.Point(33, 59);
            this.shapeSizeXNumericUpDown.Name = "shapeSizeXNumericUpDown";
            this.shapeSizeXNumericUpDown.Size = new System.Drawing.Size(51, 20);
            this.shapeSizeXNumericUpDown.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "y:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "x:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Size";
            // 
            // shapeComboBox
            // 
            this.shapeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.shapeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shapeComboBox.FormattingEnabled = true;
            this.shapeComboBox.Items.AddRange(new object[] {
            "None",
            "Circle"});
            this.shapeComboBox.Location = new System.Drawing.Point(6, 19);
            this.shapeComboBox.Name = "shapeComboBox";
            this.shapeComboBox.Size = new System.Drawing.Size(78, 21);
            this.shapeComboBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Speed";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Off Set";
            // 
            // possition1
            // 
            this.possition1.AutoMovePointer = true;
            this.possition1.BackColor = System.Drawing.Color.White;
            this.possition1.Location = new System.Drawing.Point(3, 3);
            this.possition1.LockX = false;
            this.possition1.LockY = false;
            this.possition1.MaxXValue = 100;
            this.possition1.MaxYValue = 100;
            this.possition1.MinXValue = 0;
            this.possition1.MinYValue = 0;
            this.possition1.Name = "possition1";
            this.possition1.PointerColor = System.Drawing.Color.DarkBlue;
            this.possition1.PointerSize = new System.Drawing.Size(10, 10);
            this.possition1.PointerThickness = 2F;
            this.possition1.Size = new System.Drawing.Size(220, 220);
            this.possition1.TabIndex = 0;
            this.possition1.Value = new System.Drawing.Point(50, 50);
            this.possition1.XValue = 50;
            this.possition1.YValue = 50;
            this.possition1.Paint += new System.Windows.Forms.PaintEventHandler(this.possition1_Paint);
            // 
            // FixturePossitionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.possition1);
            this.Name = "FixturePossitionControl";
            this.Size = new System.Drawing.Size(322, 228);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shapeSizeYNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shapeSizeXNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Kevsoft.LiveControl.WFCL.Possition possition1;
        private System.Windows.Forms.CheckBox lockXCheckBox;
        private System.Windows.Forms.CheckBox lockYCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox shapeComboBox;
        private System.Windows.Forms.NumericUpDown shapeSizeYNumericUpDown;
        private System.Windows.Forms.NumericUpDown shapeSizeXNumericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox shapeOffSetComboBox;
        private System.Windows.Forms.ComboBox shapeSpeedComboBox;
        private System.Windows.Forms.Label label5;


    }
}

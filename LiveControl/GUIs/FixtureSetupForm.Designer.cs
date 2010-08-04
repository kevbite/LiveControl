namespace Kevsoft.LiveControl.GUIs
{
    partial class FixtureSetupForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.addPersonalityButton = new System.Windows.Forms.Button();
            this.Patchbutton = new System.Windows.Forms.Button();
            this.removePatchButton = new System.Windows.Forms.Button();
            this.personalitiesListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.fixturesListView = new System.Windows.Forms.ListView();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.createPersonalityButton = new System.Windows.Forms.Button();
            this.universeComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.setButton = new System.Windows.Forms.Button();
            this.setDMXchNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.setDMXchNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Personalities Loaded:";
            // 
            // addPersonalityButton
            // 
            this.addPersonalityButton.Location = new System.Drawing.Point(15, 281);
            this.addPersonalityButton.Name = "addPersonalityButton";
            this.addPersonalityButton.Size = new System.Drawing.Size(75, 23);
            this.addPersonalityButton.TabIndex = 2;
            this.addPersonalityButton.Text = "&Add";
            this.addPersonalityButton.UseVisualStyleBackColor = true;
            this.addPersonalityButton.Click += new System.EventHandler(this.addPersonalityButton_Click);
            // 
            // Patchbutton
            // 
            this.Patchbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Patchbutton.Location = new System.Drawing.Point(202, 104);
            this.Patchbutton.Name = "Patchbutton";
            this.Patchbutton.Size = new System.Drawing.Size(46, 43);
            this.Patchbutton.TabIndex = 4;
            this.Patchbutton.Text = ">>";
            this.Patchbutton.UseVisualStyleBackColor = true;
            this.Patchbutton.Click += new System.EventHandler(this.Patchbutton_Click);
            // 
            // removePatchButton
            // 
            this.removePatchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removePatchButton.Location = new System.Drawing.Point(202, 153);
            this.removePatchButton.Name = "removePatchButton";
            this.removePatchButton.Size = new System.Drawing.Size(46, 43);
            this.removePatchButton.TabIndex = 5;
            this.removePatchButton.Text = "<<";
            this.removePatchButton.UseVisualStyleBackColor = true;
            this.removePatchButton.Click += new System.EventHandler(this.removePatchButton_Click);
            // 
            // personalitiesListView
            // 
            this.personalitiesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.personalitiesListView.FullRowSelect = true;
            this.personalitiesListView.HideSelection = false;
            this.personalitiesListView.Location = new System.Drawing.Point(15, 28);
            this.personalitiesListView.MultiSelect = false;
            this.personalitiesListView.Name = "personalitiesListView";
            this.personalitiesListView.ShowItemToolTips = true;
            this.personalitiesListView.Size = new System.Drawing.Size(181, 247);
            this.personalitiesListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.personalitiesListView.TabIndex = 1;
            this.personalitiesListView.UseCompatibleStateImageBehavior = false;
            this.personalitiesListView.View = System.Windows.Forms.View.Details;
            this.personalitiesListView.DoubleClick += new System.EventHandler(this.personalitiesListView_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 140;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Manufacture";
            this.columnHeader2.Width = 0;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Channels";
            this.columnHeader3.Width = 36;
            // 
            // fixturesListView
            // 
            this.fixturesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
            this.fixturesListView.FullRowSelect = true;
            this.fixturesListView.HideSelection = false;
            this.fixturesListView.LabelEdit = true;
            this.fixturesListView.Location = new System.Drawing.Point(254, 55);
            this.fixturesListView.Name = "fixturesListView";
            this.fixturesListView.ShowItemToolTips = true;
            this.fixturesListView.Size = new System.Drawing.Size(193, 220);
            this.fixturesListView.TabIndex = 9;
            this.fixturesListView.UseCompatibleStateImageBehavior = false;
            this.fixturesListView.View = System.Windows.Forms.View.Details;
            this.fixturesListView.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.fixturesListView_AfterLabelEdit);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Name";
            this.columnHeader4.Width = 121;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Dmx Patch";
            this.columnHeader5.Width = 65;
            // 
            // createPersonalityButton
            // 
            this.createPersonalityButton.Location = new System.Drawing.Point(121, 281);
            this.createPersonalityButton.Name = "createPersonalityButton";
            this.createPersonalityButton.Size = new System.Drawing.Size(75, 23);
            this.createPersonalityButton.TabIndex = 3;
            this.createPersonalityButton.Text = "&Create";
            this.createPersonalityButton.UseVisualStyleBackColor = true;
            this.createPersonalityButton.Click += new System.EventHandler(this.createPersonalityButton_Click);
            // 
            // universeComboBox
            // 
            this.universeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.universeComboBox.FormattingEnabled = true;
            this.universeComboBox.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.universeComboBox.Location = new System.Drawing.Point(309, 28);
            this.universeComboBox.Name = "universeComboBox";
            this.universeComboBox.Size = new System.Drawing.Size(138, 21);
            this.universeComboBox.TabIndex = 8;
            this.universeComboBox.SelectedIndexChanged += new System.EventHandler(this.universeComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(251, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Universe:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(251, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Patched Fixtures:";
            // 
            // setButton
            // 
            this.setButton.Location = new System.Drawing.Point(372, 281);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(75, 23);
            this.setButton.TabIndex = 12;
            this.setButton.Text = "Set";
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // setDMXchNumericUpDown
            // 
            this.setDMXchNumericUpDown.Location = new System.Drawing.Point(309, 282);
            this.setDMXchNumericUpDown.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.setDMXchNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.setDMXchNumericUpDown.Name = "setDMXchNumericUpDown";
            this.setDMXchNumericUpDown.Size = new System.Drawing.Size(57, 20);
            this.setDMXchNumericUpDown.TabIndex = 11;
            this.setDMXchNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(253, 286);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "DMX Ch:";
            // 
            // FixtureSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 343);
            this.Controls.Add(this.setDMXchNumericUpDown);
            this.Controls.Add(this.setButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.universeComboBox);
            this.Controls.Add(this.fixturesListView);
            this.Controls.Add(this.createPersonalityButton);
            this.Controls.Add(this.personalitiesListView);
            this.Controls.Add(this.removePatchButton);
            this.Controls.Add(this.Patchbutton);
            this.Controls.Add(this.addPersonalityButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FixtureSetupForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Fixture Setup";
            this.Load += new System.EventHandler(this.FixtureSetupForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.setDMXchNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button addPersonalityButton;
        private System.Windows.Forms.Button Patchbutton;
        private System.Windows.Forms.Button removePatchButton;
        private System.Windows.Forms.ListView personalitiesListView;
        private System.Windows.Forms.ListView fixturesListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button createPersonalityButton;
        private System.Windows.Forms.ComboBox universeComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button setButton;
        private System.Windows.Forms.NumericUpDown setDMXchNumericUpDown;
        private System.Windows.Forms.Label label4;
    }
}
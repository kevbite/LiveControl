using Kevsoft.LiveControl.WFCL;
namespace Kevsoft.LiveControl.GUIs
{
    partial class AttributeControl
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

            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.preSetsButton = new System.Windows.Forms.Button();
            this.nameLabel = new System.Windows.Forms.Label();
            this.valueLabel = new System.Windows.Forms.Label();
            this.avFader1 = new Kevsoft.LiveControl.WFCL.AVFader();
            this.SuspendLayout();
            // 
            // preSetsButton
            // 
            this.preSetsButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.preSetsButton.Location = new System.Drawing.Point(11, 192);
            this.preSetsButton.Name = "preSetsButton";
            this.preSetsButton.Size = new System.Drawing.Size(40, 33);
            this.preSetsButton.TabIndex = 3;
            this.preSetsButton.UseVisualStyleBackColor = true;
            this.preSetsButton.Click += new System.EventHandler(this.preSetsButton_Click);
            // 
            // nameLabel
            // 
            this.nameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameLabel.Location = new System.Drawing.Point(0, 5);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(63, 15);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Name";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // valueLabel
            // 
            this.valueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.valueLabel.Location = new System.Drawing.Point(0, 174);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(63, 15);
            this.valueLabel.TabIndex = 2;
            this.valueLabel.Text = "Value";
            this.valueLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // avFader1
            // 
            this.avFader1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.avFader1.Location = new System.Drawing.Point(11, 18);
            this.avFader1.Maximum = 255;
            this.avFader1.Name = "avFader1";
            this.avFader1.Size = new System.Drawing.Size(40, 153);
            this.avFader1.TabIndex = 1;
            this.avFader1.TickColor = System.Drawing.Color.Black;
            this.avFader1.TickFrequency = 10;
            // 
            // AttributeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.valueLabel);
            this.Controls.Add(this.preSetsButton);
            this.Controls.Add(this.avFader1);
            this.DoubleBuffered = true;
            this.Name = "AttributeControl";
            this.Size = new System.Drawing.Size(63, 228);
            this.ResumeLayout(false);

        }

        #endregion

        private AVFader avFader1;
        private System.Windows.Forms.Button preSetsButton;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label valueLabel;

    }
}

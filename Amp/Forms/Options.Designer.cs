namespace Amp
{
    partial class Options
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
			this.cycleButton = new System.Windows.Forms.Button();
			this.muteButton = new System.Windows.Forms.Button();
			this.stateCheck = new System.Windows.Forms.CheckBox();
			this.bootCheck = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// cycleButton
			// 
			this.cycleButton.Location = new System.Drawing.Point(12, 12);
			this.cycleButton.Name = "cycleButton";
			this.cycleButton.Size = new System.Drawing.Size(260, 32);
			this.cycleButton.TabIndex = 1;
			this.cycleButton.Text = "cycleButton";
			this.cycleButton.UseVisualStyleBackColor = true;
			this.cycleButton.Click += new System.EventHandler(this.cycleButton_Click);
			// 
			// muteButton
			// 
			this.muteButton.Location = new System.Drawing.Point(12, 50);
			this.muteButton.Name = "muteButton";
			this.muteButton.Size = new System.Drawing.Size(260, 32);
			this.muteButton.TabIndex = 2;
			this.muteButton.Text = "muteButton";
			this.muteButton.UseVisualStyleBackColor = true;
			this.muteButton.Click += new System.EventHandler(this.muteButton_Click);
			// 
			// stateCheck
			// 
			this.stateCheck.AutoSize = true;
			this.stateCheck.Location = new System.Drawing.Point(12, 89);
			this.stateCheck.Name = "stateCheck";
			this.stateCheck.Size = new System.Drawing.Size(164, 17);
			this.stateCheck.TabIndex = 3;
			this.stateCheck.Text = "Play sound on changed state";
			this.stateCheck.UseVisualStyleBackColor = true;
			this.stateCheck.CheckedChanged += new System.EventHandler(this.stateCheck_CheckedChanged);
			// 
			// bootCheck
			// 
			this.bootCheck.AutoSize = true;
			this.bootCheck.Location = new System.Drawing.Point(12, 113);
			this.bootCheck.Name = "bootCheck";
			this.bootCheck.Size = new System.Drawing.Size(117, 17);
			this.bootCheck.TabIndex = 4;
			this.bootCheck.Text = "Start with Windows";
			this.bootCheck.UseVisualStyleBackColor = true;
			this.bootCheck.CheckedChanged += new System.EventHandler(this.bootCheck_CheckedChanged);
			// 
			// Options
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 137);
			this.Controls.Add(this.bootCheck);
			this.Controls.Add(this.stateCheck);
			this.Controls.Add(this.muteButton);
			this.Controls.Add(this.cycleButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Options";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Amp";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Button cycleButton;
		private System.Windows.Forms.Button muteButton;
		private System.Windows.Forms.CheckBox stateCheck;
		private System.Windows.Forms.CheckBox bootCheck;
    }
}
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
			this.cycleButton = new System.Windows.Forms.Button();
			this.cycleNoteButton = new System.Windows.Forms.Button();
			this.muteButton = new System.Windows.Forms.Button();
			this.muteNoteButton = new System.Windows.Forms.Button();
			this.bootCheck = new System.Windows.Forms.CheckBox();
			this.balloonCheck = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// cycleButton
			// 
			this.cycleButton.Location = new System.Drawing.Point(12, 12);
			this.cycleButton.Name = "cycleButton";
			this.cycleButton.Size = new System.Drawing.Size(222, 32);
			this.cycleButton.TabIndex = 1;
			this.cycleButton.Text = "cycleButton";
			this.cycleButton.UseVisualStyleBackColor = true;
			this.cycleButton.Click += new System.EventHandler(this.cycleButton_Click);
			// 
			// cycleNoteButton
			// 
			this.cycleNoteButton.Location = new System.Drawing.Point(240, 12);
			this.cycleNoteButton.Name = "cycleNoteButton";
			this.cycleNoteButton.Size = new System.Drawing.Size(32, 32);
			this.cycleNoteButton.TabIndex = 2;
			this.cycleNoteButton.UseVisualStyleBackColor = true;
			this.cycleNoteButton.Click += new System.EventHandler(this.cycleNoteButton_Click);
			// 
			// muteButton
			// 
			this.muteButton.Location = new System.Drawing.Point(12, 50);
			this.muteButton.Name = "muteButton";
			this.muteButton.Size = new System.Drawing.Size(222, 32);
			this.muteButton.TabIndex = 3;
			this.muteButton.Text = "muteButton";
			this.muteButton.UseVisualStyleBackColor = true;
			this.muteButton.Click += new System.EventHandler(this.muteButton_Click);
			// 
			// muteNoteButton
			// 
			this.muteNoteButton.Location = new System.Drawing.Point(240, 50);
			this.muteNoteButton.Name = "muteNoteButton";
			this.muteNoteButton.Size = new System.Drawing.Size(32, 32);
			this.muteNoteButton.TabIndex = 4;
			this.muteNoteButton.UseVisualStyleBackColor = true;
			this.muteNoteButton.Click += new System.EventHandler(this.muteNoteButton_Click);
			// 
			// bootCheck
			// 
			this.bootCheck.AutoSize = true;
			this.bootCheck.Location = new System.Drawing.Point(13, 88);
			this.bootCheck.Name = "bootCheck";
			this.bootCheck.Size = new System.Drawing.Size(117, 17);
			this.bootCheck.TabIndex = 5;
			this.bootCheck.Text = "Start with Windows";
			this.bootCheck.UseVisualStyleBackColor = true;
			this.bootCheck.CheckedChanged += new System.EventHandler(this.bootCheck_CheckedChanged);
			// 
			// balloonCheck
			// 
			this.balloonCheck.AutoSize = true;
			this.balloonCheck.Location = new System.Drawing.Point(13, 111);
			this.balloonCheck.Name = "balloonCheck";
			this.balloonCheck.Size = new System.Drawing.Size(132, 17);
			this.balloonCheck.TabIndex = 6;
			this.balloonCheck.Text = "Show tray notifications";
			this.balloonCheck.UseVisualStyleBackColor = true;
			this.balloonCheck.CheckedChanged += new System.EventHandler(this.balloonCheck_CheckedChanged);
			// 
			// Options
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 134);
			this.Controls.Add(this.balloonCheck);
			this.Controls.Add(this.bootCheck);
			this.Controls.Add(this.muteNoteButton);
			this.Controls.Add(this.muteButton);
			this.Controls.Add(this.cycleNoteButton);
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
		private System.Windows.Forms.Button cycleNoteButton;
		private System.Windows.Forms.Button muteButton;
		private System.Windows.Forms.Button muteNoteButton;
		private System.Windows.Forms.CheckBox bootCheck;
		private System.Windows.Forms.CheckBox balloonCheck;
	}
}
using System;
using System.Windows.Forms;

namespace RDLCReports
{
	partial class Form1 : Form
	{
		private Label label1;
		private Button button1;

		// Required designer variable
		private System.ComponentModel.IContainer components = null;

		// Constructor
		public Form1()
		{
			InitializeComponent();
		}

		// Clean up any resources being used
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		// Required method for Designer support - do not modify
		// the contents of this method with the code editor
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(300, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(198, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "STORE MASTER REPORT";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(344, 67);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(94, 43);
			this.button1.TabIndex = 1;
			this.button1.Text = "SUBMIT";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
	}
}

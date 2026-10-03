namespace Battleship
{
	partial class ConfigForm
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
			propertyGrid1 = new PropertyGrid();
			btnCancel = new Button();
			btnOk = new Button();
			SuspendLayout();
			// 
			// propertyGrid1
			// 
			propertyGrid1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			propertyGrid1.BackColor = SystemColors.Control;
			propertyGrid1.Location = new Point(12, 12);
			propertyGrid1.Name = "propertyGrid1";
			propertyGrid1.Size = new Size(308, 522);
			propertyGrid1.TabIndex = 0;
			// 
			// btnCancel
			// 
			btnCancel.Anchor = AnchorStyles.Bottom;
			btnCancel.Location = new Point(39, 547);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(94, 29);
			btnCancel.TabIndex = 1;
			btnCancel.Text = "Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			btnCancel.Click += btnCancel_Click;
			// 
			// btnOk
			// 
			btnOk.Anchor = AnchorStyles.Bottom;
			btnOk.Location = new Point(191, 547);
			btnOk.Name = "btnOk";
			btnOk.Size = new Size(94, 29);
			btnOk.TabIndex = 2;
			btnOk.Text = "OK";
			btnOk.UseVisualStyleBackColor = true;
			btnOk.Click += btnOk_Click;
			// 
			// ConfigForm
			// 
			AcceptButton = btnOk;
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = btnCancel;
			ClientSize = new Size(325, 588);
			Controls.Add(btnOk);
			Controls.Add(btnCancel);
			Controls.Add(propertyGrid1);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MinimumSize = new Size(343, 333);
			Name = "ConfigForm";
			ShowIcon = false;
			SizeGripStyle = SizeGripStyle.Hide;
			Text = "ConfigForm";
			ResumeLayout(false);
		}

		#endregion

		private PropertyGrid propertyGrid1;
		private Button btnCancel;
		private Button btnOk;
	}
}
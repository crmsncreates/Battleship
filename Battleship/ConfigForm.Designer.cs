namespace Battleship
{
	partial class ConfigForm
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && components is not null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			propertyGrid1 = new PropertyGrid();
			fleetTable = new DataGridView();
			shipName = new DataGridViewTextBoxColumn();
			shipLength = new DataGridViewTextBoxColumn();
			lblStatus = new Label();
			btnNext = new Button();
			btnCancel = new Button();
			((System.ComponentModel.ISupportInitialize)fleetTable).BeginInit();
			SuspendLayout();
			// 
			// propertyGrid1
			// 
			propertyGrid1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			propertyGrid1.BackColor = SystemColors.Control;
			propertyGrid1.Location = new Point(12, 12);
			propertyGrid1.Name = "propertyGrid1";
			propertyGrid1.Size = new Size(416, 160);
			propertyGrid1.TabIndex = 0;
			propertyGrid1.ToolbarVisible = false;
			// 
			// fleetTable
			// 
			fleetTable.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			fleetTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			fleetTable.Columns.AddRange(new DataGridViewColumn[] { shipName, shipLength });
			fleetTable.Location = new Point(12, 184);
			fleetTable.Name = "fleetTable";
			fleetTable.Size = new Size(416, 210);
			fleetTable.TabIndex = 1;
			fleetTable.DataError += fleetTable_DataError;
			// 
			// shipName
			// 
			shipName.DataPropertyName = "Name";
			shipName.HeaderText = "ship name";
			shipName.Name = "shipName";
			// 
			// shipLength
			// 
			shipLength.DataPropertyName = "Length";
			shipLength.HeaderText = "length";
			shipLength.Name = "shipLength";
			// 
			// lblStatus
			// 
			lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			lblStatus.Location = new Point(12, 402);
			lblStatus.Name = "lblStatus";
			lblStatus.Size = new Size(416, 61);
			lblStatus.TabIndex = 2;
			lblStatus.Text = "edit rules and ship lengths. add a ship in the blank row; select a row and press delete to remove it. click next to arrange formations.";
			// 
			// btnNext
			// 
			btnNext.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnNext.Location = new Point(272, 475);
			btnNext.Name = "btnNext";
			btnNext.Size = new Size(75, 23);
			btnNext.TabIndex = 2;
			btnNext.Text = "next >";
			btnNext.Click += btnNext_Click;
			// 
			// btnCancel
			// 
			btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnCancel.CausesValidation = false;
			btnCancel.Location = new Point(353, 475);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(75, 23);
			btnCancel.TabIndex = 3;
			btnCancel.Text = "cancel";
			btnCancel.Click += btnCancel_Click;
			// 
			// ConfigForm
			// 
			AcceptButton = btnNext;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = btnCancel;
			ClientSize = new Size(440, 510);
			Controls.Add(propertyGrid1);
			Controls.Add(fleetTable);
			Controls.Add(lblStatus);
			Controls.Add(btnNext);
			Controls.Add(btnCancel);
			MinimizeBox = false;
			MinimumSize = new Size(456, 549);
			Name = "ConfigForm";
			ShowIcon = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "game rules";
			((System.ComponentModel.ISupportInitialize)fleetTable).EndInit();
			ResumeLayout(false);
		}
		#endregion

		private PropertyGrid propertyGrid1;
		private DataGridView fleetTable;
		private DataGridViewTextBoxColumn shipName;
		private DataGridViewTextBoxColumn shipLength;
		private Label lblStatus;
		private Button btnNext;
		private Button btnCancel;
	}
}

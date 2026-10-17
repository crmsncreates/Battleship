namespace Battleship
{
	partial class FormationForm
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
			boardTable = new TableLayoutPanel();
			lstShips = new ListBox();
			lblStatus = new Label();
			btnReady = new Button();
			btnRotate = new Button();
			btnLock = new Button();
			btnRules = new Button();
			btnOk = new Button();
			btnCancel = new Button();
			SuspendLayout();
			// 
			// boardTable
			// 
			boardTable.AllowDrop = true;
			boardTable.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			boardTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
			boardTable.ColumnCount = 10;
			boardTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
			boardTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
			boardTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
			boardTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
			boardTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
			boardTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
			boardTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
			boardTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
			boardTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
			boardTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
			boardTable.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
			boardTable.Location = new Point(318, 12);
			boardTable.Name = "boardTable";
			boardTable.RowCount = 10;
			boardTable.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			boardTable.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			boardTable.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			boardTable.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			boardTable.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			boardTable.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			boardTable.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			boardTable.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			boardTable.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			boardTable.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			boardTable.Size = new Size(423, 423);
			boardTable.TabIndex = 0;
			boardTable.DragDrop += boardTable_DragDrop;
			boardTable.DragEnter += boardTable_DragEnter;
			boardTable.DragOver += boardTable_DragEnter;
			// 
			// lstShips
			// 
			lstShips.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
			lstShips.IntegralHeight = false;
			lstShips.Location = new Point(12, 12);
			lstShips.Name = "lstShips";
			lstShips.Size = new Size(300, 243);
			lstShips.TabIndex = 0;
			lstShips.MouseDown += lstShips_MouseDown;
			// 
			// lblStatus
			// 
			lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			lblStatus.Location = new Point(12, 327);
			lblStatus.Name = "lblStatus";
			lblStatus.Size = new Size(300, 108);
			lblStatus.TabIndex = 1;
			// 
			// btnReady
			// 
			btnReady.Location = new Point(328, 225);
			btnReady.Name = "btnReady";
			btnReady.Size = new Size(180, 23);
			btnReady.TabIndex = 3;
			btnReady.Text = "ready";
			btnReady.Click += btnReady_Click;
			// 
			// btnRotate
			// 
			btnRotate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			btnRotate.Location = new Point(12, 267);
			btnRotate.Name = "btnRotate";
			btnRotate.Size = new Size(300, 23);
			btnRotate.TabIndex = 1;
			btnRotate.Text = "rotate: horizontal";
			btnRotate.Click += btnRotate_Click;
			// 
			// btnLock
			// 
			btnLock.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			btnLock.Location = new Point(12, 296);
			btnLock.Name = "btnLock";
			btnLock.Size = new Size(300, 23);
			btnLock.TabIndex = 2;
			btnLock.Text = "lock formation";
			btnLock.Click += btnLock_Click;
			// 
			// btnRules
			// 
			btnRules.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			btnRules.Location = new Point(12, 438);
			btnRules.Name = "btnRules";
			btnRules.Size = new Size(101, 23);
			btnRules.TabIndex = 4;
			btnRules.Text = "back to rules";
			btnRules.Click += btnRules_Click;
			// 
			// btnOk
			// 
			btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnOk.Location = new Point(646, 438);
			btnOk.Name = "btnOk";
			btnOk.Size = new Size(95, 23);
			btnOk.TabIndex = 5;
			btnOk.Text = "start game";
			btnOk.Click += btnOk_Click;
			// 
			// btnCancel
			// 
			btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnCancel.CausesValidation = false;
			btnCancel.Location = new Point(565, 438);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(75, 23);
			btnCancel.TabIndex = 6;
			btnCancel.Text = "cancel";
			btnCancel.Click += btnCancel_Click;
			// 
			// FormationForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = btnCancel;
			ClientSize = new Size(753, 473);
			Controls.Add(boardTable);
			Controls.Add(lstShips);
			Controls.Add(lblStatus);
			Controls.Add(btnReady);
			Controls.Add(btnRotate);
			Controls.Add(btnLock);
			Controls.Add(btnRules);
			Controls.Add(btnOk);
			Controls.Add(btnCancel);
			MinimizeBox = false;
			MinimumSize = new Size(600, 400);
			Name = "FormationForm";
			ShowIcon = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "formation";
			ResumeLayout(false);
		}
		#endregion

		private TableLayoutPanel boardTable;
		private ListBox lstShips;
		private Label lblStatus;
		private Button btnReady;
		private Button btnRotate;
		private Button btnLock;
		private Button btnRules;
		private Button btnOk;
		private Button btnCancel;
	}
}

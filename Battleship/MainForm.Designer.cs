namespace Battleship
{
	partial class MainForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			splitContainer1 = new SplitContainer();
			pegSource1 = new PegSource();
			btnReady = new Button();
			lblStatus = new Label();
			clbShips = new CheckedListBox();
			btnNewGame = new Button();
			btnForfeit = new Button();
			btnConfig = new Button();
			boardTable = new TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			SuspendLayout();
			// 
			// splitContainer1
			// 
			splitContainer1.Dock = DockStyle.Fill;
			splitContainer1.FixedPanel = FixedPanel.Panel2;
			splitContainer1.Location = new Point(0, 0);
			splitContainer1.Margin = new Padding(3, 2, 3, 2);
			splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.Controls.Add(pegSource1);
			splitContainer1.Panel1.Controls.Add(btnReady);
			splitContainer1.Panel1.Controls.Add(lblStatus);
			splitContainer1.Panel1.Controls.Add(clbShips);
			splitContainer1.Panel1.Controls.Add(btnNewGame);
			splitContainer1.Panel1.Controls.Add(btnForfeit);
			splitContainer1.Panel1.Controls.Add(btnConfig);
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(boardTable);
			splitContainer1.Panel2MinSize = 400;
			splitContainer1.Size = new Size(686, 372);
			splitContainer1.SplitterDistance = 282;
			splitContainer1.TabIndex = 0;
			// 
			// pegSource1
			// 
			pegSource1.BackColor = Color.Transparent;
			pegSource1.BackgroundImage = (Image)resources.GetObject("pegSource1.BackgroundImage");
			pegSource1.BackgroundImageLayout = ImageLayout.Zoom;
			pegSource1.Location = new Point(12, 151);
			pegSource1.Name = "pegSource1";
			pegSource1.Size = new Size(32, 32);
			pegSource1.TabIndex = 7;
			// 
			// btnReady
			// 
			btnReady.Location = new Point(10, 89);
			btnReady.Name = "btnReady";
			btnReady.Size = new Size(82, 23);
			btnReady.TabIndex = 6;
			btnReady.Text = "ready";
			btnReady.UseVisualStyleBackColor = true;
			// 
			// lblStatus
			// 
			lblStatus.AutoSize = true;
			lblStatus.Location = new Point(98, 9);
			lblStatus.Name = "lblStatus";
			lblStatus.Size = new Size(131, 45);
			lblStatus.TabIndex = 4;
			lblStatus.Text = "PLAYER 1 TURN\r\nPLAYER 1 SHIPS LEFT: {}\r\nPLAYER 2 SHIPS LEFT: {}";
			// 
			// clbShips
			// 
			clbShips.Dock = DockStyle.Bottom;
			clbShips.Enabled = false;
			clbShips.FormattingEnabled = true;
			clbShips.HorizontalScrollbar = true;
			clbShips.Location = new Point(0, 188);
			clbShips.Margin = new Padding(3, 2, 3, 2);
			clbShips.Name = "clbShips";
			clbShips.Size = new Size(282, 184);
			clbShips.Sorted = true;
			clbShips.TabIndex = 2;
			clbShips.MouseDown += clbShips_MouseDown;
			// 
			// btnNewGame
			// 
			btnNewGame.Location = new Point(10, 35);
			btnNewGame.Margin = new Padding(3, 2, 3, 2);
			btnNewGame.Name = "btnNewGame";
			btnNewGame.Size = new Size(82, 22);
			btnNewGame.TabIndex = 1;
			btnNewGame.Text = "new game";
			btnNewGame.UseVisualStyleBackColor = true;
			// 
			// btnForfeit
			// 
			btnForfeit.Location = new Point(10, 62);
			btnForfeit.Margin = new Padding(3, 2, 3, 2);
			btnForfeit.Name = "btnForfeit";
			btnForfeit.Size = new Size(82, 22);
			btnForfeit.TabIndex = 1;
			btnForfeit.Text = "forfeit";
			btnForfeit.UseVisualStyleBackColor = true;
			// 
			// btnConfig
			// 
			btnConfig.Location = new Point(10, 9);
			btnConfig.Margin = new Padding(3, 2, 3, 2);
			btnConfig.Name = "btnConfig";
			btnConfig.Size = new Size(82, 22);
			btnConfig.TabIndex = 0;
			btnConfig.Text = "configure";
			btnConfig.UseVisualStyleBackColor = true;
			btnConfig.Click += btnConfig_Click;
			// 
			// boardTable
			// 
			boardTable.AllowDrop = true;
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
			boardTable.Dock = DockStyle.Fill;
			boardTable.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
			boardTable.Location = new Point(0, 0);
			boardTable.Margin = new Padding(2);
			boardTable.MinimumSize = new Size(350, 300);
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
			boardTable.Size = new Size(400, 372);
			boardTable.TabIndex = 3;
			boardTable.DragDrop += boardTable_DragDrop;
			boardTable.DragEnter += boardTable_DragEnter;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			AutoSize = true;
			ClientSize = new Size(686, 372);
			Controls.Add(splitContainer1);
			MinimumSize = new Size(702, 385);
			Name = "MainForm";
			ShowIcon = false;
			Text = "BATTLE SHIP";
			Load += MainForm_Load;
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel1.PerformLayout();
			splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
			splitContainer1.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private SplitContainer splitContainer1;
		private TableLayoutPanel boardTable;
		private Button btnConfig;
		private Button btnForfeit;
		private CheckedListBox clbShips;
		private Button btnNewGame;
		private Label lblStatus;
		private Button btnReady;
		private PegSource pegSource1;
	}
}

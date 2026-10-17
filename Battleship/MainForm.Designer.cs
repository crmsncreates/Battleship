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
			statusPanel = new Panel();
			lblStatus = new Label();
			clbShips = new CheckedListBox();
			btnNewGame = new Button();
			btnForfeit = new Button();
			btnConfig = new Button();
			cmbView = new ComboBox();
			cmbOpponent = new ComboBox();
			btnFire = new Button();
			btnClear = new Button();
			btnEndTurn = new Button();
			boardTable = new TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			statusPanel.SuspendLayout();
			SuspendLayout();
			// 
			// splitContainer1
			// 
			splitContainer1.Dock = DockStyle.Fill;
			splitContainer1.FixedPanel = FixedPanel.Panel1;
			splitContainer1.Location = new Point(0, 0);
			splitContainer1.Margin = new Padding(3, 2, 3, 2);
			splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.Controls.Add(pegSource1);
			splitContainer1.Panel1.Controls.Add(btnReady);
			splitContainer1.Panel1.Controls.Add(statusPanel);
			splitContainer1.Panel1.Controls.Add(clbShips);
			splitContainer1.Panel1.Controls.Add(btnNewGame);
			splitContainer1.Panel1.Controls.Add(btnForfeit);
			splitContainer1.Panel1.Controls.Add(btnConfig);
			splitContainer1.Panel1.Controls.Add(cmbView);
			splitContainer1.Panel1.Controls.Add(cmbOpponent);
			splitContainer1.Panel1.Controls.Add(btnFire);
			splitContainer1.Panel1.Controls.Add(btnClear);
			splitContainer1.Panel1.Controls.Add(btnEndTurn);
			splitContainer1.Panel1MinSize = 280;
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(boardTable);
			splitContainer1.Panel2MinSize = 400;
			splitContainer1.Size = new Size(886, 500);
			splitContainer1.SplitterDistance = 282;
			splitContainer1.TabIndex = 0;
			// 
			// pegSource1
			// 
			pegSource1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			pegSource1.BackColor = Color.Transparent;
			pegSource1.BackgroundImage = (Image)resources.GetObject("pegSource1.BackgroundImage");
			pegSource1.BackgroundImageLayout = ImageLayout.Zoom;
			pegSource1.Location = new Point(12, 321);
			pegSource1.Name = "pegSource1";
			pegSource1.Size = new Size(32, 32);
			pegSource1.TabIndex = 7;
			// 
			// btnReady
			// 
			btnReady.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			btnReady.Location = new Point(10, 90);
			btnReady.Name = "btnReady";
			btnReady.Size = new Size(75, 23);
			btnReady.TabIndex = 6;
			btnReady.Text = "ready";
			btnReady.UseVisualStyleBackColor = true;
			btnReady.Click += btnReady_Click;
			// 
			// statusPanel
			// 
			statusPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			statusPanel.AutoScroll = true;
			statusPanel.Controls.Add(lblStatus);
			statusPanel.Location = new Point(91, 9);
			statusPanel.Name = "statusPanel";
			statusPanel.Size = new Size(183, 310);
			statusPanel.TabIndex = 13;
			statusPanel.ClientSizeChanged += statusPanel_ClientSizeChanged;
			// 
			// lblStatus
			// 
			lblStatus.AutoSize = true;
			lblStatus.Location = new Point(0, 0);
			lblStatus.MaximumSize = new Size(166, 0);
			lblStatus.Name = "lblStatus";
			lblStatus.Size = new Size(112, 45);
			lblStatus.TabIndex = 4;
			lblStatus.Text = "player 1 turn\r\nplayer 1 ships left: {}\r\nplayer 2 ships left: {}";
			// 
			// clbShips
			// 
			clbShips.Dock = DockStyle.Bottom;
			clbShips.Enabled = false;
			clbShips.FormattingEnabled = true;
			clbShips.HorizontalScrollbar = true;
			clbShips.Location = new Point(0, 388);
			clbShips.Margin = new Padding(3, 2, 3, 2);
			clbShips.Name = "clbShips";
			clbShips.Size = new Size(282, 112);
			clbShips.TabIndex = 2;
			// 
			// btnNewGame
			// 
			btnNewGame.Location = new Point(10, 35);
			btnNewGame.Margin = new Padding(3, 2, 3, 2);
			btnNewGame.Name = "btnNewGame";
			btnNewGame.Size = new Size(75, 23);
			btnNewGame.TabIndex = 1;
			btnNewGame.Text = "new game";
			btnNewGame.UseVisualStyleBackColor = true;
			btnNewGame.Click += btnNewGame_Click;
			// 
			// btnForfeit
			// 
			btnForfeit.Location = new Point(10, 62);
			btnForfeit.Margin = new Padding(3, 2, 3, 2);
			btnForfeit.Name = "btnForfeit";
			btnForfeit.Size = new Size(75, 23);
			btnForfeit.TabIndex = 1;
			btnForfeit.Text = "forfeit";
			btnForfeit.UseVisualStyleBackColor = true;
			btnForfeit.Click += btnForfeit_Click;
			// 
			// btnConfig
			// 
			btnConfig.Location = new Point(10, 9);
			btnConfig.Margin = new Padding(3, 2, 3, 2);
			btnConfig.Name = "btnConfig";
			btnConfig.Size = new Size(75, 23);
			btnConfig.TabIndex = 0;
			btnConfig.Text = "configure";
			btnConfig.UseVisualStyleBackColor = true;
			btnConfig.Click += btnConfig_Click;
			// 
			// cmbView
			// 
			cmbView.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			cmbView.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbView.Items.AddRange(new object[] { "target", "own fleet" });
			cmbView.Location = new Point(50, 325);
			cmbView.Name = "cmbView";
			cmbView.Size = new Size(224, 23);
			cmbView.TabIndex = 8;
			cmbView.SelectedIndexChanged += cmbView_SelectedIndexChanged;
			// 
			// cmbOpponent
			// 
			cmbOpponent.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			cmbOpponent.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbOpponent.Location = new Point(12, 359);
			cmbOpponent.MaxDropDownItems = 12;
			cmbOpponent.Name = "cmbOpponent";
			cmbOpponent.Size = new Size(262, 23);
			cmbOpponent.TabIndex = 9;
			cmbOpponent.SelectedIndexChanged += cmbOpponent_SelectedIndexChanged;
			// 
			// btnFire
			// 
			btnFire.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			btnFire.Location = new Point(10, 119);
			btnFire.Name = "btnFire";
			btnFire.Size = new Size(75, 23);
			btnFire.TabIndex = 10;
			btnFire.Text = "fire";
			btnFire.Click += btnFire_Click;
			// 
			// btnClear
			// 
			btnClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			btnClear.Location = new Point(10, 148);
			btnClear.Name = "btnClear";
			btnClear.Size = new Size(75, 23);
			btnClear.TabIndex = 11;
			btnClear.Text = "clear pegs";
			btnClear.Click += btnClear_Click;
			// 
			// btnEndTurn
			// 
			btnEndTurn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			btnEndTurn.Location = new Point(10, 177);
			btnEndTurn.Name = "btnEndTurn";
			btnEndTurn.Size = new Size(75, 23);
			btnEndTurn.TabIndex = 12;
			btnEndTurn.Text = "end turn";
			btnEndTurn.Click += btnEndTurn_Click;
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
			boardTable.Size = new Size(600, 500);
			boardTable.TabIndex = 3;
			boardTable.DragDrop += boardTable_DragDrop;
			boardTable.DragEnter += boardTable_DragEnter;
			boardTable.DragOver += boardTable_DragEnter;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(886, 500);
			Controls.Add(splitContainer1);
			MinimumSize = new Size(902, 539);
			Name = "MainForm";
			ShowIcon = false;
			Text = "battle ship";
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
			splitContainer1.ResumeLayout(false);
			statusPanel.ResumeLayout(false);
			statusPanel.PerformLayout();
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
		private ComboBox cmbView;
		private ComboBox cmbOpponent;
		private Button btnFire;
		private Button btnClear;
		private Button btnEndTurn;
		private Panel statusPanel;
	}
}

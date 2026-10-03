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
			splitContainer1 = new SplitContainer();
			boardTable = new TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			SuspendLayout();
			// 
			// splitContainer1
			// 
			splitContainer1.Dock = DockStyle.Fill;
			splitContainer1.FixedPanel = FixedPanel.Panel2;
			splitContainer1.Location = new Point(0, 0);
			splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(boardTable);
			splitContainer1.Panel2MinSize = 400;
			splitContainer1.Size = new Size(782, 453);
			splitContainer1.SplitterDistance = 326;
			splitContainer1.TabIndex = 0;
			// 
			// boardTable
			// 
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
			boardTable.MinimumSize = new Size(400, 400);
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
			boardTable.Size = new Size(452, 453);
			boardTable.TabIndex = 3;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			AutoSize = true;
			ClientSize = new Size(782, 453);
			Controls.Add(splitContainer1);
			Margin = new Padding(3, 4, 3, 4);
			MinimumSize = new Size(800, 500);
			Name = "MainForm";
			ShowIcon = false;
			Text = "BATTLE SHIP";
			Load += MainForm_Load;
			splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
			splitContainer1.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private SplitContainer splitContainer1;
		private TableLayoutPanel boardTable;
	}
}

using System.Diagnostics;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;

namespace Battleship
{
	public partial class MainForm : Form
	{
		// NULL unless a game is currently running.
		// everything but the config button is disabled when this is NULL
		Game.Game? game = null;
		Game.GameConfig? cfg = null;

		public MainForm()
		{
			InitializeComponent();
			lblStatus.Text = "Press CONFIGURE to configure a game";
		}

		private void MainForm_Load(object sender, EventArgs e)
		{

		}

		private void btnConfig_Click(object sender, EventArgs e)
		{
			using ConfigForm modalForm = new();
			if (modalForm.ShowDialog(this) is not DialogResult.OK) return;

			var cfg = modalForm.BuildConfig();
			game = new(cfg);
			game.Shrouding += Enshroud;
			game.Unshrouding += Unshroud;
			game.BeginTurn(0);
		}

		// clear board contents until ready button is hit
		void Enshroud()
		{
		}

		// populate board contents
		void Unshroud()
		{
		}

		void ReadyUp()
		{
			game.Shrouded = false;
		}

		private void clbShips_MouseDown(object sender, MouseEventArgs e)
		{
			if (game is not null || clbShips.Items.Count is 0) return;
			if (clbShips.IndexFromPoint(e.X, e.Y) is >= 0 and var index)
			{
				var item = clbShips.Items[index];
				clbShips.DoDragDrop(item, DragDropEffects.Copy | DragDropEffects.Move);
			}
		}

		private void boardTable_DragEnter(object sender, DragEventArgs e)
		{

		}
		private void boardTable_DragDrop(object sender, DragEventArgs e)
		{
			var dropped = e.Data;
			if (dropped is null) return;

			var clientPoint = boardTable.PointToClient(new Point(e.X, e.Y));
			var targetColumn = GetColumnIndex(boardTable, clientPoint.X);
			var targetRow = GetRowIndex(boardTable, clientPoint.Y);

			switch (dropped.GetData(DataFormats.Text)?.ToString())
			{
				case "[Peg]":
					boardTable.Controls.Add(new PlacedPeg(), targetColumn, targetRow);
					break;
				case string droppedText:
					// drop a ship
					break;
			}
		}

		// Helper method to find column index based on mouse X coordinate
		private int GetColumnIndex(TableLayoutPanel panel, int x)
		{
			int cumulativeWidth = 0;
			int[] widths = panel.GetColumnWidths();
			for (int i = 0; i < widths.Length; i++)
			{
				cumulativeWidth += widths[i];
				if (x < cumulativeWidth) return i;
			}
			return widths.Length - 1; // Fallback to last column
		}

		// Helper method to find row index based on mouse Y coordinate
		private int GetRowIndex(TableLayoutPanel panel, int y)
		{
			int cumulativeHeight = 0;
			int[] heights = panel.GetRowHeights();
			for (int i = 0; i < heights.Length; i++)
			{
				cumulativeHeight += heights[i];
				if (y < cumulativeHeight) return i;
			}
			return heights.Length - 1; // Fallback to last row
		}
	}
}

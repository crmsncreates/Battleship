using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace Battleship
{
	public partial class MainForm : Form
	{
		Game.Game? game = null;

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{

		}

		private void btnConfig_Click(object sender, EventArgs e)
		{
			using ConfigForm modalForm = new();
			if (modalForm.ShowDialog(this) is not DialogResult.OK) return;

			var cfg = modalForm.BuildConfig();
			MessageBox.Show($"cfg: {cfg}");
		}

		private void clbShips_Click(object sender, EventArgs e)
		{

		}

		private void clbShips_MouseDown(object sender, MouseEventArgs e)
		{
			if (clbShips.Items.Count == 0) return;
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
			var droppedText = e.Data!.GetData(DataFormats.Text)!.ToString()!;

			var clientPoint = boardTable.PointToClient(new Point(e.X, e.Y));

			var targetColumn = GetColumnIndex(boardTable, clientPoint.X);
			var targetRow = GetRowIndex(boardTable, clientPoint.Y);

			var lbl = new Label
			{
				Text = droppedText,
				AutoSize = true,
				Anchor = AnchorStyles.None
			};

			boardTable.Controls.Add(lbl, targetColumn, targetRow);
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

using Battleship.Game;

namespace Battleship;

// shared by the main board and the configuration board
internal static class BoardControls
{
	public static bool TryGetPosition(TableLayoutPanel table, Point screenPoint, out Position position)
	{
		position = default;

		var point = table.PointToClient(screenPoint);

		if (!table.ClientRectangle.Contains(point))
		{
			return false;
		}

		var column = GetIndex(table.GetColumnWidths(), point.X);
		var row = GetIndex(table.GetRowHeights(), point.Y);

		if ((row, column) is (< 0, < 0))
		{
			return false;
		}

		position = new(column, row);

		return true;
	}

	static int GetIndex(int[] sizes, int coordinate)
	{
		var offset = 0;

		for (var i = 0; i < sizes.Length; ++i)
		{
			offset += sizes[i];

			if (coordinate < offset)
			{
				return i;
			}
		}

		return -1;
	}

	public static void Clear(TableLayoutPanel table)
	{
		while (table.Controls.Count > 0)
		{
			table.Controls[0].Dispose();
		}
	}

	public static void Draw(TableLayoutPanel table, Board? board, IEnumerable<ShipPlacement> placements,
		IEnumerable<Position> pending, DragEventHandler over, DragEventHandler drop, MouseEventHandler mouse)
	{
		table.SuspendLayout();

		try
		{
			var ships = placements
				.SelectMany(p => Board.GetPositions(p).Select(cell => (cell, p.Orientation)))
				.ToDictionary(item => item.cell, item => item.Orientation);
			var queued = pending.ToHashSet();

			for (var x = 0; x < table.ColumnCount; ++x)
			{
				for (var y = 0; y < table.RowCount; ++y)
				{
					var position = new Position(x, y);
					var panel = table.GetControlFromPosition(x, y);

					if (panel is null)
					{
						panel = new Panel
						{
							Dock = DockStyle.Fill,
							Margin = Padding.Empty,
							Tag = position,
							BackColor = SystemColors.Window,
							AccessibleName = $"{(char)('a' + y)}{x + 1}"
						};

						Connect(panel, over, drop, mouse);
						table.Controls.Add(panel, x, y);
					}

					var state = board?.GetCellAt(x, y) switch
					{
						{ HasPeg: true, HasShip: true } => CellState.Hit,
						{ HasPeg: true } => CellState.Miss,
						_ when queued.Contains(position) => CellState.Pending,
						_ when ships.TryGetValue(position, out var orientation) => orientation switch
						{
							Orientation.Horizontal => CellState.HorizontalShip,
							Orientation.Vertical => CellState.VerticalShip,
							_ => CellState.Empty
						},
						_ => CellState.Empty
					};

					// keep unchanged controls, rather than recreating the whole board for every peg
					var old = panel.Controls.Count > 0 ? panel.Controls[0] : null;
					var oldState = old switch
					{
						PlacedRedPeg => CellState.Hit,
						PlacedPeg { BackColor: var color } when color == Color.LightGoldenrodYellow => CellState.Pending,
						PlacedPeg => CellState.Miss,
						ShipX => CellState.HorizontalShip,
						ShipY => CellState.VerticalShip,
						_ => CellState.Empty
					};

					if (oldState == state)
					{
						continue;
					}

					old?.Dispose();

					Control? content = state switch
					{
						CellState.Hit => new PlacedRedPeg(),
						CellState.Miss => new PlacedPeg(),
						CellState.Pending => new PlacedPeg { BackColor = Color.LightGoldenrodYellow },
						CellState.HorizontalShip => new ShipX(),
						CellState.VerticalShip => new ShipY(),
						_ => null
					};

					if (content is null)
					{
						continue;
					}

					content.Dock = DockStyle.Fill;
					content.Margin = Padding.Empty;
					content.BackgroundImageLayout = ImageLayout.Zoom;
					content.Tag = position;

					Connect(content, over, drop, mouse);
					panel.Controls.Add(content);
				}
			}
		}
		finally
		{
			table.ResumeLayout();
		}
	}

	static void Connect(Control control, DragEventHandler over, DragEventHandler drop, MouseEventHandler mouse)
	{
		control.AllowDrop = true;
		control.DragEnter += over;
		control.DragOver += over;
		control.DragDrop += drop;
		control.MouseDown += mouse;
	}

	enum CellState
	{
		Empty,
		Hit,
		Miss,
		Pending,
		HorizontalShip,
		VerticalShip
	}
}

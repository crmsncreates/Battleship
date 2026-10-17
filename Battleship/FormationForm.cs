namespace Battleship;

using Battleship.Game;

public partial class FormationForm : Form
{
	readonly ShipKind[] _kinds;
	readonly ShipPlacement?[,] _placements;
	readonly int _playerCount;
	readonly bool _missilesPerUnhitTile;
	int _player;
	bool _ready;
	bool _complete;
	Orientation _orientation;

	public FormationForm() : this(new Config()) { }

	public FormationForm(Config rules)
	{
		rules.Validate();
		InitializeComponent();
		_playerCount = rules.RealPlayerCount;
		_missilesPerUnhitTile = rules.MissilesPerUnhitTile;
		_kinds = [.. rules.Ships.Select(ship => new ShipKind { Name = ship.Name.Trim(), Length = ship.Length })];
		_placements = new ShipPlacement?[_playerCount, _kinds.Length];
		UpdateSetup();
	}

	public GameConfig BuildConfig()
	{
		if (!_complete)
		{
			throw new InvalidOperationException("complete every player's formation first.");
		}

		var placements = new ShipPlacement[_playerCount, _kinds.Length];

		for (var p = 0; p < _playerCount; ++p)
		{
			for (var i = 0; i < _kinds.Length; ++i)
			{
				placements[p, i] = _placements[p, i]!;
			}
		}

		var config = new GameConfig
		{
			RealPlayerCount = _playerCount,
			Mode = new CustomGameMode { ShipKinds = [.. _kinds], MissilesPerUnhitTile = _missilesPerUnhitTile },
			Placements = placements
		};

		config.Validate();

		return config;
	}

	void UpdateSetup()
	{
		boardTable.Visible = lstShips.Visible = btnRotate.Visible = btnLock.Visible = _ready && !_complete;
		btnReady.Visible = !_ready && !_complete;
		btnOk.Enabled = _complete;
		AcceptButton = _complete ? btnOk : null;
		lstShips.Items.Clear();

		if (!_ready || _complete)
		{
			BoardControls.Clear(boardTable);
		}

		if (_complete)
		{
			lblStatus.Text = "start game.";

			return;
		}

		if (!_ready)
		{
			lblStatus.Text = $"player {_player + 1} needs a formation. others: look away. ready when ready.";

			return;
		}

		for (var i = 0; i < _kinds.Length; ++i)
		{
			lstShips.Items.Add(new FleetEntry(i, _kinds[i], _placements[_player, i] is not null));
		}

		var fleet = CurrentPlacements().ToArray();

		btnLock.Enabled = fleet.Length == _kinds.Length;
		btnRotate.Text = $"rotation :: {_orientation.ToString().ToLowerInvariant()}";
		lblStatus.Text = $"player {_player + 1}, make your formation.\nset rotation, then drag from ship list to board or from board to board.\nlock formation when you are done.";
		BoardControls.Draw(boardTable, null, fleet, [], boardTable_DragEnter, boardTable_DragDrop, boardCell_MouseDown);
	}

	IEnumerable<ShipPlacement> CurrentPlacements(int except = -1)
	{
		for (var i = 0; i < _kinds.Length; ++i)
		{
			if (i != except && _placements[_player, i] is ShipPlacement placement)
			{
				yield return placement;
			}
		}
	}

	private void btnReady_Click(object? sender, EventArgs e)
	{
		_ready = true;
		_orientation = Orientation.Horizontal;
		UpdateSetup();
	}

	private void btnRotate_Click(object? sender, EventArgs e)
	{
		_orientation = _orientation switch
		{
			Orientation.Horizontal => Orientation.Vertical,
			_ => Orientation.Horizontal,
		};
		btnRotate.Text = $"rotation :: {_orientation.ToString().ToLowerInvariant()}";
	}

	private void btnLock_Click(object? sender, EventArgs e)
	{
		if (!_ready || CurrentPlacements().Count() != _kinds.Length)
		{
			return;
		}

		_ready = false;

		if (++_player == _playerCount)
		{
			_complete = true;
		}

		UpdateSetup();
	}

	private void btnRules_Click(object? sender, EventArgs e)
	{
		if (MessageBox.Show("clear all formations and return to rule setup?", "formation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
		{
			DialogResult = DialogResult.Retry;
		}
	}

	private void btnCancel_Click(object? sender, EventArgs e) => DialogResult = DialogResult.Cancel;

	private void btnOk_Click(object? sender, EventArgs e)
	{
		if (!_complete)
		{
			MessageBox.Show(this, "incomplete formation", "incomplete formation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return;
		}

		try
		{
			BuildConfig();
			DialogResult = DialogResult.OK;
		}
		catch (ArgumentException exn)
		{
			MessageBox.Show(exn.Message, "invalid formation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}
	}
	private void lstShips_MouseDown(object? sender, MouseEventArgs e)
	{
		if (e.Button != MouseButtons.Left || !_ready)
		{
			return;
		}

		var index = lstShips.IndexFromPoint(e.Location);

		if (index >= 0 && lstShips.Items[index] is FleetEntry entry)
		{
			lstShips.DoDragDrop($"[Ship:{entry.Index}]", DragDropEffects.Move);
		}
	}

	private void boardCell_MouseDown(object? sender, MouseEventArgs e)
	{
		if (e.Button != MouseButtons.Left || !_ready || sender is not Control { Tag: Position position })
		{
			return;
		}

		for (var i = 0; i < _kinds.Length; ++i)
		{
			if (_placements[_player, i] is not ShipPlacement ship || !Board.GetPositions(ship).Contains(position))
			{
				continue;
			}

			((Control)sender).DoDragDrop($"[Ship:{i}]", DragDropEffects.Move);

			return;
		}
	}

	bool TryGetDrop(DragEventArgs e, out int index, out ShipPlacement? placement)
	{
		index = -1;
		placement = null;

		if (!_ready || _complete || e.Data is not ITypedDataObject data)
		{
			return false;
		}

		if (!data.TryGetData(DataFormats.UnicodeText, out string? text)
			|| text.Length < 8 || !text.StartsWith("[Ship:") || !text.EndsWith(']'))
		{
			return false;
		}

		if (!int.TryParse(text[6..^1], out index) || index < 0 || index >= _kinds.Length)
		{
			return false;
		}

		if (!BoardControls.TryGetPosition(boardTable, new(e.X, e.Y), out var position))
		{
			return false;
		}

		placement = new()
		{
			Kind = _kinds[index],
			X = position.X,
			Y = position.Y,
			Orientation = _orientation
		};

		return Board.CanPlace(10, 10, placement, CurrentPlacements(index));
	}

	private void boardTable_DragEnter(object? sender, DragEventArgs e)
	{
		e.Effect = TryGetDrop(e, out _, out _) ? DragDropEffects.Move : DragDropEffects.None;
	}

	private void boardTable_DragDrop(object? sender, DragEventArgs e)
	{
		if (!TryGetDrop(e, out var index, out var placement))
		{
			MessageBox.Show("invalid placement", "invalid placement", MessageBoxButtons.OK, MessageBoxIcon.Warning);

			return;
		}

		_placements[_player, index] = placement;
		UpdateSetup();
	}

	sealed record FleetEntry(int Index, ShipKind Kind, bool Placed)
	{
		public override string ToString() => $"{Kind.Name} ({Kind.Length}){(Placed ? " - placed" : "")}";
	}
}

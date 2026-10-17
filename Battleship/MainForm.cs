namespace Battleship
{
	using Battleship.Game;

	public partial class MainForm : Form
	{
		// NULL unless a game is currently running
		Game.Game? game = null;

		GameConfig? cfg = null;

		readonly List<Position> _pending = [];

		int _target = -1;

		bool _updating;

		string _resultText = "";

		public MainForm()
		{
			InitializeComponent();
			UpdateStatusWidth();
			cmbView.SelectedIndex = 0;
			clbShips.ItemCheck += (_, e) =>
			{
				if (!_updating)
				{
					e.NewValue = e.CurrentValue;
				}
			};
			UpdateBoard();
		}

		private void btnConfig_Click(object sender, EventArgs e) => ConfigureGame();

		private void btnNewGame_Click(object sender, EventArgs e) => ConfigureGame();

		void ConfigureGame()
		{
			// no old private board should show behind another player's formation
			boardTable.Visible = false;
			clbShips.Visible = false;

			try
			{
				var rules = new Config(cfg);

				while (true)
				{
					using ConfigForm rulesForm = new(rules);

					if (rulesForm.ShowDialog(this) != DialogResult.OK)
					{
						return;
					}

					rules = rulesForm.BuildRules();

					using FormationForm formationForm = new(rules);
					var result = formationForm.ShowDialog(this);

					if (result == DialogResult.Retry)
					{
						continue;
					}

					if (result != DialogResult.OK)
					{
						return;
					}

					if (game is { IsOver: false } && MessageBox.Show(this, "replace the unfinished game?", "new game",
						MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
					{
						return;
					}

					cfg = formationForm.BuildConfig();

					if (game is not null)
					{
						game.Shrouding -= Enshroud;
						game.Unshrouding -= Unshroud;
						game.GameOver -= ShowGameOver;
					}

					game = new(cfg);
					game.Shrouding += Enshroud;
					game.Unshrouding += Unshroud;
					game.GameOver += ShowGameOver;
					game.BeginTurn(0);

					return;
				}
			}
			finally
			{
				UpdateBoard();
			}
		}

		void Enshroud()
		{
			_pending.Clear();
			_resultText = "";
			_target = -1;
			_updating = true;
			cmbOpponent.Items.Clear();
			cmbView.SelectedIndex = 0;
			_updating = false;
			UpdateBoard();
		}

		void Unshroud()
		{
			if (game is null)
			{
				return;
			}

			_updating = true;
			cmbOpponent.Items.Clear();

			for (var i = 1; i < game.PlayerCount; ++i)
			{
				var opponent = (game.CurrentPlayerIndex + i) % game.PlayerCount;

				if (!game.Players[opponent].IsEliminated)
				{
					cmbOpponent.Items.Add(new Opponent(opponent));
				}
			}

			cmbOpponent.SelectedIndex = 0;
			_target = ((Opponent)cmbOpponent.SelectedItem!).Index;
			_updating = false;
			UpdateBoard();
		}

		void ShowGameOver() => UpdateBoard();

		void ReadyUp()
		{
			if (game is not null)
			{
				game.Shrouded = false;
			}
		}

		private void btnReady_Click(object sender, EventArgs e) => ReadyUp();

		void UpdateBoard()
		{
			var playing = game is { Shrouded: false, IsOver: false };
			var ownFleet = cmbView.SelectedIndex == 1;

			btnReady.Enabled = game is { Shrouded: true, IsOver: false };
			btnForfeit.Enabled = playing;
			cmbView.Enabled = playing;
			cmbOpponent.Enabled = playing && !ownFleet && _pending.Count == 0 && game is { HasFired: false };
			pegSource1.Enabled = playing && !ownFleet && game is { HasFired: false } && _pending.Count < game.MissileAllowance;
			btnFire.Enabled = playing && !ownFleet && game is { HasFired: false } && _pending.Count > 0;
			btnClear.Enabled = playing && _pending.Count > 0;
			btnEndTurn.Enabled = playing && _pending.Count == 0;
			boardTable.Visible = true;
			clbShips.Visible = true;
			_updating = true;
			clbShips.Items.Clear();
			_updating = false;

			if (game is null)
			{
				BoardControls.Clear(boardTable);
				SetStatus("press configure to configure a game");

				return;
			}

			if (game.IsOver)
			{
				BoardControls.Clear(boardTable);
				SetStatus($"player {game.WinnerIndex + 1} wins!\nclick new game to play again.");

				return;
			}

			if (game.Shrouded)
			{
				BoardControls.Clear(boardTable);
				SetStatus($"player {game.CurrentPlayerIndex + 1}\npass the pc.\nothers: look away.\nclick ready.");

				return;
			}

			var board = ownFleet ? game.CurrentPlayer.Board : game.Players[_target].Board;

			SetStatus($"player {game.CurrentPlayerIndex + 1} turn\nmissiles left: {(game.HasFired ? 0 : game.MissileAllowance - _pending.Count)}\nqueued: {_pending.Count}\n"
				+ (ownFleet ? "your fleet" : $"target: player {_target + 1}")
				+ $"\nships left: {board.Ships.Count(ship => !ship.IsSunk)}"
				+ (game.HasFired ? "\nvolley fired." : ""));
			_updating = true;

			foreach (var ship in board.Ships)
			{
				clbShips.Items.Add($"{ship.Name} ({ship.Length})", ship.IsSunk);
			}

			_updating = false;
			BoardControls.Draw(boardTable, board, ownFleet ? board.Ships.Select(ship => ship.Placement) : [],
				ownFleet ? [] : _pending, boardTable_DragEnter, boardTable_DragDrop, boardCell_MouseDown);
		}

		void SetStatus(string status)
		{
			var details = game is { Shrouded: false, IsOver: false } && _resultText.Length == 0
				? "drag pegs onto the target board, then click fire. yellow pegs are pending; right-click to remove one. clear pegs unlocks the target. end turn can pass."
				: _resultText;

			lblStatus.Text = details.Length == 0 ? status : $"{status}\n\n{details}";
			statusPanel.AutoScrollPosition = Point.Empty;
		}

		void UpdateStatusWidth()
		{
			var width = statusPanel.ClientSize.Width;

			if (!statusPanel.VerticalScroll.Visible)
			{
				width -= SystemInformation.VerticalScrollBarWidth;
			}

			lblStatus.MaximumSize = new Size(Math.Max(1, width), 0);
		}

		private void statusPanel_ClientSizeChanged(object? sender, EventArgs e) => UpdateStatusWidth();

		private void cmbOpponent_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_updating)
			{
				return;
			}

			_target = cmbOpponent.SelectedItem is Opponent opponent ? opponent.Index : -1;
			UpdateBoard();
		}

		private void cmbView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_updating)
			{
				UpdateBoard();
			}
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			_pending.Clear();
			UpdateBoard();
		}

		private void btnFire_Click(object sender, EventArgs e)
		{
			if (game is null || cmbView.SelectedIndex != 0)
			{
				return;
			}

			var target = _target;
			var result = game.FireVolley(target, _pending);

			if (!result.Accepted)
			{
				MessageBox.Show(this, result.Error, "invalid volley", MessageBoxButtons.OK, MessageBoxIcon.Warning);

				return;
			}

			_pending.Clear();

			var lines = result.Shots.Select(shot => $"{(char)('a' + shot.Position.Y)}{shot.Position.X + 1}: {(shot.Hit ? "hit" : "miss")}").ToList();

			lines.AddRange(result.SunkShips.Select(name => $"sunk: {name}"));

			if (result.EliminatedPlayer is not null)
			{
				lines.Add($"player {target + 1} eliminated.");
			}

			if (result.Winner is not null)
			{
				lines.Add($"player {result.Winner + 1} wins!");
			}
			else
			{
				lines.Add("click end turn to pass the pc.");
			}

			_resultText = string.Join(Environment.NewLine, lines);
			UpdateBoard();
		}

		private void btnEndTurn_Click(object sender, EventArgs e)
		{
			if (_pending.Count == 0)
			{
				game?.NextTurn();
			}
		}

		private void btnForfeit_Click(object sender, EventArgs e)
		{
			if (game is not { Shrouded: false, IsOver: false })
			{
				return;
			}

			if (MessageBox.Show(this, $"player {game.CurrentPlayerIndex + 1}: forfeit?", "forfeit",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				game.Forfeit();
			}
		}

		bool CanQueue(DragEventArgs e, out Position position)
		{
			position = default;

			if (game is not { Shrouded: false, IsOver: false, HasFired: false }
				|| cmbView is not { SelectedIndex: 0 })
			{
				return false;
			}

			if (_target < 0 || game.Players[_target] is { IsEliminated: true }
				|| _pending.Count >= game.MissileAllowance)
			{
				return false;
			}

			if (e.Data is not ITypedDataObject data
				|| !data.TryGetData(DataFormats.UnicodeText, out string? text) || text is not "[Peg]")
			{
				return false;
			}

			return BoardControls.TryGetPosition(boardTable, new(e.X, e.Y), out position)
				&& !_pending.Contains(position) && !game.Players[_target].Board.HasPegAt(position.X, position.Y);
		}

		private void boardTable_DragEnter(object? sender, DragEventArgs e)
		{
			e.Effect = CanQueue(e, out _) ? DragDropEffects.Copy : DragDropEffects.None;
		}

		private void boardTable_DragDrop(object? sender, DragEventArgs e)
		{
			if (!CanQueue(e, out var position))
			{
				return;
			}

			_pending.Add(position);
			UpdateBoard();
		}

		private void boardCell_MouseDown(object? sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right || cmbView.SelectedIndex != 0 || game is not { HasFired: false }
				|| sender is not Control { Tag: Position position })
			{
				return;
			}

			if (_pending.Remove(position))
			{
				UpdateBoard();
			}
		}

		sealed record Opponent(int Index)
		{
			public override string ToString() => $"player {Index + 1}";
		}
	}
}

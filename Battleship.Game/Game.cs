using System.Diagnostics.CodeAnalysis;

namespace Battleship.Game;

// forms build a config, create a game, then display the game's state
public class Game
{
	public event Action? GameOver;

	public event Action? Shrouding;

	public event Action? Unshrouding;

	public GameConfig Config => _config;

	public int PlayerCount => _config.RealPlayerCount;

	public IGameMode Mode => _config.Mode;

	public int BoardWidth => Mode.BoardWidth;

	public int BoardHeight => Mode.BoardHeight;

	public IReadOnlyCollection<ShipKind> ShipKinds => Mode.ShipKinds;

	public IReadOnlyList<Player> Players => Array.AsReadOnly(_players);

	public Player CurrentPlayer => _players[_currentPlayer];

	public int CurrentPlayerIndex => _currentPlayer;

	public Player NextPlayer => _players[NextPlayerIndex];

	public int NextPlayerIndex => Enumerable.Range(1, PlayerCount)
		.Select(offset => (_currentPlayer + offset) % PlayerCount)
		.First(index => !_players[index].IsEliminated);

	public int MissileAllowance { get; private set; }

	public bool HasFired { get; private set; }

	public int? WinnerIndex { get; private set; }

	public bool IsOver => WinnerIndex is not null;

	// hide private contents until the next player clicks ready
	public bool Shrouded
	{
		get => _shrouded;
		set
		{
			if (_shrouded == value || IsOver)
			{
				return;
			}

			_shrouded = value;

			if (value)
			{
				Shrouding?.Invoke();
			}
			else
			{
				Unshrouding?.Invoke();
			}
		}
	}

	readonly GameConfig _config;

	readonly Player[] _players;

	int _currentPlayer;

	bool _shrouded = true;

	public Game(GameConfig config)
	{
		config.Validate();
		_config = config;
		_players = new Player[PlayerCount];

		for (var p = 0; p < PlayerCount; ++p)
		{
			_players[p] = new(new(BoardWidth, BoardHeight,
				Enumerable.Range(0, ShipKinds.Count).Select(i => config.Placements[p, i]),
				allowPegRemoval: false));
		}

		MissileAllowance = GetMissileAllowance();
	}

	int GetMissileAllowance() => !Mode.MissilesPerUnhitTile ? 1 : CurrentPlayer.Board.Ships.Sum(ship => ship.Cells.Count(cell => !cell.HasPeg));

	public void BeginTurn(int player)
	{
		if (IsOver)
		{
			return;
		}

		if (player < 0 || player >= PlayerCount || _players[player].IsEliminated)
		{
			throw new ArgumentOutOfRangeException(nameof(player));
		}

		_shrouded = true;
		_currentPlayer = player;
		HasFired = false;
		MissileAllowance = GetMissileAllowance();
		Shrouding?.Invoke();
	}

	public void NextTurn()
	{
		if (IsOver || Shrouded)
		{
			return;
		}

		BeginTurn(NextPlayerIndex);
	}

	public VolleyResult FireVolley(int opponent, IEnumerable<Position> positions)
	{
		if (this is not { IsOver: false, Shrouded: false, HasFired: false })
		{
			return VolleyResult.Rejected("you cannot fire right now.");
		}

		if (opponent < 0 || opponent >= PlayerCount || opponent == _currentPlayer || _players[opponent].IsEliminated)
		{
			return VolleyResult.Rejected("choose an active opponent.");
		}

		var shots = positions.ToArray();
		var board = _players[opponent].Board;

		if (shots.Length == 0 || shots.Length > MissileAllowance)
		{
			return VolleyResult.Rejected($"choose between 1 and {MissileAllowance} shots.");
		}

		if (shots.Distinct().Count() != shots.Length || shots.Any(p => !board.Contains(p.X, p.Y) || board.HasPegAt(p.X, p.Y)))
		{
			return VolleyResult.Rejected("every shot must be inside the board and on a different, untried cell.");
		}

		var sunkBefore = board.Ships.Where(ship => ship.IsSunk).ToHashSet();

		foreach (var shot in shots)
		{
			board.TryPlacePeg(shot.X, shot.Y);
		}

		var sunk = board.Ships.Where(ship => ship.IsSunk && !sunkBefore.Contains(ship)).ToArray();
		var results = shots.Select(p => new ShotResult
		{
			Position = p,
			Hit = board.HasShipAt(p.X, p.Y)
		}).ToArray();

		HasFired = true;

		if (board.Ships.All(ship => ship.IsSunk))
		{
			_players[opponent].IsEliminated = true;
		}

		CheckWinner();

		return new()
		{
			Accepted = true,
			Shots = Array.AsReadOnly(results),
			SunkShips = Array.AsReadOnly(sunk.Select(ship => ship.Name).ToArray()),
			EliminatedPlayer = _players[opponent].IsEliminated ? opponent : null,
			Winner = WinnerIndex
		};
	}

	public bool Forfeit()
	{
		if (IsOver || Shrouded)
		{
			return false;
		}

		CurrentPlayer.IsEliminated = true;
		CheckWinner();

		if (!IsOver)
		{
			BeginTurn(NextPlayerIndex);
		}

		return true;
	}

	void CheckWinner()
	{
		var remaining = Enumerable.Range(0, PlayerCount).Where(i => !_players[i].IsEliminated).ToArray();

		if (remaining.Length != 1)
		{
			return;
		}

		WinnerIndex = remaining[0];
		_shrouded = true;
		Shrouding?.Invoke();
		GameOver?.Invoke();
	}
}

public class Player(Board board)
{
	public Board Board { get; } = board;

	public bool IsEliminated { get; internal set; }
}

public class Board
{
	public event EventHandler<Board, PositionEventArgs>? PegPlaced;

	public event EventHandler<Board, PositionEventArgs>? PegRemoved;

	public int Width => _cells.GetLength(0);

	public int Height => _cells.GetLength(1);

	public IReadOnlyCollection<Ship> Ships => Array.AsReadOnly(_ships);

	readonly Cell[,] _cells;

	readonly Ship[] _ships;

	readonly bool _allowPegRemoval;

	public Board(int width, int height, IEnumerable<ShipPlacement> placements, bool allowPegRemoval = true)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
		_cells = new Cell[width, height];
		_allowPegRemoval = allowPegRemoval;

		for (var x = 0; x < width; ++x)
		{
			for (var y = 0; y < height; ++y)
			{
				_cells[x, y] = new();
			}
		}

		var placed = new List<Ship>();

		foreach (var placement in placements)
		{
			if (!CanPlace(width, height, placement, placed.Select(ship => ship.Placement)))
			{
				throw new ArgumentException("ships must fit inside the board without overlapping", nameof(placements));
			}

			var cells = GetPositions(placement).Select(p => _cells[p.X, p.Y]).ToArray();

			placed.Add(new()
			{
				Placement = placement,
				Cells = cells
			});
		}

		_ships = [.. placed];
	}

	public static IEnumerable<Position> GetPositions(ShipPlacement placement)
	{
		for (var offset = 0; offset < placement.Kind.Length; ++offset)
		{
			yield return new(placement.X + (placement.Orientation is Orientation.Horizontal ? offset : 0),
				placement.Y + (placement.Orientation is Orientation.Vertical ? offset : 0));
		}
	}

	public static bool CanPlace(int width, int height, ShipPlacement placement, IEnumerable<ShipPlacement> others)
	{
		if (!Enum.IsDefined(placement.Orientation))
		{
			return false;
		}

		var cells = GetPositions(placement).ToArray();

		return cells.All(p => p.X >= 0 && p.X < width && p.Y >= 0 && p.Y < height)
			&& !others.Any(other => GetPositions(other).Intersect(cells).Any());
	}

	public bool Contains(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;

	public Cell GetCellAt(int x, int y) => _cells[x, y];

	public bool HasShipAt(int x, int y) => GetCellAt(x, y).HasShip;

	public bool TryGetShipAt(int x, int y, [MaybeNullWhen(false)] out Ship ship)
	{
		return (ship = GetCellAt(x, y).Ship) is not null;
	}

	public bool HasPegAt(int x, int y) => GetCellAt(x, y).HasPeg;

	public bool TryPlacePeg(int x, int y)
	{
		if (!Contains(x, y) || HasPegAt(x, y))
		{
			return false;
		}

		_cells[x, y].HasPeg = true;
		PegPlaced?.Invoke(this, new() { X = x, Y = y });

		return true;
	}

	public bool TryRemovePeg(int x, int y)
	{
		if (!_allowPegRemoval || !Contains(x, y) || !HasPegAt(x, y))
		{
			return false;
		}

		_cells[x, y].HasPeg = false;
		PegRemoved?.Invoke(this, new() { X = x, Y = y });

		return true;
	}

	public class Cell
	{
		public Ship? Ship { get; set; } = null;

		public bool HasPeg { get; set; } = false;

		public bool HasShip => Ship is not null;
	}
}

public interface IGameMode
{
	public int BoardWidth { get; }

	public int BoardHeight { get; }

	public IReadOnlyCollection<ShipKind> ShipKinds { get; }

	public bool MissilesPerUnhitTile { get; }
}

public class NormalGameMode : IGameMode
{
	public int BoardWidth => 10;

	public int BoardHeight => 10;

	public virtual bool MissilesPerUnhitTile => false;

	public IReadOnlyCollection<ShipKind> ShipKinds => [
		new() { Name = "carrier", Length = 5 },
		new() { Name = "battleship", Length = 4 },
		new() { Name = "cruiser", Length = 3 },
		new() { Name = "submarine", Length = 3 },
		new() { Name = "destroyer", Length = 2 }
	];
}

public class OldTimesGameMode : NormalGameMode
{
	public override bool MissilesPerUnhitTile => true;
}

public record CustomGameMode : IGameMode
{
	public int BoardWidth { get; init; } = 10;

	public int BoardHeight { get; init; } = 10;

	public bool MissilesPerUnhitTile { get; init; }

	public List<ShipKind> ShipKinds { get; init; } = [];

	IReadOnlyCollection<ShipKind> IGameMode.ShipKinds => ShipKinds;
}

public record GameConfig
{
	public required int RealPlayerCount { get; init; }

	public required IGameMode Mode { get; init; }

	public required ShipPlacement[,] Placements { get; init; }

	public void Validate()
	{
		if (RealPlayerCount < 2)
		{
			throw new ArgumentException("at least two players are required");
		}

		if (Mode is not { BoardWidth: 10, BoardHeight: 10 })
		{
			throw new ArgumentException("10x10 board is required, for now");
		}

		var kinds = Mode.ShipKinds.ToArray();

		if (kinds.Length == 0 || kinds.Any(kind => string.IsNullOrWhiteSpace(kind.Name) || kind.Length is < 1 or > 10))
		{
			throw new ArgumentException("must have at least one named ship, with lengths between 1 and 10");
		}

		if (Placements is null || Placements.GetLength(0) != RealPlayerCount || Placements.GetLength(1) != kinds.Length)
		{
			throw new ArgumentException("every player must place every ship");
		}

		for (var p = 0; p < RealPlayerCount; ++p)
		{
			var fleet = new List<ShipPlacement>();

			for (var i = 0; i < kinds.Length; ++i)
			{
				var placement = Placements[p, i];

				if (placement is null || placement.Kind != kinds[i] || !Board.CanPlace(10, 10, placement, fleet))
				{
					throw new ArgumentException($"player {p + 1} has an invalid fleet");
				}

				fleet.Add(placement);
			}
		}
	}
}

public record Ship
{
	public required ShipPlacement Placement { get; init; }

	public required Board.Cell[] Cells
	{
		get;
		init
		{
			ArgumentOutOfRangeException.ThrowIfZero(value.Length);

			foreach (var cell in value)
			{
				cell.Ship = this;
			}

			field = value;
		}
	}

	public int X => Placement.X;

	public int Y => Placement.Y;

	public Orientation Orientation => Placement.Orientation;

	public bool IsHorizontal => Orientation is Orientation.Horizontal;

	public bool IsVertical => Orientation is Orientation.Vertical;

	public ShipKind Kind => Placement.Kind;

	public string Name => Kind.Name;

	public int Length => Kind.Length;

	public bool IsSunk => Cells.All(static cell => cell.HasPeg);

	public bool Overlaps(Ship that) => Maths.BoxOverlapsBox(
		X, Y, IsHorizontal ? Length : 1, IsVertical ? Length : 1,
		that.X, that.Y, that.IsHorizontal ? that.Length : 1, that.IsVertical ? that.Length : 1);

	public bool Overlaps(int x, int y) => Maths.BoxOverlapsPoint(X, Y, IsHorizontal ? Length : 1, IsVertical ? Length : 1, x, y);
}

public record ShipKind
{
	public required string Name { get; init; }

	public required int Length
	{
		get;
		init
		{
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
			field = value;
		}
	}
}

public record ShipPlacement
{
	public required ShipKind Kind { get; init; }

	public required int X
	{
		get;
		init
		{
			ArgumentOutOfRangeException.ThrowIfNegative(value);
			field = value;
		}
	}

	public required int Y
	{
		get;
		init
		{
			ArgumentOutOfRangeException.ThrowIfNegative(value);
			field = value;
		}
	}

	public required Orientation Orientation
	{
		get;
		init
		{
			if (value is not (Orientation.Horizontal or Orientation.Vertical))
			{
				throw new ArgumentOutOfRangeException(nameof(value), "orientation must be horizontal or vertical");
			}

			field = value;
		}
	}
}

public readonly record struct Position(int X, int Y);

public record ShotResult
{
	public required Position Position { get; init; }

	public required bool Hit { get; init; }
}

public record VolleyResult
{
	public bool Accepted { get; init; }

	public string? Error { get; init; }

	public IReadOnlyList<ShotResult> Shots { get; init; } = [];

	public IReadOnlyList<string> SunkShips { get; init; } = [];

	public int? EliminatedPlayer { get; init; }

	public int? Winner { get; init; }

	public static VolleyResult Rejected(string error) => new() { Error = error };
}

public class PositionEventArgs : EventArgs
{
	public required int X { get; init; }
	public required int Y { get; init; }
};

public static class Maths
{
	public static bool BoxOverlapsPoint(int bx, int by, int w, int h, int px, int py)
	{
		return px >= bx && px < bx + w && py >= by && py < by + h;
	}
	public static bool BoxOverlapsBox(int x1, int y1, int w1, int h1, int x2, int y2, int w2, int h2)
	{
		return x1 < x2 + w2 && x1 + w1 > x2 && y1 < y2 + h2 && y1 + h1 > y2;
	}
}

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using Windows.UI.Popups;
using Windows.Web.Http;

namespace Battleship;


// the basic flow will be:
// |> forms build up a game config (mode, player count, ship kinds and locations)
// |> game config is used to create a game
// |> form controls the game, runs until somebody wins
// |> victory screen is shown via form, and that game is thrown to the garbage collector


// a running game
public class Game
{
	public GameConfig Config => _config;
	public int PlayerCount => _config.RealPlayerCount;
	public int BoardWidth => _config.BoardWidth;
	public int BoardHeight => _config.BoardHeight;
	public IReadOnlyCollection<ShipKind> ShipKinds => _config.ShipKinds;
	public IReadOnlyCollection<Player> Players => _players;

	readonly GameConfig _config;
	readonly Player[] _players;

	public Game(GameConfig config)
	{
		_config = config;
		_players = new Player[PlayerCount];
		for (var p = 0; p < PlayerCount; p++)
		{
			_players[p] = new(new Board(
				_config.BoardWidth,
				_config.BoardHeight,
				placements: Enumerable.Range(0, _config.Placements.GetLength(1)).Select(i => _config.Placements[p, i])
			));
		}
	}
}

public class Player
{
	public Board Board { get; }

	public Player(Board board)
	{
		Board = board;
	}

	// TODO! Handle input
}

public class Board
{
	public event EventHandler<Board, PositionEventArgs>? PegPlaced;
	public event EventHandler<Board, PositionEventArgs>? PegRemoved;

	public int Width => _cells.GetLength(0);
	public int Height => _cells.GetLength(1);
	public IReadOnlyCollection<Ship> Ships => _ships;

	readonly Cell[,] _cells;
	readonly Ship[] _ships;

	public Board(int width, int height, IEnumerable<ShipPlacement> placements)
	{
		_cells = new Cell[width, height];
		_ships = [..PlaceShips()];

		IEnumerable<Ship> PlaceShips()
		{
			foreach (var placement in placements)
			{
				var kind = placement.Kind;
				var klen = kind.Length;
				var cells = new Cell[klen];
				switch (placement.Orientation)
				{
					case Orientation.Horizontal:
						for (var xOffset = 0; xOffset < klen; ++xOffset)
						{
							_cells[placement.X + xOffset, placement.Y] = cells[xOffset] = new();
						}
						break;
					case Orientation.Vertical:
						for (var yOffset = 0; yOffset < klen; ++yOffset)
						{
							_cells[placement.X, placement.Y + yOffset] = cells[yOffset] = new();
						}
						break;
				}
				yield return new() { Placement = placement, Cells = cells };
			}
		}
	}

	public Cell GetCellAt(int x, int y) => _cells[x, y];

	public bool HasShipAt(int x, int y) => GetCellAt(x, y).HasShip;
	public bool TryGetShipAt(int x, int y, [MaybeNullWhen(false)] out Ship ship)
	{
		ship = GetCellAt(x, y).Ship;
		return ship is not null;
	}

	public bool HasPegAt(int x, int y) => _cells[x, y].HasPeg;
	public bool TryPlacePeg(int x, int y)
	{
		if (HasPegAt(x, y))
		{
			return false;
		}
		PegPlaced?.Invoke(this, new() { X = x, Y = y });
		_cells[x, y].HasPeg = true;
		return true;
	}
	public bool TryRemovePeg(int x, int y)
	{
		if (!HasPegAt(x, y))
		{
			return false;
		}
		PegRemoved?.Invoke(this, new() { X = x, Y = y });
		_cells[x, y].HasPeg = false;
		return true;
	}

	public record Ship
	{
		public required ShipPlacement Placement { get; init; }
		public required Cell[] Cells
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
		
		public int MinX => X;
		public int MinY => Y;
		public int MaxX => IsHorizontal ? X + Length : X;
		public int MaxY => IsVertical ? Y + Length : Y;

		public ShipKind Kind => Placement.Kind;
		public string Name => Kind.Name;
		public int Length => Kind.Length;

		public bool IsSunk => Cells.All(static x => x.HasPeg);

		public bool Overlaps(Ship that) => MaxX >= that.MinX && MinX <= that.MaxX && MaxY >= that.MinY && MinY <= that.MaxY;
		public bool Overlaps(int x, int y) => x >= MinX && x <= MaxX && y >= MinY && y <= MaxY;
	}
	public class Cell
	{
		public Ship? Ship { get; set; } = null;
		public bool HasPeg { get; set; } = false;

		public bool HasShip => Ship is not null;
	}
}

public abstract class GameMode
{
	public abstract int BoardWidth { get; }
	public abstract int BoardHeight { get; }
	public abstract IEnumerable<int> ShipLengths { get; }
}

public class NormalGameMode : GameMode
{
	public override int BoardWidth => 10;
	public override int BoardHeight => 10;
	public override IEnumerable<int> ShipLengths => [5, 4, 3, 3, 2];
}

public class OldTimesGameMode : GameMode
{
	public override int BoardWidth => 10;
	public override int BoardHeight => 10;
	public override IEnumerable<int> ShipLengths => [5, 4, 3, 3, 2];
}

// TODO! CustomGameMode

public record GameConfig
{
	public required int RealPlayerCount
	{
		get;
		init
		{
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
			field = value;
		}
	}
	// TODO! FakePlayerCount
	public required int BoardWidth
	{
		get;
		init
		{
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
			field = value;
		}
	}
	public required int BoardHeight
	{
		get;
		init
		{
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
			field = value;
		}
	}
	public required ShipKind[] ShipKinds
	{
		get;
		init
		{
			ArgumentOutOfRangeException.ThrowIfZero(value.Length);
			field = value;
		}
	}
	public required ShipPlacement[,] Placements
	{
		get;
		init
		{
			ArgumentOutOfRangeException.ThrowIfNotEqual(value.GetLength(0), RealPlayerCount);
			ArgumentOutOfRangeException.ThrowIfNotEqual(value.GetLength(1), ShipKinds.Length);
			field = value;
		}
	}
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
				throw new ArgumentOutOfRangeException(nameof(value), "Orientation must be Horizontal or Vertical");
			field = value;
		}
	}
}

public class PositionEventArgs : EventArgs
{
	public required int X { get; init; }
	public required int Y { get; init; }
};

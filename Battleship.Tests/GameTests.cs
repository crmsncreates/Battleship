namespace Battleship.Tests;

using Battleship.Game;
using System.Windows.Forms;

public class GameTests
{
	internal static GameConfig MakeConfig(int players = 2, bool tiles = false, params int[] lengths)
	{
		if (lengths.Length == 0)
		{
			lengths = [3, 2];
		}

		var kinds = lengths.Select((length, i) => new ShipKind { Name = $"Ship {i + 1}", Length = length }).ToList();
		var placements = new ShipPlacement[players, kinds.Count];

		for (var p = 0; p < players; ++p)
		{
			for (var i = 0; i < kinds.Count; ++i)
			{
				placements[p, i] = new()
				{
					Kind = kinds[i],
					X = 0,
					Y = i,
					Orientation = Orientation.Horizontal
				};
			}
		}

		return new()
		{
			RealPlayerCount = players,
			Mode = new CustomGameMode { ShipKinds = kinds, MissilesPerUnhitTile = tiles },
			Placements = placements
		};
	}

	static Game Start(int players = 2, bool tiles = false, params int[] lengths)
	{
		var game = new Game(MakeConfig(players, tiles, lengths));

		game.BeginTurn(0);
		game.Shrouded = false;

		return game;
	}

	static void Next(Game game)
	{
		game.NextTurn();
		game.Shrouded = false;
	}

	[Test]
	public void GameConfig_CanInit()
	{
		var config = MakeConfig();

		Assert.DoesNotThrow(config.Validate);
		Assert.That(new Game(config).PlayerCount, Is.EqualTo(2));
	}

	[Test]
	public void WaterCellsExistAndShipCellsAreLinked()
	{
		var game = Start();

		Assert.That(game.CurrentPlayer.Board.GetCellAt(9, 9).HasShip, Is.False);

		var ship = game.CurrentPlayer.Board.Ships.First();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(ship.Cells.All(cell => ReferenceEquals(cell.Ship, ship)), Is.True);
			Assert.That(game.Players[0].Board, Is.Not.SameAs(game.Players[1].Board));
		}
	}

	[Test]
	public void InvalidConfigsAndOverlappingFleetsAreRejected()
	{
		Assert.Throws<ArgumentException>(() => new Game(MakeConfig() with { RealPlayerCount = 1 }));
		Assert.Throws<ArgumentException>(() => new Game(MakeConfig() with { Placements = new ShipPlacement[2, 1] }));

		var config = MakeConfig();

		config.Placements[0, 1] = config.Placements[0, 1] with
		{
			Y = 0
		};
		Assert.Throws<ArgumentException>(() => new Game(config));
		config = MakeConfig();
		config.Placements[0, 0] = config.Placements[0, 0] with
		{
			X = 9
		};
		Assert.Throws<ArgumentException>(() => new Game(config));
		Assert.Throws<ArgumentException>(() => new Game(MakeConfig(2, false, 11)));
	}

	[Test]
	public void PlacementValidationAllowsTouchingAndKeepsEqualShipsDistinct()
	{
		var config = MakeConfig(2, false, 3, 3);

		Assert.DoesNotThrow(config.Validate);

		var placement = config.Placements[0, 0] with
		{
			X = 9,
			Y = 0,
			Orientation = Orientation.Vertical
		};

		using (Assert.EnterMultipleScope())
		{
			Assert.That(Board.CanPlace(10, 10, placement, [config.Placements[0, 1]]), Is.True);
			Assert.That(Board.GetPositions(placement).Last(), Is.EqualTo(new Position(9, 2)));
			Assert.That(Board.CanPlace(10, 10, placement with { Y = 9 }, []), Is.False);
		}
	}

	[Test]
	public void NormalAndOldTimesPresetsDifferOnlyInMissileRule()
	{
		using (Assert.EnterMultipleScope())
		{
			Assert.That(new NormalGameMode().MissilesPerUnhitTile, Is.False);
			Assert.That(new OldTimesGameMode().MissilesPerUnhitTile, Is.True);
			Assert.That(new NormalGameMode().ShipKinds, Is.EqualTo(new OldTimesGameMode().ShipKinds));
		}
	}

	[Test]
	public void NormalRuleGivesOneMissileAndRejectsOversizedVolley()
	{
		var game = Start();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(game.MissileAllowance, Is.EqualTo(1));
			Assert.That(game.FireVolley(1, [new(0, 0), new(1, 0)]).Accepted, Is.False);
			Assert.That(game.Players[1].Board.HasPegAt(0, 0), Is.False);
			Assert.That(game.HasFired, Is.False);
			Assert.That(game.FireVolley(1, [new(0, 0)]).Accepted, Is.True);
			Assert.That(game.HasFired, Is.True);
			Assert.That(game.FireVolley(1, [new(1, 0)]).Accepted, Is.False);
		}
	}

	[Test]
	public void TileRuleCountsUnhitTilesAndAllowsPartialVolleys()
	{
		var game = Start(2, true);

		Assert.That(game.MissileAllowance, Is.EqualTo(5));
		
		game.FireVolley(1, [new(0, 0), new(1, 0)]);
		Next(game);
		
		Assert.That(game.MissileAllowance, Is.EqualTo(3));
		
		game.FireVolley(0, [new(0, 0)]);
		Next(game);
		
		Assert.That(game.MissileAllowance, Is.EqualTo(4));
	}

	[Test]
	public void InvalidVolleysNeverPartiallyMutateTheBoard()
	{
		var game = Start(3, true);
		Position[][] invalid = [[], [new(0, 0), new(0, 0)], [new(0, 0), new(10, 0)], [new(-1, 0)]];

		foreach (var shots in invalid)
		{
			using (Assert.EnterMultipleScope())
			{
				Assert.That(game.FireVolley(1, shots).Accepted, Is.False);
				Assert.That(game.HasFired, Is.False);
				Assert.That(game.Players[1].Board.HasPegAt(0, 0), Is.False);
			}
		}

		using (Assert.EnterMultipleScope())
		{
			Assert.That(game.FireVolley(0, [new(0, 0)]).Accepted, Is.False);
			Assert.That(game.FireVolley(-1, [new(0, 0)]).Accepted, Is.False);
			Assert.That(game.FireVolley(3, [new(0, 0)]).Accepted, Is.False);
		}
	}

	[Test]
	public void SharedHistoryRejectsRepeatsAndCombatPegsArePermanent()
	{
		var game = Start(3, true);

		game.FireVolley(2, [new(0, 0), new(9, 9)]);
		Next(game);
		
		using (Assert.EnterMultipleScope())
		{
			Assert.That(game.FireVolley(2, [new(1, 0), new(0, 0)]).Accepted, Is.False);
			Assert.That(game.Players[2].Board.HasPegAt(1, 0), Is.False);
			Assert.That(game.FireVolley(2, [new(9, 9)]).Accepted, Is.False);
			Assert.That(game.Players[2].Board.TryRemovePeg(0, 0), Is.False);
		}
	}

	[Test]
	public void VolleyResolvesAllShotsAndReportsSunkShipsTogether()
	{
		var game = Start(3, true);
		var result = game.FireVolley(1, [new(0, 1), new(1, 1), new(9, 9)]);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.Shots.Select(shot => shot.Hit), Is.EqualTo(new[] { true, true, false }));
			Assert.That(result.SunkShips, Is.EqualTo(new[] { "Ship 2" }));
			Assert.That(result.EliminatedPlayer, Is.Null);
			Assert.That(game.MissileAllowance, Is.EqualTo(5));
		}
	}

	[TestCase(2), TestCase(3)]
	public void CompleteGameesEliminateSkipAndDeclareWinner(int players)
	{
		var game = Start(players, true);
		var fleet = new Position[] { new(0, 0), new(1, 0), new(2, 0), new(0, 1), new(1, 1) };

		for (var target = 1; target < players; ++target)
		{
			while (game.CurrentPlayerIndex != 0)
			{
				Next(game);
			}

			var result = game.FireVolley(target, fleet);

			using (Assert.EnterMultipleScope())
			{
				Assert.That(result.EliminatedPlayer, Is.EqualTo(target));
				Assert.That(game.Players[target].IsEliminated, Is.True);
			}

			if (!game.IsOver)
			{
				Next(game);
				Assert.That(game.CurrentPlayerIndex, Is.Not.EqualTo(target));
			}
		}

		using (Assert.EnterMultipleScope())
		{
			Assert.That(game.WinnerIndex, Is.EqualTo(0));
			Assert.That(game.Shrouded);
			Assert.That(!game.FireVolley(1, [new(9, 9)]).Accepted);
			Assert.That(!game.Forfeit());
		}
	}

	[Test]
	public void HandoffsBlockFiringAndForfeitCanEndGame()
	{
		var game = Start(3);

		game.NextTurn();
		
		using (Assert.EnterMultipleScope())
		{
			Assert.That(game.Shrouded);
			Assert.That(!game.FireVolley(2, [new(0, 0)]).Accepted);
			Assert.That(!game.Forfeit());
		}

		game.Shrouded = false;
		
		using (Assert.EnterMultipleScope())
		{
			Assert.That(game.Forfeit());
			Assert.That(game.CurrentPlayerIndex, Is.EqualTo(2));
		}
		
		game.Shrouded = false;
		game.Forfeit();

		Assert.That(game.WinnerIndex, Is.EqualTo(0));
	}

	[Test]
	public void PegEventsObserveTheUpdatedCell()
	{
		var board = Start().Players[1].Board;
		bool called = false;

		board.PegPlaced += (sender, e) =>
		{
			called = true;
			Assert.That(sender.HasPegAt(e.X, e.Y));
		};
		board.TryPlacePeg(0, 0);
		Assert.That(called);
	}
}

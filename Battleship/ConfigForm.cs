namespace Battleship;

using Battleship.Game;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

public partial class ConfigForm : Form
{
	readonly Config _config;

	bool _halted;

	public ConfigForm() : this(null) { }

	public ConfigForm(Config? previous)
	{
		InitializeComponent();

		_config = previous?.Clone() ?? new();
		
		propertyGrid1.SelectedObject = _config;
		
		fleetTable.AutoGenerateColumns = false;
		fleetTable.DataSource = _config.Ships;
	}

	public Config BuildRules()
	{
		_config.Validate();
		return _config.Clone();
	}

	private void btnNext_Click(object? sender, EventArgs e)
	{
		if (!ValidateChildren() || !fleetTable.EndEdit())
		{
			return;
		}

		BindingContext?[_config.Ships]?.EndCurrentEdit();

		if (!_config.TryValidate(out var error))
		{
			ShowInputError(error);
			return;
		}

		DialogResult = DialogResult.OK;
	}

	void ShowInputError(string message)
	{
		if (_halted)
		{
			return;
		}

		_halted = true;
		lblStatus.Text = message;

		try
		{
			MessageBox.Show(this, message, "invalid rules", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}
		finally
		{
			_halted = false;
		}
	}

	private void btnCancel_Click(object? sender, EventArgs e) => DialogResult = DialogResult.Cancel;

	private void fleetTable_DataError(object? sender, DataGridViewDataErrorEventArgs e)
	{
		e.ThrowException = false;
		e.Cancel = true;
		ShowInputError("ship length must be in [1..10].");
	}
}

public class Config
{
	public Config(GameConfig? previous = null)
	{
		if (previous is null)
		{
			return;
		}

		RealPlayerCount = previous.RealPlayerCount;
		MissilesPerUnhitTile = previous.Mode.MissilesPerUnhitTile;
		Ships.Clear();

		foreach (var kind in previous.Mode.ShipKinds)
		{
			Ships.Add(new() { Name = kind.Name, Length = kind.Length });
		}
	}

	public Config Clone()
	{
		var copy = new Config { RealPlayerCount = RealPlayerCount, MissilesPerUnhitTile = MissilesPerUnhitTile };

		copy.Ships.Clear();

		foreach (var ship in Ships)
		{
			copy.Ships.Add(new() { Name = ship.Name, Length = ship.Length });
		}

		return copy;
	}

	public bool TryValidate([NotNullWhen(false)] out string? error)
	{
		error = this switch
		{
			{ RealPlayerCount: < 2 } => "at least two players are required.",
			{ Ships: [] } => "add at least one ship before continuing.",
			_ when Ships.Any(ship => string.IsNullOrWhiteSpace(ship.Name)) => "each ship needs a name.",
			_ when Ships.Any(ship => ship.Length is < 1 or > 10) => "ship length must be from 1 to 10.",
			_ when Ships.Sum(ship => ship.Length) > 100 => "your ships use more than 100 tiles. shorten or remove a ship.",
			_ => null
		};

		return error is null;
	}

	public void Validate()
	{
		if (!TryValidate(out var error))
		{
			throw new ArgumentException(error);
		}
	}

	[Category("rules"), DisplayName("players"), Description("two or more people sharing this pc.")]
	public int RealPlayerCount { get; set; } = 2;

	[Category("rules"), DisplayName("missiles/tile"), Description("when false, you get 1 missile per turn.\nwhen true, players get 1 missile for each un-hit tile in your own fleet.")]
	public bool MissilesPerUnhitTile { get; set; }

	[Browsable(false)]
	public BindingList<Metaship> Ships { get; } = [.. new NormalGameMode().ShipKinds.Select(kind => new Metaship { Name = kind.Name, Length = kind.Length })];

	public class Metaship
	{
		public string Name { get; set; } = "ship";

		public int Length { get; set; } = 2;
	}
}

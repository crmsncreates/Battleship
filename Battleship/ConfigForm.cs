namespace Battleship;
using Battleship.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Windows.Forms;

public partial class ConfigForm : Form
{
	Config _config;

	public ConfigForm()
	{
		InitializeComponent();

		propertyGrid1.SelectedObject = _config = new Config();
	}

	public GameConfig BuildConfig()
	{
		List<ShipKind> kinds = [
			new() { Length = _config.Ship1.Length, Name = _config.Ship1.Name },
			new() { Length = _config.Ship2.Length, Name = _config.Ship2.Name },
			new() { Length = _config.Ship3.Length, Name = _config.Ship3.Name },
			new() { Length = _config.Ship4.Length, Name = _config.Ship4.Name },
			new() { Length = _config.Ship5.Length, Name = _config.Ship5.Name }
		];
		List<ShipPlacement> placements = [
			new() { X = _config.Ship1.X, Y = _config.Ship1.Y, Orientation = _config.Ship1.Orientation, Kind = kinds[0] },
			new() { X = _config.Ship2.X, Y = _config.Ship2.Y, Orientation = _config.Ship2.Orientation, Kind = kinds[1] },
			new() { X = _config.Ship3.X, Y = _config.Ship3.Y, Orientation = _config.Ship3.Orientation, Kind = kinds[2] },
			new() { X = _config.Ship4.X, Y = _config.Ship4.Y, Orientation = _config.Ship4.Orientation, Kind = kinds[3] },
			new() { X = _config.Ship5.X, Y = _config.Ship5.Y, Orientation = _config.Ship5.Orientation, Kind = kinds[4] },
		];
		var p2d = new ShipPlacement[2, 5];
		for (int i = 0; i < 5; ++i) p2d[1, i] = p2d[0, i] = placements[i];
		return new()
		{
			Mode = new NormalGameMode(),
			RealPlayerCount = 2,
			Placements = p2d
		};
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		DialogResult = DialogResult.Cancel;
	}

	private void btnOk_Click(object sender, EventArgs e)
	{
		DialogResult = DialogResult.OK;
	}
}

public class Config
{
	public Metaship Ship1 { get; set; } = new() { Name = "1", Length = 2, X = 1 };
	public Metaship Ship2 { get; set; } = new() { Name = "2", Length = 3, X = 2 };
	public Metaship Ship3 { get; set; } = new() { Name = "3", Length = 4, X = 3 };
	public Metaship Ship4 { get; set; } = new() { Name = "4", Length = 4, X = 4 };
	public Metaship Ship5 { get; set; } = new() { Name = "5", Length = 5, X = 5 };

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class Metaship
	{
		public string Name { get; set; } = "";
		public int Length { get; set; } = 1;
		public int X { get; set; } = 0;
		public int Y { get; set; } = 0;
		public Orientation Orientation { get; set; } = Orientation.Vertical;
	}
}

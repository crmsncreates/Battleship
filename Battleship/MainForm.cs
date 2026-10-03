using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace Battleship
{
	// HEAVY wip. config window works, but i'm
	// figuring out how to do ship placement
	// in the gui.
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
	}
}

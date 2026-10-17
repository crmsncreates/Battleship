namespace Battleship
{
	/// <summary>drag pegs from this onto the grid</summary>
	public partial class PegSource : UserControl
	{
		public PegSource()
		{
			InitializeComponent();
		}

		private void PegSource_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				DoDragDrop("[Peg]", DragDropEffects.Copy);
			}
		}
	}
}

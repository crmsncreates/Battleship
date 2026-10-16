using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
			DoDragDrop("[Peg]", DragDropEffects.Copy | DragDropEffects.Move);
		}
	}
}

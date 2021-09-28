using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProgTest
{
	internal partial class Form1 : Form
	{
		class TScreenImpl : CGame.CScreen
		{
			Graphics gr;
			internal TScreenImpl(Graphics g) { gr = g; }
			internal override void DrawLine(Vector a, Vector b)
			{
				gr.DrawLine(Pens.Black, a.X, SizeY - a.Y - 1, b.X, SizeY - b.Y - 1);
			}
		}

		CGame Game = new CGame();
		CGame.CInput Input = new CGame.CInput();
		long LastTicks;

		internal Form1()
		{
			InitializeComponent();
			ClientSize = new Size(640, 480);
			LastTicks = DateTime.Now.Ticks;
			Game.GameCreate();
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			TScreenImpl scr = new TScreenImpl(e.Graphics) { SizeX = ClientSize.Width, SizeY = ClientSize.Height };

			long newTicks = DateTime.Now.Ticks;
			float dt = (newTicks - LastTicks) * 0.0000001f;
			LastTicks = newTicks;
			dt = Math.Max(0.001f, Math.Min(1, dt));	//omezi >=1ms a <=1sec

			Game.SetControls(Input);
			Game.GameTick(scr, dt);

			Input.Fire = false;	//HACK - aby strelba reagovala jen na stisk, ne na drzeni

			Invalidate();
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
				case Keys.Up:
					Input.Up = true;
					break;
				case Keys.Down:
					Input.Down = true;
					break;
				case Keys.Left:
					Input.Left = true;
					break;
				case Keys.Right:
					Input.Right = true;
					break;
				case Keys.Space:
					Input.Fire = true;
					break;
			}
		}

		private void Form1_KeyUp(object sender, KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
				case Keys.Up:
					Input.Up = false;
					break;
				case Keys.Down:
					Input.Down = false;
					break;
				case Keys.Left:
					Input.Left = false;
					break;
				case Keys.Right:
					Input.Right = false;
					break;
				case Keys.Space:
					Input.Fire = false;
					break;
			}
		}
	}
}

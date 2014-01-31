using System;
using Gtk;
using System.IO;
using System.Reflection;

namespace AddAnyDock
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run();
		}	
	}
}

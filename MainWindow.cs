using System;
using Gtk;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using MonoMac;
using System.Collections.Generic;
using AddAnyDock;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		label1.Text = "Click below to " + Environment.NewLine + "add a new Item to the Dock.";
		this.Destroyed += OnDestroyed;
		button1.Clicked += OnButtonClicked;

	}
	protected void OnDestroyed (object sender, EventArgs e)
	{
		Environment.Exit (0);
	}

	protected void OnButtonClicked (object sender, EventArgs e)
	{
		if (sender == button1)
		{

			Gtk.FileChooserDialog dialog = new Gtk.FileChooserDialog ("Choose item...", this, FileChooserAction.Open, "Cancel",  ResponseType.Cancel, "Insert Spacer",  ResponseType.None, "Add", ResponseType.Accept);
					
			Gtk.Alignment align = new Alignment (1, 0, 0, 1);
			Gtk.Frame frame = new Frame ("Position");
			Gtk.HBox hbox = new HBox (false, 4);

			RadioButton rbRight;
			rbRight = new RadioButton ("Right");
			hbox.PackEnd(rbRight, false, false, 1);
			hbox.PackEnd(new RadioButton (rbRight, "Left"), false, false, 1);


			frame.Add (hbox);
			align.Add (frame);
			align.ShowAll ();
			dialog.ExtraWidget = align;

			ResponseType response = (ResponseType)dialog.Run ();
			if (response == ResponseType.Accept) {
				RunCommand ("defaults write com.apple.dock " + GetAlign(dialog.ExtraWidget) + " -array-add '<dict><key>tile-data</key><dict><key>file-data</key><dict><key>_CFURLString</key><string>" + dialog.Filename + "</string><key>_CFURLStringType</key><integer>0</integer></dict></dict></dict>' && /bin/sleep 1 &&/usr/bin/killall Dock");
			} else if (response == ResponseType.None) {
				RunCommand ("defaults write com.apple.dock " + GetAlign(dialog.ExtraWidget) + " -array-add '{tile-data={}; tile-type=\"spacer-tile\";}' && /bin/sleep 1 &&/usr/bin/killall Dock");
			}
			dialog.Destroy ();

		}

	}

	private string GetAlign(Widget frame)
	{

		if (((RadioButton)((HBox)((Frame)((Alignment)frame).Child).Child).Children[0]).Active == true)
			return "persistent-apps";
		else
			return "persistent-others";

	}

	private void RunCommand(string cmd)
	{
		MonoMac.Foundation.NSTask task = new MonoMac.Foundation.NSTask ();
		task.LaunchPath = "/bin/sh";
		task.Arguments = new string[]{ "-c", cmd };
		task.Launch ();
	}


}

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CefSharp;

namespace Uebersicht_Windows {
	class UBApplicationContext : ApplicationContext {
		private NotifyIcon trayIcon;
		private ContextMenuStrip trayIconContextMenu;
		
		private ToolStripMenuItem aboutMenuItem;
		private ToolStripMenuItem updateMenuItem;
		private ToolStripMenuItem widgetFolderMenuItem;
		private ToolStripMenuItem widgetGalleryMenuItem;
		private ToolStripMenuItem debugConsoleMenuItem;
		private ToolStripMenuItem refreshMenuItem;
		private ToolStripMenuItem preferencesMenuItem;
		private ToolStripMenuItem quitMenuItem;

		private UBWindow widgetWindow;

		public UBApplicationContext() {
			Cef.Initialize(new CefSettings());

			Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
			InitializeTrayIcon();
			InitializeWidgetWindow();
		}

		private void InitializeTrayIcon() {
			trayIcon = new NotifyIcon();
			
			trayIcon.Text = "Übersicht";
			trayIcon.Icon = new Icon("..\\..\\..\\status-icon.ico");

			trayIconContextMenu = new ContextMenuStrip();

			trayIconContextMenu.SuspendLayout();

			aboutMenuItem = new ToolStripMenuItem("About Übersicht");
			aboutMenuItem.Click += new EventHandler(this.AboutMenuItem_Click);
			trayIconContextMenu.Items.Add(aboutMenuItem);

			updateMenuItem = new ToolStripMenuItem("Check for Updates...");
			updateMenuItem.Click += new EventHandler(this.UpdateMenuItem_Click);
			trayIconContextMenu.Items.Add(updateMenuItem);

			trayIconContextMenu.Items.Add("-");

			widgetFolderMenuItem = new ToolStripMenuItem("Open Widgets Folder");
			widgetFolderMenuItem.Click += new EventHandler(this.WidgetFolderMenuItem_Click);
			trayIconContextMenu.Items.Add(widgetFolderMenuItem);

			widgetGalleryMenuItem = new ToolStripMenuItem("Visit Widgets Gallery");
			widgetGalleryMenuItem.Click += new EventHandler(this.WidgetGalleryMenuItem_Click);
			trayIconContextMenu.Items.Add(widgetGalleryMenuItem);

			trayIconContextMenu.Items.Add("-");

			debugConsoleMenuItem = new ToolStripMenuItem("Show Debug Console");
			debugConsoleMenuItem.Click += new EventHandler(this.DebugConsoleMenuItem_Click);
			trayIconContextMenu.Items.Add(debugConsoleMenuItem);

			refreshMenuItem = new ToolStripMenuItem("Refresh All Widgets");
			refreshMenuItem.Click += new EventHandler(this.RefreshMenuItem_Click);
			trayIconContextMenu.Items.Add(refreshMenuItem);

			trayIconContextMenu.Items.Add("-");

			preferencesMenuItem = new ToolStripMenuItem("Preferences...");
			preferencesMenuItem.Click += new EventHandler(this.PreferencesMenuItem_Click);
			trayIconContextMenu.Items.Add(preferencesMenuItem);

			trayIconContextMenu.Items.Add("-");

			quitMenuItem = new ToolStripMenuItem("Quit Übersicht");
			quitMenuItem.Click += new EventHandler(this.QuitMenuItem_Click);
			trayIconContextMenu.Items.Add(quitMenuItem);

			trayIconContextMenu.ResumeLayout(false);
			trayIcon.ContextMenuStrip = trayIconContextMenu;

			trayIcon.Visible = true;
		}

		private void InitializeWidgetWindow() {
			widgetWindow = new UBWindow();
			

			
			widgetWindow.Visible = true;
		}

		private void OnApplicationExit(object sender, EventArgs e) {
			trayIcon.Visible = false;
		}

		private void TrayIcon_DoubleClick(object sender, EventArgs e) {
			trayIcon.ShowBalloonTip(10000);
		}

		private void AboutMenuItem_Click(object sender, EventArgs e) {
		}

		private void UpdateMenuItem_Click(object sender, EventArgs e) {
		}

		private void WidgetFolderMenuItem_Click(object sender, EventArgs e) {
		}

		private void WidgetGalleryMenuItem_Click(object sender, EventArgs e) {
			System.Diagnostics.Process.Start("http://tracesof.net/uebersicht-widgets/");
		}

		private void DebugConsoleMenuItem_Click(object sender, EventArgs e) {
			// nothing for now
		}

		private void RefreshMenuItem_Click(object sender, EventArgs e) {
			widgetWindow.Refresh();
		}

		private void PreferencesMenuItem_Click(object sender, EventArgs e) {
			// TODO: Make preference manager
		}

		private void QuitMenuItem_Click(object sender, EventArgs e) {
			Application.Exit();
		}
	}
}

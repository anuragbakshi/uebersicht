using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Win32Wrapper;

using CefSharp.WinForms;

namespace Uebersicht_Windows {
	public class UBWindow : Form {
		private const String WIDGET_SERVER_URL = "http://192.168.1.2:41416";

		private IntPtr workerw = IntPtr.Zero;

		private WebBrowser webBrowser;
		//private ChromiumWebBrowser widgetView;

		private bool isInFront = false;

		public UBWindow() {
			InitializeBackgroundLayer();
			InitializeComponents();
			SendToDesktop();

			//Console.WriteLine(webBrowser.Version);
			LoadUrl(WIDGET_SERVER_URL);
		}

		public void LoadUrl(string url) {
			webBrowser.Navigate(new Uri(url));
			//widgetView.Load(url);
		}

		public void Reload() {
			webBrowser.Refresh();
			//widgetView.Reload(false);
		}

		public void FillScreen(Screen s) {
			// ...
		}

		public void SendToDesktop() {
			isInFront = false;
			W32.SetParent(Handle, workerw);
		}

		public void ComeToFront() {
			isInFront = true;
			// ...
		}

		public bool IsInFront() {
			return isInFront;
		}

		private void InitializeComponents() {
			this.FormBorderStyle = FormBorderStyle.None;
			this.SetBounds(0, 0, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);

			webBrowser = new WebBrowser();
			webBrowser.SetBounds(0, 0, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
			this.Controls.Add(webBrowser);

			//widgetView = new ChromiumWebBrowser(WIDGET_SERVER_URL);
			//widgetView.SetBounds(0, 0, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
			//this.Controls.Add(widgetView);
		}

		private void InitializeBackgroundLayer() {
			IntPtr progman = W32.FindWindow("Progman", null);
			IntPtr result = IntPtr.Zero;

			W32.SendMessageTimeout(progman, 0x052C, new IntPtr(0), IntPtr.Zero, W32.SendMessageTimeoutFlags.SMTO_NORMAL, 1000, out result);

			W32.EnumWindows(new W32.EnumWindowsProc((tophandle, topparamhandle) => {
				IntPtr p = W32.FindWindowEx(tophandle, IntPtr.Zero, "SHELLDLL_DefView", IntPtr.Zero);

				if (p != IntPtr.Zero) {
					workerw = W32.FindWindowEx(IntPtr.Zero, tophandle, "WorkerW", IntPtr.Zero);
				}

				return true;
			}), IntPtr.Zero);
		}
	}
}

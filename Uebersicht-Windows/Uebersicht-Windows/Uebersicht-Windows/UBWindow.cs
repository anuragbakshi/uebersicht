using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Win32;

using Win32Wrapper;

using CefSharp.WinForms;

namespace Uebersicht_Windows {
	public class UBWindow : Form {
		private static readonly string WIDGET_SERVER_URL = "http://192.168.1.2:41416";
		private static readonly string WALLPAPER_IMAGE_B64;
		
		private IntPtr workerw = IntPtr.Zero;

		private ChromiumWebBrowser widgetView;

		private bool isInFront = false;

		static UBWindow() {
			var path = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop").GetValue("WallPaper").ToString();
			var format = path.Substring(path.LastIndexOf(".") + 1);

			if(format == "jpg")
				format = "jpeg";
			else if(format == "tif")
				format = "tiff";

			WALLPAPER_IMAGE_B64 = "data:image/" + format + ";base64," + Convert.ToBase64String(File.ReadAllBytes(path));
		}

		public UBWindow() {
			InitializeBackgroundLayer();
			InitializeComponents();
			
			SendToDesktop();
			FillScreen(Screen.PrimaryScreen);
		}

		public void LoadUrl(string url) {
			widgetView.Load(url);
		}

		public void Reload() {
			widgetView.Reload(false);
		}

		public void FillScreen(Screen s) {
			widgetView.Bounds = s.Bounds;
		}

		public void SendToDesktop() {
			isInFront = false;
			W32.SetParent(this.Handle, workerw);
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

			widgetView = new ChromiumWebBrowser(WIDGET_SERVER_URL);
			this.Controls.Add(widgetView);

			widgetView.IsLoadingChanged += widgetView_IsLoadingChanged;
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

		private void widgetView_IsLoadingChanged(object sender, CefSharp.IsLoadingChangedEventArgs e) {
			if(!widgetView.IsLoading) {
				widgetView.ExecuteScriptAsync("document.body.style['background'] = 'url(" + WALLPAPER_IMAGE_B64 + ") no-repeat center center fixed';");
				widgetView.ExecuteScriptAsync("document.body.style['background-size'] = 'cover';");
			}
		}
	}
}

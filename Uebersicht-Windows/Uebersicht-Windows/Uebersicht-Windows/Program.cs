using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uebersicht_Windows {
	class Program {
		public Program() {

			//window = new UBWindow();
			//Application.Run(window);
		}

		public void ShowPreferences() {
		}

		public void OpenWidgetDir() {
		}

		public void RefreshWidget() {
		}

		[STAThread]
		static void Main(string[] args) {
			//new Program();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new UBApplicationContext());
		}
	}
}

using System;
using System.Windows.Forms;

namespace KeyAuth
{
	// Token: 0x02000014 RID: 20
	internal static class Program
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x000024B6 File Offset: 0x000006B6
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Main());
		}
	}
}

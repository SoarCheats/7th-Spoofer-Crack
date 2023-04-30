using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using Siticone.UI.WinForms;
using Siticone.UI.WinForms.Enums;

namespace KeyAuth
{
	// Token: 0x02000011 RID: 17
	public partial class Login : Form
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00002450 File Offset: 0x00000650
		public Login()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00002417 File Offset: 0x00000617
		private void siticoneControlBox1_Click(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000051C8 File Offset: 0x000033C8
		public static bool SubExist(string name)
		{
			return Login.KeyAuthApp.user_data.subscriptions.Exists((api.Data x) => x.subscription == name);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005208 File Offset: 0x00003408
		private void Login_Load(object sender, EventArgs e)
		{
			Login.KeyAuthApp.init();
			if (Login.KeyAuthApp.response.message == "invalidver")
			{
				if (!string.IsNullOrEmpty(Login.KeyAuthApp.app_data.downloadLink))
				{
					DialogResult dialogResult = MessageBox.Show("Yes to open file in browser\nNo to download file automatically", "Auto update", MessageBoxButtons.YesNo);
					if (dialogResult != DialogResult.Yes)
					{
						if (dialogResult != DialogResult.No)
						{
							MessageBox.Show("Invalid option");
							Environment.Exit(0);
						}
						else
						{
							WebClient webClient = new WebClient();
							string text = Application.ExecutablePath;
							string str = Login.random_string();
							text = text.Replace(".exe", "-" + str + ".exe");
							webClient.DownloadFile(Login.KeyAuthApp.app_data.downloadLink, text);
							Process.Start(text);
							Process.Start(new ProcessStartInfo
							{
								Arguments = "/C choice /C Y /N /D Y /T 3 & Del \"" + Application.ExecutablePath + "\"",
								WindowStyle = ProcessWindowStyle.Hidden,
								CreateNoWindow = true,
								FileName = "cmd.exe"
							});
							Environment.Exit(0);
						}
					}
					else
					{
						Process.Start(Login.KeyAuthApp.app_data.downloadLink);
						Environment.Exit(0);
					}
				}
				MessageBox.Show("Version of this program does not match the one online. Furthermore, the download link online isn't set. You will need to manually obtain the download link from the developer");
				Environment.Exit(0);
			}
			if (!Login.KeyAuthApp.response.success)
			{
				MessageBox.Show(Login.KeyAuthApp.response.message);
				Environment.Exit(0);
			}
			Login.KeyAuthApp.check();
			this.siticoneLabel1.Text = string.Format("Current Session Validation Status: {0}", Login.KeyAuthApp.response.success);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000053A4 File Offset: 0x000035A4
		private static string random_string()
		{
			string text = null;
			Random random = new Random();
			for (int i = 0; i < 5; i++)
			{
				text += Convert.ToChar(Convert.ToInt32(Math.Floor(26.0 * random.NextDouble() + 65.0))).ToString();
			}
			return text;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00005400 File Offset: 0x00003600
		private void UpgradeBtn_Click(object sender, EventArgs e)
		{
			Login.KeyAuthApp.upgrade(this.username.Text, this.key.Text);
			this.status.Text = "Status: " + Login.KeyAuthApp.response.message;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005454 File Offset: 0x00003654
		private void LoginBtn_Click(object sender, EventArgs e)
		{
			Login.KeyAuthApp.login(this.username.Text, this.password.Text);
			if (Login.KeyAuthApp.response.success)
			{
				new Main().Show();
				base.Hide();
				return;
			}
			this.status.Text = "Status: " + Login.KeyAuthApp.response.message;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000054C8 File Offset: 0x000036C8
		private void RgstrBtn_Click(object sender, EventArgs e)
		{
			Login.KeyAuthApp.register(this.username.Text, this.password.Text, this.key.Text);
			if (Login.KeyAuthApp.response.success)
			{
				new Main().Show();
				base.Hide();
				return;
			}
			this.status.Text = "Status: " + Login.KeyAuthApp.response.message;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005548 File Offset: 0x00003748
		private void LicBtn_Click(object sender, EventArgs e)
		{
			Login.KeyAuthApp.license(this.key.Text);
			if (Login.KeyAuthApp.response.success)
			{
				new Main().Show();
				base.Hide();
				return;
			}
			this.status.Text = "Status: " + Login.KeyAuthApp.response.message;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00002422 File Offset: 0x00000622
		private void label2_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0400004F RID: 79
		public static api KeyAuthApp = new api("avs_spoofer", "o1kT5ZX76N", "d297fe0b17f14753ee52a3f061cd93a4658c94b8086260e97c567b0c8ed95414", "1.0");
	}
}

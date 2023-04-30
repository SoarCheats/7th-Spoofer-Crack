using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Siticone.UI.WinForms;
using Siticone.UI.WinForms.Enums;

namespace KeyAuth
{
	// Token: 0x02000010 RID: 16
	public partial class Main : Form
	{
		// Token: 0x06000090 RID: 144 RVA: 0x000023FE File Offset: 0x000005FE
		public Main()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00002417 File Offset: 0x00000617
		private void siticoneControlBox1_Click(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004478 File Offset: 0x00002678
		private void Main_Load(object sender, EventArgs e)
		{
			this.key.Text = "Username: CrackedBySoarCheats#1337";
			this.expiry.Text = "Expiry: " + this.UnixTimeToDateTime(1123123123L).ToString();
			this.subscription.Text = "Subscription: ";
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000241F File Offset: 0x0000061F
		public static bool SubExist(string name, int len)
		{
			return true;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000044D0 File Offset: 0x000026D0
		public DateTime UnixTimeToDateTime(long unixtime)
		{
			DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
			try
			{
				result = result.AddSeconds((double)unixtime).ToLocalTime();
			}
			catch
			{
				result = DateTime.MaxValue;
			}
			return result;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00002422 File Offset: 0x00000622
		private void timer1_Tick(object sender, EventArgs e)
		{
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00002422 File Offset: 0x00000622
		private void label3_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00002422 File Offset: 0x00000622
		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00002422 File Offset: 0x00000622
		private void chatmsg_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00002424 File Offset: 0x00000624
		private void siticoneRoundedButton1_Click(object sender, EventArgs e)
		{
			Spoofer.Spoof();
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00002422 File Offset: 0x00000622
		private void label2_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00002422 File Offset: 0x00000622
		private void siticoneLabel2_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00002422 File Offset: 0x00000622
		private void siticoneLabel2_Click_1(object sender, EventArgs e)
		{
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00002422 File Offset: 0x00000622
		private void siticoneLabel2_Click_2(object sender, EventArgs e)
		{
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00002422 File Offset: 0x00000622
		private void siticoneLabel2_Click_3(object sender, EventArgs e)
		{
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00002422 File Offset: 0x00000622
		private void key_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00002422 File Offset: 0x00000622
		private void expiry_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00002422 File Offset: 0x00000622
		private void subscriptionDaysLabel_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00002422 File Offset: 0x00000622
		private void siticoneLabel2_Click_4(object sender, EventArgs e)
		{
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00002422 File Offset: 0x00000622
		private void siticoneLabel3_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0400003C RID: 60
		private string chatchannel = "testing";
	}
}

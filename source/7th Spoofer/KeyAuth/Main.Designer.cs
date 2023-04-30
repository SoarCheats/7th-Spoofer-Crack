namespace KeyAuth
{
	// Token: 0x02000010 RID: 16
	public partial class Main : global::System.Windows.Forms.Form
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x0000242B File Offset: 0x0000062B
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004520 File Offset: 0x00002720
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::KeyAuth.Main));
			this.siticoneDragControl1 = new global::Siticone.UI.WinForms.SiticoneDragControl(this.components);
			this.siticoneControlBox1 = new global::Siticone.UI.WinForms.SiticoneControlBox();
			this.siticoneControlBox2 = new global::Siticone.UI.WinForms.SiticoneControlBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.key = new global::Siticone.UI.WinForms.SiticoneLabel();
			this.expiry = new global::Siticone.UI.WinForms.SiticoneLabel();
			this.subscription = new global::Siticone.UI.WinForms.SiticoneLabel();
			this.ip = new global::Siticone.UI.WinForms.SiticoneLabel();
			this.hwid = new global::Siticone.UI.WinForms.SiticoneLabel();
			this.lastLogin = new global::Siticone.UI.WinForms.SiticoneLabel();
			this.siticoneLabel1 = new global::Siticone.UI.WinForms.SiticoneLabel();
			this.siticoneShadowForm = new global::Siticone.UI.WinForms.SiticoneShadowForm(this.components);
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.siticoneRoundedButton1 = new global::Siticone.UI.WinForms.SiticoneRoundedButton();
			this.siticoneLabel2 = new global::Siticone.UI.WinForms.SiticoneLabel();
			this.siticoneLabel3 = new global::Siticone.UI.WinForms.SiticoneLabel();
			base.SuspendLayout();
			this.siticoneDragControl1.TargetControl = this;
			this.siticoneControlBox1.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right);
			this.siticoneControlBox1.BorderRadius = 10;
			this.siticoneControlBox1.FillColor = global::System.Drawing.Color.Black;
			this.siticoneControlBox1.HoveredState.FillColor = global::System.Drawing.Color.FromArgb(232, 17, 35);
			this.siticoneControlBox1.HoveredState.IconColor = global::System.Drawing.Color.White;
			this.siticoneControlBox1.HoveredState.Parent = this.siticoneControlBox1;
			this.siticoneControlBox1.IconColor = global::System.Drawing.Color.White;
			this.siticoneControlBox1.Location = new global::System.Drawing.Point(495, 1);
			this.siticoneControlBox1.Name = "siticoneControlBox1";
			this.siticoneControlBox1.ShadowDecoration.Parent = this.siticoneControlBox1;
			this.siticoneControlBox1.Size = new global::System.Drawing.Size(33, 29);
			this.siticoneControlBox1.TabIndex = 1;
			this.siticoneControlBox1.Click += new global::System.EventHandler(this.siticoneControlBox1_Click);
			this.siticoneControlBox2.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right);
			this.siticoneControlBox2.BackColor = global::System.Drawing.Color.Black;
			this.siticoneControlBox2.BorderRadius = 10;
			this.siticoneControlBox2.ControlBoxType = global::Siticone.UI.WinForms.Enums.ControlBoxType.MinimizeBox;
			this.siticoneControlBox2.FillColor = global::System.Drawing.Color.Black;
			this.siticoneControlBox2.HoveredState.Parent = this.siticoneControlBox2;
			this.siticoneControlBox2.IconColor = global::System.Drawing.Color.White;
			this.siticoneControlBox2.Location = new global::System.Drawing.Point(455, 1);
			this.siticoneControlBox2.Name = "siticoneControlBox2";
			this.siticoneControlBox2.ShadowDecoration.Parent = this.siticoneControlBox2;
			this.siticoneControlBox2.Size = new global::System.Drawing.Size(40, 29);
			this.siticoneControlBox2.TabIndex = 2;
			this.label1.AutoSize = true;
			this.label1.Font = new global::System.Drawing.Font("Segoe UI Light", 10f);
			this.label1.ForeColor = global::System.Drawing.Color.White;
			this.label1.Location = new global::System.Drawing.Point(-1, 136);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(0, 19);
			this.label1.TabIndex = 22;
			this.label2.AutoSize = true;
			this.label2.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.label2.Font = new global::System.Drawing.Font("Arial Black", 21.75f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label2.ForeColor = global::System.Drawing.Color.Yellow;
			this.label2.ImageAlign = global::System.Drawing.ContentAlignment.TopLeft;
			this.label2.Location = new global::System.Drawing.Point(151, 9);
			this.label2.Margin = new global::System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(200, 41);
			this.label2.TabIndex = 27;
			this.label2.Text = "7th Spoofer";
			this.label2.Click += new global::System.EventHandler(this.label2_Click);
			this.key.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.key.AutoSize = false;
			this.key.BackColor = global::System.Drawing.Color.Transparent;
			this.key.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Center;
			this.key.Font = new global::System.Drawing.Font("Arial Black", 11.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.key.ForeColor = global::System.Drawing.Color.Yellow;
			this.key.Location = new global::System.Drawing.Point(11, 108);
			this.key.Margin = new global::System.Windows.Forms.Padding(2, 2, 2, 2);
			this.key.Name = "key";
			this.key.Size = new global::System.Drawing.Size(340, 29);
			this.key.TabIndex = 37;
			this.key.Text = "usernameLabel";
			this.key.Click += new global::System.EventHandler(this.key_Click);
			this.expiry.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.expiry.AutoSize = false;
			this.expiry.BackColor = global::System.Drawing.Color.Transparent;
			this.expiry.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Center;
			this.expiry.Font = new global::System.Drawing.Font("Arial Black", 11.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.expiry.ForeColor = global::System.Drawing.Color.Yellow;
			this.expiry.Location = new global::System.Drawing.Point(11, 141);
			this.expiry.Margin = new global::System.Windows.Forms.Padding(2, 2, 2, 2);
			this.expiry.Name = "expiry";
			this.expiry.Size = new global::System.Drawing.Size(340, 29);
			this.expiry.TabIndex = 38;
			this.expiry.Text = "expiryLabel";
			this.expiry.Click += new global::System.EventHandler(this.expiry_Click);
			this.subscription.BackColor = global::System.Drawing.Color.Transparent;
			this.subscription.Location = new global::System.Drawing.Point(0, 0);
			this.subscription.Name = "subscription";
			this.subscription.Size = new global::System.Drawing.Size(3, 2);
			this.subscription.TabIndex = 65;
			this.subscription.Text = null;
			this.ip.BackColor = global::System.Drawing.Color.Transparent;
			this.ip.Location = new global::System.Drawing.Point(0, 0);
			this.ip.Name = "ip";
			this.ip.Size = new global::System.Drawing.Size(3, 2);
			this.ip.TabIndex = 0;
			this.ip.Text = null;
			this.hwid.BackColor = global::System.Drawing.Color.Transparent;
			this.hwid.Location = new global::System.Drawing.Point(0, 0);
			this.hwid.Name = "hwid";
			this.hwid.Size = new global::System.Drawing.Size(3, 2);
			this.hwid.TabIndex = 0;
			this.hwid.Text = null;
			this.lastLogin.BackColor = global::System.Drawing.Color.Transparent;
			this.lastLogin.Location = new global::System.Drawing.Point(0, 0);
			this.lastLogin.Margin = new global::System.Windows.Forms.Padding(2, 2, 2, 2);
			this.lastLogin.Name = "lastLogin";
			this.lastLogin.Size = new global::System.Drawing.Size(3, 2);
			this.lastLogin.TabIndex = 62;
			this.lastLogin.Text = null;
			this.siticoneLabel1.BackColor = global::System.Drawing.Color.Transparent;
			this.siticoneLabel1.Location = new global::System.Drawing.Point(0, 0);
			this.siticoneLabel1.Name = "siticoneLabel1";
			this.siticoneLabel1.Size = new global::System.Drawing.Size(3, 2);
			this.siticoneLabel1.TabIndex = 61;
			this.siticoneLabel1.Text = null;
			this.timer1.Enabled = true;
			this.timer1.Interval = 1;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			this.siticoneRoundedButton1.BackColor = global::System.Drawing.Color.Transparent;
			this.siticoneRoundedButton1.BorderColor = global::System.Drawing.Color.Yellow;
			this.siticoneRoundedButton1.BorderThickness = 1;
			this.siticoneRoundedButton1.CheckedState.Parent = this.siticoneRoundedButton1;
			this.siticoneRoundedButton1.CustomBorderColor = global::System.Drawing.Color.Black;
			this.siticoneRoundedButton1.CustomImages.Parent = this.siticoneRoundedButton1;
			this.siticoneRoundedButton1.FillColor = global::System.Drawing.Color.Black;
			this.siticoneRoundedButton1.Font = new global::System.Drawing.Font("Segoe UI", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.siticoneRoundedButton1.ForeColor = global::System.Drawing.Color.Yellow;
			this.siticoneRoundedButton1.HoveredState.BorderColor = global::System.Drawing.Color.FromArgb(213, 218, 223);
			this.siticoneRoundedButton1.HoveredState.Parent = this.siticoneRoundedButton1;
			this.siticoneRoundedButton1.Location = new global::System.Drawing.Point(158, 264);
			this.siticoneRoundedButton1.Name = "siticoneRoundedButton1";
			this.siticoneRoundedButton1.ShadowDecoration.Parent = this.siticoneRoundedButton1;
			this.siticoneRoundedButton1.Size = new global::System.Drawing.Size(193, 49);
			this.siticoneRoundedButton1.TabIndex = 54;
			this.siticoneRoundedButton1.Text = "Spoof";
			this.siticoneRoundedButton1.Click += new global::System.EventHandler(this.siticoneRoundedButton1_Click);
			this.siticoneLabel2.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.siticoneLabel2.AutoSize = false;
			this.siticoneLabel2.BackColor = global::System.Drawing.Color.Transparent;
			this.siticoneLabel2.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Center;
			this.siticoneLabel2.Font = new global::System.Drawing.Font("Arial Black", 11.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.siticoneLabel2.ForeColor = global::System.Drawing.Color.White;
			this.siticoneLabel2.Location = new global::System.Drawing.Point(11, 75);
			this.siticoneLabel2.Margin = new global::System.Windows.Forms.Padding(2);
			this.siticoneLabel2.Name = "siticoneLabel2";
			this.siticoneLabel2.Size = new global::System.Drawing.Size(243, 29);
			this.siticoneLabel2.TabIndex = 66;
			this.siticoneLabel2.Text = "USER INFO";
			this.siticoneLabel2.Click += new global::System.EventHandler(this.siticoneLabel2_Click_4);
			this.siticoneLabel3.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.siticoneLabel3.AutoSize = false;
			this.siticoneLabel3.BackColor = global::System.Drawing.Color.Transparent;
			this.siticoneLabel3.BackgroundImage = (global::System.Drawing.Image)componentResourceManager.GetObject("siticoneLabel3.BackgroundImage");
			this.siticoneLabel3.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Zoom;
			this.siticoneLabel3.Font = new global::System.Drawing.Font("Arial Black", 11.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.siticoneLabel3.ForeColor = global::System.Drawing.Color.Black;
			this.siticoneLabel3.Location = new global::System.Drawing.Point(0, 1);
			this.siticoneLabel3.Margin = new global::System.Windows.Forms.Padding(2);
			this.siticoneLabel3.Name = "siticoneLabel3";
			this.siticoneLabel3.Size = new global::System.Drawing.Size(97, 60);
			this.siticoneLabel3.TabIndex = 67;
			this.siticoneLabel3.Click += new global::System.EventHandler(this.siticoneLabel3_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoValidate = global::System.Windows.Forms.AutoValidate.Disable;
			this.BackColor = global::System.Drawing.Color.Black;
			this.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Center;
			base.ClientSize = new global::System.Drawing.Size(530, 353);
			base.Controls.Add(this.siticoneLabel3);
			base.Controls.Add(this.siticoneLabel2);
			base.Controls.Add(this.siticoneRoundedButton1);
			base.Controls.Add(this.siticoneLabel1);
			base.Controls.Add(this.lastLogin);
			base.Controls.Add(this.subscription);
			base.Controls.Add(this.expiry);
			base.Controls.Add(this.key);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.siticoneControlBox2);
			base.Controls.Add(this.siticoneControlBox1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.None;
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "Main";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "7th Spoofer";
			base.TopMost = true;
			base.Load += new global::System.EventHandler(this.Main_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400003D RID: 61
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400003E RID: 62
		private global::Siticone.UI.WinForms.SiticoneDragControl siticoneDragControl1;

		// Token: 0x0400003F RID: 63
		private global::Siticone.UI.WinForms.SiticoneControlBox siticoneControlBox1;

		// Token: 0x04000040 RID: 64
		private global::Siticone.UI.WinForms.SiticoneControlBox siticoneControlBox2;

		// Token: 0x04000041 RID: 65
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000042 RID: 66
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000043 RID: 67
		private global::Siticone.UI.WinForms.SiticoneShadowForm siticoneShadowForm;

		// Token: 0x04000044 RID: 68
		private global::Siticone.UI.WinForms.SiticoneLabel subscription;

		// Token: 0x04000045 RID: 69
		private global::Siticone.UI.WinForms.SiticoneLabel expiry;

		// Token: 0x04000046 RID: 70
		private global::Siticone.UI.WinForms.SiticoneLabel key;

		// Token: 0x04000047 RID: 71
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04000048 RID: 72
		private global::Siticone.UI.WinForms.SiticoneLabel ip;

		// Token: 0x04000049 RID: 73
		private global::Siticone.UI.WinForms.SiticoneLabel hwid;

		// Token: 0x0400004A RID: 74
		private global::Siticone.UI.WinForms.SiticoneLabel lastLogin;

		// Token: 0x0400004B RID: 75
		private global::Siticone.UI.WinForms.SiticoneLabel siticoneLabel1;

		// Token: 0x0400004C RID: 76
		private global::Siticone.UI.WinForms.SiticoneRoundedButton siticoneRoundedButton1;

		// Token: 0x0400004D RID: 77
		private global::Siticone.UI.WinForms.SiticoneLabel siticoneLabel2;

		// Token: 0x0400004E RID: 78
		private global::Siticone.UI.WinForms.SiticoneLabel siticoneLabel3;
	}
}

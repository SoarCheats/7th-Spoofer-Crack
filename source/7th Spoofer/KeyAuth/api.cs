using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows;

namespace KeyAuth
{
	// Token: 0x02000004 RID: 4
	public class api
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000024D0 File Offset: 0x000006D0
		public api(string name, string ownerid, string secret, string version)
		{
			if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(ownerid) || string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(version))
			{
				api.error("Application not setup correctly. Please watch video link found in Program.cs \n Make sure you've added your application name, secret, ownerID, and version in correctly, and that you have KeyAuthApp.init(); on load.");
				Environment.Exit(0);
			}
			this.name = name;
			this.ownerid = ownerid;
			this.secret = secret;
			this.version = version;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002564 File Offset: 0x00000764
		public void init()
		{
			this.enckey = encryption.sha256(encryption.iv_key());
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("init"));
			nameValueCollection["ver"] = encryption.encrypt(this.version, this.secret, text);
			nameValueCollection["hash"] = api.checksum(Process.GetCurrentProcess().MainModule.FileName);
			nameValueCollection["enckey"] = encryption.encrypt(this.enckey, this.secret, text);
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			MessageBox.Show(text2);
			if (text2 == "KeyAuth_Invalid")
			{
				api.error("Application not found. Please check your application name, secret, ownerID, and version.");
				Environment.Exit(0);
			}
			text2 = encryption.decrypt(text2, this.secret, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				this.load_app_data(response_structure.appinfo);
				this.sessionid = response_structure.sessionid;
				this.initialized = true;
				return;
			}
			if (response_structure.message == "invalidver")
			{
				this.app_data.downloadLink = response_structure.download;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000026EC File Offset: 0x000008EC
		public void register(string username, string pass, string key)
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load.");
				Environment.Exit(0);
			}
			string value = WindowsIdentity.GetCurrent().User.Value;
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("register"));
			nameValueCollection["username"] = encryption.encrypt(username, this.enckey, text);
			nameValueCollection["pass"] = encryption.encrypt(pass, this.enckey, text);
			nameValueCollection["key"] = encryption.encrypt(key, this.enckey, text);
			nameValueCollection["hwid"] = encryption.encrypt(value, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				this.load_user_data(response_structure.info);
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002858 File Offset: 0x00000A58
		public void login(string username, string pass)
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load.");
			}
			string value = WindowsIdentity.GetCurrent().User.Value;
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("login"));
			nameValueCollection["username"] = encryption.encrypt(username, this.enckey, text);
			nameValueCollection["pass"] = encryption.encrypt(pass, this.enckey, text);
			nameValueCollection["hwid"] = encryption.encrypt(value, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				this.load_user_data(response_structure.info);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000029A8 File Offset: 0x00000BA8
		public void web_login()
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load.");
				Environment.Exit(0);
			}
			string value = WindowsIdentity.GetCurrent().User.Value;
			HttpListener httpListener = new HttpListener();
			string text = "handshake";
			text = "http://localhost:1337/" + text + "/";
			httpListener.Prefixes.Add(text);
			httpListener.Start();
			HttpListenerContext context = httpListener.GetContext();
			HttpListenerRequest request = context.Request;
			HttpListenerResponse httpListenerResponse = context.Response;
			httpListenerResponse.AddHeader("Access-Control-Allow-Methods", "GET, POST");
			httpListenerResponse.AddHeader("Access-Control-Allow-Origin", "*");
			httpListenerResponse.AddHeader("Via", "Via");
			httpListenerResponse.AddHeader("Location", "Location");
			httpListenerResponse.AddHeader("Retry-After", "Retry");
			httpListenerResponse.Headers.Add("Server", "\r\n\r\n");
			httpListener.AuthenticationSchemes = AuthenticationSchemes.Negotiate;
			httpListener.UnsafeConnectionNtlmAuthentication = true;
			httpListener.IgnoreWriteExceptions = true;
			string text2 = request.RawUrl.Replace("/handshake?user=", "").Replace("&token=", " ");
			string value2 = text2.Split(Array.Empty<char>())[0];
			string value3 = text2.Split(new char[]
			{
				' '
			})[1];
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "login";
			nameValueCollection["username"] = value2;
			nameValueCollection["token"] = value3;
			nameValueCollection["hwid"] = value;
			nameValueCollection["sessionid"] = this.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req_unenc(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(response_structure);
			bool flag = true;
			if (response_structure.success)
			{
				this.load_user_data(response_structure.info);
				httpListenerResponse.StatusCode = 420;
				httpListenerResponse.StatusDescription = "SHEESH";
			}
			else
			{
				Console.WriteLine(response_structure.message);
				httpListenerResponse.StatusCode = 200;
				httpListenerResponse.StatusDescription = response_structure.message;
				flag = false;
			}
			byte[] bytes = Encoding.UTF8.GetBytes("Whats up?");
			httpListenerResponse.ContentLength64 = (long)bytes.Length;
			httpListenerResponse.OutputStream.Write(bytes, 0, bytes.Length);
			Thread.Sleep(1250);
			httpListener.Stop();
			if (!flag)
			{
				Environment.Exit(0);
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002C1C File Offset: 0x00000E1C
		public void button(string button)
		{
			if (!this.initialized)
			{
				api.error("Please initialize first");
				Environment.Exit(0);
			}
			HttpListener httpListener = new HttpListener();
			string uriPrefix = "http://localhost:1337/" + button + "/";
			httpListener.Prefixes.Add(uriPrefix);
			httpListener.Start();
			HttpListenerContext context = httpListener.GetContext();
			HttpListenerRequest request = context.Request;
			HttpListenerResponse httpListenerResponse = context.Response;
			httpListenerResponse.AddHeader("Access-Control-Allow-Methods", "GET, POST");
			httpListenerResponse.AddHeader("Access-Control-Allow-Origin", "*");
			httpListenerResponse.AddHeader("Via", "Via");
			httpListenerResponse.AddHeader("Location", "Location");
			httpListenerResponse.AddHeader("Retry-After", "Rety");
			httpListenerResponse.Headers.Add("Server", "\r\n\r\n");
			httpListenerResponse.StatusCode = 420;
			httpListenerResponse.StatusDescription = "SHEESH";
			httpListener.AuthenticationSchemes = AuthenticationSchemes.Negotiate;
			httpListener.UnsafeConnectionNtlmAuthentication = true;
			httpListener.IgnoreWriteExceptions = true;
			httpListener.Stop();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002D14 File Offset: 0x00000F14
		public void upgrade(string username, string key)
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load.");
				Environment.Exit(0);
			}
			string value = WindowsIdentity.GetCurrent().User.Value;
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("upgrade"));
			nameValueCollection["username"] = encryption.encrypt(username, this.enckey, text);
			nameValueCollection["key"] = encryption.encrypt(key, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			response_structure.success = false;
			this.load_response_struct(response_structure);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002E44 File Offset: 0x00001044
		public void license(string key)
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load.");
				Environment.Exit(0);
			}
			string value = WindowsIdentity.GetCurrent().User.Value;
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("license"));
			nameValueCollection["key"] = encryption.encrypt(key, this.enckey, text);
			nameValueCollection["hwid"] = encryption.encrypt(value, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				this.load_user_data(response_structure.info);
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002F80 File Offset: 0x00001180
		public void check()
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load.");
				Environment.Exit(0);
			}
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("check"));
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure data = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(data);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00003068 File Offset: 0x00001268
		public void setvar(string var, string data)
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load.");
				Environment.Exit(0);
			}
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("setvar"));
			nameValueCollection["var"] = encryption.encrypt(var, this.enckey, text);
			nameValueCollection["data"] = encryption.encrypt(data, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure data2 = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(data2);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00003180 File Offset: 0x00001380
		public string getvar(string var)
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load.");
				Environment.Exit(0);
			}
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("getvar"));
			nameValueCollection["var"] = encryption.encrypt(var, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				return response_structure.response;
			}
			return null;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00003290 File Offset: 0x00001490
		public void ban(string reason = null)
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load.");
				Environment.Exit(0);
			}
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("ban"));
			nameValueCollection["reason"] = reason;
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure data = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(data);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00003384 File Offset: 0x00001584
		public string var(string varid)
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load.");
				Environment.Exit(0);
			}
			string value = WindowsIdentity.GetCurrent().User.Value;
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("var"));
			nameValueCollection["varid"] = encryption.encrypt(varid, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				return response_structure.message;
			}
			return null;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000034A4 File Offset: 0x000016A4
		public List<api.users> fetchOnline()
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load.");
				Environment.Exit(0);
			}
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("fetchOnline"));
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				return response_structure.users;
			}
			return null;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000359C File Offset: 0x0000179C
		public List<api.msg> chatget(string channelname)
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load.");
				Environment.Exit(0);
			}
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("chatget"));
			nameValueCollection["channel"] = encryption.encrypt(channelname, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				return response_structure.messages;
			}
			return null;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000036AC File Offset: 0x000018AC
		public bool chatsend(string msg, string channelname)
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load.");
				Environment.Exit(0);
			}
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("chatsend"));
			nameValueCollection["message"] = encryption.encrypt(msg, this.enckey, text);
			nameValueCollection["channel"] = encryption.encrypt(channelname, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			return response_structure.success;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000037D0 File Offset: 0x000019D0
		public bool checkblack()
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load.");
				Environment.Exit(0);
			}
			string value = WindowsIdentity.GetCurrent().User.Value;
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("checkblacklist"));
			nameValueCollection["hwid"] = encryption.encrypt(value, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			return response_structure.success;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000038EC File Offset: 0x00001AEC
		public string webhook(string webid, string param, string body = "", string conttype = "")
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load.");
				Environment.Exit(0);
				return null;
			}
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("webhook"));
			nameValueCollection["webid"] = encryption.encrypt(webid, this.enckey, text);
			nameValueCollection["params"] = encryption.encrypt(param, this.enckey, text);
			nameValueCollection["body"] = encryption.encrypt(body, this.enckey, text);
			nameValueCollection["conttype"] = encryption.encrypt(conttype, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				return response_structure.response;
			}
			return null;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00003A48 File Offset: 0x00001C48
		public byte[] download(string fileid)
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load. File is empty since no request could be made.");
				Environment.Exit(0);
			}
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("file"));
			nameValueCollection["fileid"] = encryption.encrypt(fileid, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				return encryption.str_to_byte_arr(response_structure.contents);
			}
			return null;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00003B60 File Offset: 0x00001D60
		public void log(string message)
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load.");
				Environment.Exit(0);
			}
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("log"));
			nameValueCollection["pcuser"] = encryption.encrypt(Environment.UserName, this.enckey, text);
			nameValueCollection["message"] = encryption.encrypt(message, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			api.req(nameValueCollection);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003C5C File Offset: 0x00001E5C
		public static string checksum(string filename)
		{
			string result;
			using (MD5 md = MD5.Create())
			{
				using (FileStream fileStream = File.OpenRead(filename))
				{
					result = BitConverter.ToString(md.ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant();
				}
			}
			return result;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00003CCC File Offset: 0x00001ECC
		public static void error(string message)
		{
			Process.Start(new ProcessStartInfo("cmd.exe", "/c start cmd /C \"color b && title Error && echo " + message + " && timeout /t 5\"")
			{
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false
			});
			Environment.Exit(0);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003D1C File Offset: 0x00001F1C
		private static string req(NameValueCollection post_data)
		{
			api.RemoveUnwantedCert();
			string result;
			try
			{
				using (WebClient webClient = new WebClient())
				{
					byte[] bytes = webClient.UploadValues("https://keyauth.win/api/1.0/", post_data);
					result = Encoding.Default.GetString(bytes);
				}
			}
			catch (WebException ex)
			{
				if (((HttpWebResponse)ex.Response).StatusCode == (HttpStatusCode)429)
				{
					api.error("You're connecting too fast to loader, slow down.");
					Environment.Exit(0);
					result = "";
				}
				else
				{
					api.error("Connection failure. Please try again, or contact us for help.");
					Environment.Exit(0);
					result = "";
				}
			}
			return result;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00003DBC File Offset: 0x00001FBC
		private static string req_unenc(NameValueCollection post_data)
		{
			api.RemoveUnwantedCert();
			string result;
			try
			{
				using (WebClient webClient = new WebClient())
				{
					byte[] bytes = webClient.UploadValues("https://keyauth.win/api/1.1/", post_data);
					result = Encoding.Default.GetString(bytes);
				}
			}
			catch (WebException ex)
			{
				if (((HttpWebResponse)ex.Response).StatusCode == (HttpStatusCode)429)
				{
					Thread.Sleep(1000);
					result = api.req(post_data);
				}
				else
				{
					api.error("Connection failure. Please try again, or contact us for help.");
					Environment.Exit(0);
					result = "";
				}
			}
			return result;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003E58 File Offset: 0x00002058
		private static void RemoveUnwantedCert()
		{
			string storeName = "Root";
			StoreLocation storeLocation = StoreLocation.LocalMachine;
			X509Store x509Store = new X509Store(storeName, storeLocation);
			x509Store.Open(OpenFlags.ReadOnly);
			foreach (X509Certificate2 x509Certificate in x509Store.Certificates)
			{
				if (x509Certificate.SubjectName.Name == "CN=asdhashdgashd")
				{
					try
					{
						x509Store.Open(OpenFlags.ReadWrite);
						x509Store.Remove(x509Certificate);
						x509Store.Close();
						break;
					}
					catch (Exception)
					{
						break;
					}
				}
			}
			x509Store.Close();
			string storeName2 = "MY";
			StoreLocation storeLocation2 = StoreLocation.LocalMachine;
			X509Store x509Store2 = new X509Store(storeName2, storeLocation2);
			x509Store.Open(OpenFlags.ReadOnly);
			foreach (X509Certificate2 x509Certificate2 in x509Store.Certificates)
			{
				if (x509Certificate2.SubjectName.Name == "CN=asdhashdgashd")
				{
					try
					{
						x509Store2.Open(OpenFlags.ReadWrite);
						x509Store2.Remove(x509Certificate2);
						x509Store2.Close();
						break;
					}
					catch (Exception)
					{
						break;
					}
				}
			}
			x509Store2.Close();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003F60 File Offset: 0x00002160
		private void load_app_data(api.app_data_structure data)
		{
			this.app_data.numUsers = data.numUsers;
			this.app_data.numOnlineUsers = data.numOnlineUsers;
			this.app_data.numKeys = data.numKeys;
			this.app_data.version = data.version;
			this.app_data.customerPanelLink = data.customerPanelLink;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003FC4 File Offset: 0x000021C4
		private void load_user_data(api.user_data_structure data)
		{
			this.user_data.username = data.username;
			this.user_data.ip = data.ip;
			this.user_data.hwid = data.hwid;
			this.user_data.createdate = data.createdate;
			this.user_data.lastlogin = data.lastlogin;
			this.user_data.subscriptions = data.subscriptions;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00004038 File Offset: 0x00002238
		public string expirydaysleft()
		{
			if (!this.initialized)
			{
				api.error("Please initialize first. Add KeyAuthApp.init(); on load.");
				Environment.Exit(0);
			}
			DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
			d = d.AddSeconds((double)long.Parse(this.user_data.subscriptions[0].expiry)).ToLocalTime();
			TimeSpan timeSpan = d - DateTime.Now;
			return Convert.ToString(timeSpan.Days.ToString() + " Days " + timeSpan.Hours.ToString() + " Hours Left");
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000020B8 File Offset: 0x000002B8
		private void load_response_struct(api.response_structure data)
		{
			this.response.success = data.success;
			this.response.message = data.message;
		}

		// Token: 0x04000004 RID: 4
		public string name;

		// Token: 0x04000005 RID: 5
		public string ownerid;

		// Token: 0x04000006 RID: 6
		public string secret;

		// Token: 0x04000007 RID: 7
		public string version;

		// Token: 0x04000008 RID: 8
		private string sessionid;

		// Token: 0x04000009 RID: 9
		private string enckey;

		// Token: 0x0400000A RID: 10
		private bool initialized;

		// Token: 0x0400000B RID: 11
		public api.app_data_class app_data = new api.app_data_class();

		// Token: 0x0400000C RID: 12
		public api.user_data_class user_data = new api.user_data_class();

		// Token: 0x0400000D RID: 13
		public api.response_class response = new api.response_class();

		// Token: 0x0400000E RID: 14
		private json_wrapper response_decoder = new json_wrapper(new api.response_structure());

		// Token: 0x02000005 RID: 5
		[DataContract]
		private class response_structure
		{
			// Token: 0x17000004 RID: 4
			// (get) Token: 0x06000025 RID: 37 RVA: 0x000020DC File Offset: 0x000002DC
			// (set) Token: 0x06000026 RID: 38 RVA: 0x000020E4 File Offset: 0x000002E4
			[DataMember]
			public bool success { get; set; }

			// Token: 0x17000005 RID: 5
			// (get) Token: 0x06000027 RID: 39 RVA: 0x000020ED File Offset: 0x000002ED
			// (set) Token: 0x06000028 RID: 40 RVA: 0x000020F5 File Offset: 0x000002F5
			[DataMember]
			public string sessionid { get; set; }

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x06000029 RID: 41 RVA: 0x000020FE File Offset: 0x000002FE
			// (set) Token: 0x0600002A RID: 42 RVA: 0x00002106 File Offset: 0x00000306
			[DataMember]
			public string contents { get; set; }

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x0600002B RID: 43 RVA: 0x0000210F File Offset: 0x0000030F
			// (set) Token: 0x0600002C RID: 44 RVA: 0x00002117 File Offset: 0x00000317
			[DataMember]
			public string response { get; set; }

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x0600002D RID: 45 RVA: 0x00002120 File Offset: 0x00000320
			// (set) Token: 0x0600002E RID: 46 RVA: 0x00002128 File Offset: 0x00000328
			[DataMember]
			public string message { get; set; }

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x0600002F RID: 47 RVA: 0x00002131 File Offset: 0x00000331
			// (set) Token: 0x06000030 RID: 48 RVA: 0x00002139 File Offset: 0x00000339
			[DataMember]
			public string download { get; set; }

			// Token: 0x1700000A RID: 10
			// (get) Token: 0x06000031 RID: 49 RVA: 0x00002142 File Offset: 0x00000342
			// (set) Token: 0x06000032 RID: 50 RVA: 0x0000214A File Offset: 0x0000034A
			[DataMember(IsRequired = false, EmitDefaultValue = false)]
			public api.user_data_structure info { get; set; }

			// Token: 0x1700000B RID: 11
			// (get) Token: 0x06000033 RID: 51 RVA: 0x00002153 File Offset: 0x00000353
			// (set) Token: 0x06000034 RID: 52 RVA: 0x0000215B File Offset: 0x0000035B
			[DataMember(IsRequired = false, EmitDefaultValue = false)]
			public api.app_data_structure appinfo { get; set; }

			// Token: 0x1700000C RID: 12
			// (get) Token: 0x06000035 RID: 53 RVA: 0x00002164 File Offset: 0x00000364
			// (set) Token: 0x06000036 RID: 54 RVA: 0x0000216C File Offset: 0x0000036C
			[DataMember]
			public List<api.msg> messages { get; set; }

			// Token: 0x1700000D RID: 13
			// (get) Token: 0x06000037 RID: 55 RVA: 0x00002175 File Offset: 0x00000375
			// (set) Token: 0x06000038 RID: 56 RVA: 0x0000217D File Offset: 0x0000037D
			[DataMember]
			public List<api.users> users { get; set; }
		}

		// Token: 0x02000006 RID: 6
		public class msg
		{
			// Token: 0x1700000E RID: 14
			// (get) Token: 0x0600003A RID: 58 RVA: 0x00002186 File Offset: 0x00000386
			// (set) Token: 0x0600003B RID: 59 RVA: 0x0000218E File Offset: 0x0000038E
			public string message { get; set; }

			// Token: 0x1700000F RID: 15
			// (get) Token: 0x0600003C RID: 60 RVA: 0x00002197 File Offset: 0x00000397
			// (set) Token: 0x0600003D RID: 61 RVA: 0x0000219F File Offset: 0x0000039F
			public string author { get; set; }

			// Token: 0x17000010 RID: 16
			// (get) Token: 0x0600003E RID: 62 RVA: 0x000021A8 File Offset: 0x000003A8
			// (set) Token: 0x0600003F RID: 63 RVA: 0x000021B0 File Offset: 0x000003B0
			public string timestamp { get; set; }
		}

		// Token: 0x02000007 RID: 7
		public class users
		{
			// Token: 0x17000011 RID: 17
			// (get) Token: 0x06000041 RID: 65 RVA: 0x000021B9 File Offset: 0x000003B9
			// (set) Token: 0x06000042 RID: 66 RVA: 0x000021C1 File Offset: 0x000003C1
			public string credential { get; set; }
		}

		// Token: 0x02000008 RID: 8
		[DataContract]
		private class user_data_structure
		{
			// Token: 0x17000012 RID: 18
			// (get) Token: 0x06000044 RID: 68 RVA: 0x000021CA File Offset: 0x000003CA
			// (set) Token: 0x06000045 RID: 69 RVA: 0x000021D2 File Offset: 0x000003D2
			[DataMember]
			public string username { get; set; }

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x06000046 RID: 70 RVA: 0x000021DB File Offset: 0x000003DB
			// (set) Token: 0x06000047 RID: 71 RVA: 0x000021E3 File Offset: 0x000003E3
			[DataMember]
			public string ip { get; set; }

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x06000048 RID: 72 RVA: 0x000021EC File Offset: 0x000003EC
			// (set) Token: 0x06000049 RID: 73 RVA: 0x000021F4 File Offset: 0x000003F4
			[DataMember]
			public string hwid { get; set; }

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x0600004A RID: 74 RVA: 0x000021FD File Offset: 0x000003FD
			// (set) Token: 0x0600004B RID: 75 RVA: 0x00002205 File Offset: 0x00000405
			[DataMember]
			public string createdate { get; set; }

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x0600004C RID: 76 RVA: 0x0000220E File Offset: 0x0000040E
			// (set) Token: 0x0600004D RID: 77 RVA: 0x00002216 File Offset: 0x00000416
			[DataMember]
			public string lastlogin { get; set; }

			// Token: 0x17000017 RID: 23
			// (get) Token: 0x0600004E RID: 78 RVA: 0x0000221F File Offset: 0x0000041F
			// (set) Token: 0x0600004F RID: 79 RVA: 0x00002227 File Offset: 0x00000427
			[DataMember]
			public List<api.Data> subscriptions { get; set; }
		}

		// Token: 0x02000009 RID: 9
		[DataContract]
		private class app_data_structure
		{
			// Token: 0x17000018 RID: 24
			// (get) Token: 0x06000051 RID: 81 RVA: 0x00002230 File Offset: 0x00000430
			// (set) Token: 0x06000052 RID: 82 RVA: 0x00002238 File Offset: 0x00000438
			[DataMember]
			public string numUsers { get; set; }

			// Token: 0x17000019 RID: 25
			// (get) Token: 0x06000053 RID: 83 RVA: 0x00002241 File Offset: 0x00000441
			// (set) Token: 0x06000054 RID: 84 RVA: 0x00002249 File Offset: 0x00000449
			[DataMember]
			public string numOnlineUsers { get; set; }

			// Token: 0x1700001A RID: 26
			// (get) Token: 0x06000055 RID: 85 RVA: 0x00002252 File Offset: 0x00000452
			// (set) Token: 0x06000056 RID: 86 RVA: 0x0000225A File Offset: 0x0000045A
			[DataMember]
			public string numKeys { get; set; }

			// Token: 0x1700001B RID: 27
			// (get) Token: 0x06000057 RID: 87 RVA: 0x00002263 File Offset: 0x00000463
			// (set) Token: 0x06000058 RID: 88 RVA: 0x0000226B File Offset: 0x0000046B
			[DataMember]
			public string version { get; set; }

			// Token: 0x1700001C RID: 28
			// (get) Token: 0x06000059 RID: 89 RVA: 0x00002274 File Offset: 0x00000474
			// (set) Token: 0x0600005A RID: 90 RVA: 0x0000227C File Offset: 0x0000047C
			[DataMember]
			public string customerPanelLink { get; set; }

			// Token: 0x1700001D RID: 29
			// (get) Token: 0x0600005B RID: 91 RVA: 0x00002285 File Offset: 0x00000485
			// (set) Token: 0x0600005C RID: 92 RVA: 0x0000228D File Offset: 0x0000048D
			[DataMember]
			public string downloadLink { get; set; }
		}

		// Token: 0x0200000A RID: 10
		public class app_data_class
		{
			// Token: 0x1700001E RID: 30
			// (get) Token: 0x0600005E RID: 94 RVA: 0x00002296 File Offset: 0x00000496
			// (set) Token: 0x0600005F RID: 95 RVA: 0x0000229E File Offset: 0x0000049E
			public string numUsers { get; set; }

			// Token: 0x1700001F RID: 31
			// (get) Token: 0x06000060 RID: 96 RVA: 0x000022A7 File Offset: 0x000004A7
			// (set) Token: 0x06000061 RID: 97 RVA: 0x000022AF File Offset: 0x000004AF
			public string numOnlineUsers { get; set; }

			// Token: 0x17000020 RID: 32
			// (get) Token: 0x06000062 RID: 98 RVA: 0x000022B8 File Offset: 0x000004B8
			// (set) Token: 0x06000063 RID: 99 RVA: 0x000022C0 File Offset: 0x000004C0
			public string numKeys { get; set; }

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x06000064 RID: 100 RVA: 0x000022C9 File Offset: 0x000004C9
			// (set) Token: 0x06000065 RID: 101 RVA: 0x000022D1 File Offset: 0x000004D1
			public string version { get; set; }

			// Token: 0x17000022 RID: 34
			// (get) Token: 0x06000066 RID: 102 RVA: 0x000022DA File Offset: 0x000004DA
			// (set) Token: 0x06000067 RID: 103 RVA: 0x000022E2 File Offset: 0x000004E2
			public string customerPanelLink { get; set; }

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x06000068 RID: 104 RVA: 0x000022EB File Offset: 0x000004EB
			// (set) Token: 0x06000069 RID: 105 RVA: 0x000022F3 File Offset: 0x000004F3
			public string downloadLink { get; set; }
		}

		// Token: 0x0200000B RID: 11
		public class user_data_class
		{
			// Token: 0x17000024 RID: 36
			// (get) Token: 0x0600006B RID: 107 RVA: 0x000022FC File Offset: 0x000004FC
			// (set) Token: 0x0600006C RID: 108 RVA: 0x00002304 File Offset: 0x00000504
			public string username { get; set; }

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x0600006D RID: 109 RVA: 0x0000230D File Offset: 0x0000050D
			// (set) Token: 0x0600006E RID: 110 RVA: 0x00002315 File Offset: 0x00000515
			public string ip { get; set; }

			// Token: 0x17000026 RID: 38
			// (get) Token: 0x0600006F RID: 111 RVA: 0x0000231E File Offset: 0x0000051E
			// (set) Token: 0x06000070 RID: 112 RVA: 0x00002326 File Offset: 0x00000526
			public string hwid { get; set; }

			// Token: 0x17000027 RID: 39
			// (get) Token: 0x06000071 RID: 113 RVA: 0x0000232F File Offset: 0x0000052F
			// (set) Token: 0x06000072 RID: 114 RVA: 0x00002337 File Offset: 0x00000537
			public string createdate { get; set; }

			// Token: 0x17000028 RID: 40
			// (get) Token: 0x06000073 RID: 115 RVA: 0x00002340 File Offset: 0x00000540
			// (set) Token: 0x06000074 RID: 116 RVA: 0x00002348 File Offset: 0x00000548
			public string lastlogin { get; set; }

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x06000075 RID: 117 RVA: 0x00002351 File Offset: 0x00000551
			// (set) Token: 0x06000076 RID: 118 RVA: 0x00002359 File Offset: 0x00000559
			public List<api.Data> subscriptions { get; set; }
		}

		// Token: 0x0200000C RID: 12
		public class Data
		{
			// Token: 0x1700002A RID: 42
			// (get) Token: 0x06000078 RID: 120 RVA: 0x00002362 File Offset: 0x00000562
			// (set) Token: 0x06000079 RID: 121 RVA: 0x0000236A File Offset: 0x0000056A
			public string subscription { get; set; }

			// Token: 0x1700002B RID: 43
			// (get) Token: 0x0600007A RID: 122 RVA: 0x00002373 File Offset: 0x00000573
			// (set) Token: 0x0600007B RID: 123 RVA: 0x0000237B File Offset: 0x0000057B
			public string expiry { get; set; }

			// Token: 0x1700002C RID: 44
			// (get) Token: 0x0600007C RID: 124 RVA: 0x00002384 File Offset: 0x00000584
			// (set) Token: 0x0600007D RID: 125 RVA: 0x0000238C File Offset: 0x0000058C
			public string timeleft { get; set; }
		}

		// Token: 0x0200000D RID: 13
		public class response_class
		{
			// Token: 0x1700002D RID: 45
			// (get) Token: 0x0600007F RID: 127 RVA: 0x00002395 File Offset: 0x00000595
			// (set) Token: 0x06000080 RID: 128 RVA: 0x0000239D File Offset: 0x0000059D
			public bool success { get; set; }

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x06000081 RID: 129 RVA: 0x000023A6 File Offset: 0x000005A6
			// (set) Token: 0x06000082 RID: 130 RVA: 0x000023AE File Offset: 0x000005AE
			public string message { get; set; }
		}
	}
}

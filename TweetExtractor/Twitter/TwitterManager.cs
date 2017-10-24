using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;

namespace TweetExtractor.Twitter {
	/// <summary>
	/// CoreTweetのマネージャー
	/// </summary>
	public class TwitterManager {

		/// <summary>
		/// Twitter API Key
		/// </summary>
		private const string API_KEY = "pbx7TIOAFvQSTmnNLurl9I4jX";

		/// <summary>
		/// Twitter API Secret
		/// </summary>
		private const string API_SECRET = "aINco9fPgZuh6yM2KG2d8pnMrZN9uPy5joGL6uNzaBI5TeHB9Z";

		private const string USERDATA_CONFIG = @"user.config";

		public Tokens Token { get; }


		public TwitterManager(string user) {
			if (!File.Exists(USERDATA_CONFIG)) {
				goto NewUser;
			}
			//ファイルのロード
			using (StreamReader reader = new StreamReader(USERDATA_CONFIG, Encoding.UTF8)) {
				Console.WriteLine("Loading UserFile.");
				string line;
				while ((line = reader.ReadLine()) != null) {
					int eqindex = line.IndexOf('=');
					string name = line.Substring(0, eqindex);
					if (name != user) {
						continue;
					}
					string keys = line.Substring(eqindex + 1);
					int commaindex = keys.IndexOf(',');
					this.Token = Tokens.Create(API_KEY, API_SECRET, keys.Substring(0, commaindex), keys.Substring(commaindex + 1));
					Console.WriteLine("Accept user {0}, KEY={1}, SECRET={2}", user, this.Token.AccessToken, this.Token.AccessTokenSecret);
					LogTweet("Loggin from Tweet Extractor.");
					return;
				}
			}
			NewUser:
			OAuth.OAuthSession session = OAuth.Authorize(API_KEY, API_SECRET);
			Console.WriteLine("NewUser Name: {0}", user);
			Console.WriteLine("Access here: {0}", session.AuthorizeUri);
			System.Diagnostics.Process.Start(session.AuthorizeUri.ToString());
			Console.Write("PinCode: ");
			this.Token = session.GetTokens(Console.ReadLine());
			LogTweet("Loggin from Tweet Extractor.");
			//ファイルへトークンの書き込み
			using (StreamWriter writer = new StreamWriter(USERDATA_CONFIG, true, Encoding.UTF8)) {
				writer.WriteLine(string.Format("{0}={1},{2}", user, this.Token.AccessToken, this.Token.AccessTokenSecret));
			}
		}

		public void LogTweet(string contents) => this.Token.Statuses.Update(DateTime.Now.ToString("[yyyy-MM-dd-HH:mm:ss]") + " Log\n" + contents);
	}
}

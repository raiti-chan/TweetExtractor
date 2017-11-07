using System;
using System.IO;
using System.Text;
using CoreTweet;

namespace TweetExtractor.Twitter {
	/// <summary>
	/// CoreTweetのマネージャー
	/// </summary>
	public class TwitterManager {
		/// <summary>
		/// Twitter API Key
		/// </summary>
		private const string ApiKey = "pbx7TIOAFvQSTmnNLurl9I4jX";

		/// <summary>
		/// Twitter API Secret
		/// </summary>
		private const string ApiSecret = "aINco9fPgZuh6yM2KG2d8pnMrZN9uPy5joGL6uNzaBI5TeHB9Z";
		
		/// <summary>
		/// ユーザーのトークン保管ファイル
		/// </summary>
		private const string UserdataConfig = @"user.config";
		
		/// <summary>
		/// ログをツイッターに送信する場合trueになるフラグ
		/// </summary>
		private readonly bool _isSentLogtoTwitter;
		
		public Tokens Token { get; }

		/// <summary>
		/// トークンを取得し、マネージャーを生成します。
		/// </summary>
		/// <param name="user">ログインするユーザー名、新規の場合認証モードになります。</param>
		/// <param name="isSentLogtoTwitter">Twitterにログを送る場合true</param>
		public TwitterManager(string user, bool isSentLogtoTwitter) {
			this._isSentLogtoTwitter = isSentLogtoTwitter;
			if (!File.Exists(UserdataConfig)) {
				goto NewUser;
			}
			//ファイルのロード
			using (StreamReader reader = new StreamReader(UserdataConfig, Encoding.UTF8)) {
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
					this.Token = Tokens.Create(ApiKey, ApiSecret, keys.Substring(0, commaindex), keys.Substring(commaindex + 1));
					Console.WriteLine("Accept user {0}", user);
					LogTweet("Loggin from Tweet Extractor.");
					return;
				}
			}
			NewUser:
			OAuth.OAuthSession session = OAuth.Authorize(ApiKey, ApiSecret);
			Console.WriteLine("NewUser Name: {0}", user);
			Console.WriteLine("Access here: {0}", session.AuthorizeUri);
			System.Diagnostics.Process.Start(session.AuthorizeUri.ToString());
			Console.Write("PinCode: ");
			this.Token = session.GetTokens(Console.ReadLine());
			LogTweet("Loggin from Tweet Extractor.");
			//ファイルへトークンの書き込み
			using (StreamWriter writer = new StreamWriter(UserdataConfig, true, Encoding.UTF8)) {
				writer.WriteLine($"{user}={this.Token.AccessToken},{this.Token.AccessTokenSecret}");
			}
		}

		/// <summary>
		/// Twitterにログを送信します。
		/// <see cref="_isSentLogtoTwitter"/>がfalseの場合送信されません。
		/// </summary>
		/// <param name="contents">送信するログの内容</param>
		public void LogTweet(string contents) {
			if (!this._isSentLogtoTwitter) return;
			this.Token.Statuses.Update($"{DateTime.Now:[yyyy-MM-dd-HH:mm:ss]} Log\n{contents}");
		}
		
		

		/// <summary>
		/// ツイートを送信します。
		/// </summary>
		/// <param name="contents">ツイートする内容。</param>
		public void SendTweet(string contents) {
			this.Token.Statuses.Update(contents);
			Console.WriteLine("send tweet:{0}", contents);
		}
	}
}

using System;
using System.IO;
using TweetExtractor.DataBase;
using TweetExtractor.ProgramAugment;
using TweetExtractor.Twitter;

namespace TweetExtractor { 
	/// <summary>
	/// Tweet Extractor メインクラス
	/// </summary>
	internal static class TweetExtractor {

		// ReSharper disable once MemberCanBePrivate.Global
		public static ProgramAugmentDictionary ProgramAugment;

		// ReSharper disable once MemberCanBePrivate.Global
		public static TwitterManager Twitter;
		
		/// <summary>
		/// メインメソッド
		/// </summary>
		/// <param name="args">プログラム引数</param>
		private static void Main(string[] args) {
			ProgramAugment = new ProgramAugmentDictionary(args);

			string user = ProgramAugment["user"];

			switch (user) {
				case null:
					Console.Error.WriteLine("Not found user augment.");
					Exit(1);
					break;
				case "true":
					Console.Error.WriteLine("name augment is flag.");
					Exit(0);
					break;
			}

			Twitter = new TwitterManager(user, ProgramAugment["tweetlog"] != null);
			
			if (ProgramAugment["tweet"] != null) {
				Twitter.SendTweet(ProgramAugment["tweet"]);
				Exit(0);
			}
			
			if (ProgramAugment["db"] != null) {
				if (ProgramAugment["db"] == "true") {
					Console.WriteLine("Please follow the db tag followed by the db path parameter.");
					Exit(1);
				}
				string logm = $"DataBase Output. Out:{ProgramAugment["db"]}";
				Console.WriteLine(logm);
				Twitter.LogTweet(logm);
				
				DataBaseMode(ProgramAugment["db"]);
				Exit(0);
			}
			
			Console.WriteLine("Text Output");
			Exit(0);
		}

		/// <summary>
		/// 取得したツイートをデーターベースに保管するモード
		/// </summary>
		private static void DataBaseMode(string dataBasePath) {
			if (ProgramAugment["add"] == null && File.Exists(dataBasePath)) {
				Console.WriteLine("File already exists. Do you want to overwrite?");
				Console.Write("Y/N:");
				if (Console.ReadKey().Key == ConsoleKey.N) {
					Exit(0);
				}
				File.Delete(dataBasePath);
			}
			
			TweetDataBase tdb = new TweetDataBase(dataBasePath);
		}

		private static void Exit(int exitCode) {
			ProgramAugment = null;
			Twitter = null;
			Environment.Exit(exitCode);
		}
	}
}

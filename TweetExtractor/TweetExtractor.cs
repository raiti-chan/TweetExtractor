using System;
using TweetExtractor.ProgramAugment;
using TweetExtractor.Twitter;

namespace TweetExtractor { 
	/// <summary>
	/// Tweet Extractor メインクラス
	/// </summary>
	internal class TweetExtractor {

		public static ProgramAugmentDictionary ProgramAugment;

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
					Environment.Exit(1);
					break;
				case "true":
					Console.Error.WriteLine("name augment is flag.");
					Environment.Exit(1);
					break;
			}

			Twitter = new TwitterManager(user);
		}
	}
}

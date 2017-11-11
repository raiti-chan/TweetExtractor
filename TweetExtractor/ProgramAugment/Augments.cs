using System;
using TweetExtractor.ProgramAugment.Attributes;

namespace TweetExtractor.ProgramAugment {
	/// <inheritdoc />
	/// <summary>
	/// プログラム引数
	/// </summary>
	public class Augments : AugmentsObjectBase{
		
		/// <summary>
		/// ツイートモードの場合true
		/// </summary>
		[FlagAugment("tweet", "ツイートモードにします。この場合ツイートするだけになります。")]
		// ReSharper disable once UnusedMember.Global
		// ReSharper disable once UnassignedGetOnlyAutoProperty
		public bool Tweet { get; }
		
		/// <summary>
		/// ログをツイートするか
		/// </summary>
		[FlagAugment("tweetlog", "ログをツイートします。")]
		// ReSharper disable once UnassignedGetOnlyAutoProperty
		// ReSharper disable once UnusedMember.Global
		public bool TweetLog { get; }

		/// <summary>
		/// ユーザー名
		/// </summary>
		[ParameterFlagAugment("user", "ログインするローカルユーザー名です。新規の場合ログインモードに入ります。")]
		// ReSharper disable once UnassignedGetOnlyAutoProperty
		// ReSharper disable once UnusedMember.Global
		public string User { get; }
		
		// ReSharper disable once UnusedMember.Global
		[FlagAugment]
		public readonly bool FTest = false;
		
		[ParameterFlagAugment]
		// ReSharper disable once UnusedMember.Global
		public readonly string FTest2 = null;
		
		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <param name="propertys">引数</param>
		public Augments(string[] propertys) : base(propertys) { }
		
		[FlagAugment]
		// ReSharper disable once UnusedMember.Global
		public void Test() {
			Console.WriteLine("Test");
		}

		[ParameterFlagAugment]
		// ReSharper disable once UnusedMember.Global
		public void Test2(string parameter) {
			Console.WriteLine("Tsst2:{0}",parameter);
		} 
	}
}

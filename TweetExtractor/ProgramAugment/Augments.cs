using TweetExtractor.ProgramAugment.Attributes;

namespace TweetExtractor.ProgramAugment {
	/// <inheritdoc />
	/// <summary>
	/// プログラム引数
	/// </summary>
	// ReSharper disable once ClassNeverInstantiated.Global
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
		[ParameterFlagAugment("user", "ログインするローカルユーザー名です。新規の場合ログインモードに入ります。", "ユーザー名")]
		// ReSharper disable once UnassignedGetOnlyAutoProperty
		// ReSharper disable once UnusedMember.Global
		public string User { get; }
		
		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <param name="propertys">引数</param>
		public Augments(string[] propertys) : base(propertys) { }
	}
}

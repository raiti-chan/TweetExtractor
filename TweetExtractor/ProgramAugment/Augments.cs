namespace TweetExtractor.ProgramAugment {
	/// <summary>
	/// プログラム引数
	/// </summary>
	public class Augments : AugmentsObjectBase{
		
		/// <summary>
		/// ツイートモードの場合true
		/// </summary>
		[FlagAugment("tweet")]
		public bool Tweet { get; }

		/// <summary>
		/// ユーザーの
		/// </summary>
		[PropertyFlagAugment("user")]
		public string User { get; }

		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <param name="propertys">引数</param>
		public Augments(string[] propertys) : base(propertys) { }
	}
}

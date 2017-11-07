namespace TweetExtractor.ProgramAugment {
	/// <summary>
	/// 引数オブジェクトの基礎クラス
	/// </summary>
	// ReSharper disable once UnusedMember.Global
	public class AugmentsObjectBase {
		
		/// <summary>
		/// 引数の格納先
		/// </summary>
		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		public string[] Propertys { get; }
		
		/// <summary>
		/// 引数オブジェクトを初期化します。
		/// </summary>
		/// <param name="propertys">引数</param>
		// ReSharper disable once MemberCanBeProtected.Global
		public AugmentsObjectBase(string[] propertys) {
			this.Propertys = propertys;
		}


	}
}

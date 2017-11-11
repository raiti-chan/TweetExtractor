namespace TweetExtractor.ProgramAugment.Attributes {
	/// <inheritdoc />
	/// <summary>
	/// パラメータを持ったフラグのプロパティを格納するプロパティ、フィールドおよびパラメータを引数として呼び出されるメソッドにつける属性
	/// </summary>
	public class ParameterFlagAugmentAttribute : FlagAugmentAttribute {
		
		/// <summary>
		/// パラメータの説明メッセージ
		/// </summary>
		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		// ReSharper disable once MemberCanBePrivate.Global
		public string ParameterMessage { get; }

		/// <inheritdoc />
		/// <summary>
		/// プロパティ、フィールド、メソッドをパラメータ付きフラグとして定義します。
		/// メソッドの場合string引数のみのメソッドです。
		/// プロパティおよびフィールドの型はstringです。
		/// </summary>
		/// <param name="flagName">フラグ名、nullにするとメンバ名と同じになります。</param>
		/// <param name="helpMessage">ヘルプで表示するメッセージです。</param>
		/// <param name="parameterMessage">パラメータの説明</param>
		public ParameterFlagAugmentAttribute(string flagName = null, string helpMessage = "", string parameterMessage = "") : base(flagName, helpMessage) {
			this.ParameterMessage = parameterMessage;
		}
	}
}

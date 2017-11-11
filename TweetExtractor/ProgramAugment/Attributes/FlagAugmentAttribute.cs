using System;

namespace TweetExtractor.ProgramAugment.Attributes {
	/// <inheritdoc />
	/// <summary>
	/// フラグの有無を記録するプロパティ、フィールド、およびフラグが存在した場合に実行されるメソッドにつける属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method)]
	public class FlagAugmentAttribute : Attribute {
		
		/// <summary>
		/// フラグ名
		/// </summary>
		public string FlagName { get; }
		
		/// <summary>
		/// ヘルプで表示するメッセージ
		/// </summary>
		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		public string HelpMessage { get; }
		
		/// <inheritdoc />
		/// <summary>
		/// プロパティ、フィールド、メソッドをフラグ定義とします。
		/// メソッドの場合引数無しの戻り値がvoidです。
		/// プロパティおよびフィールドの型はboolです。
		/// </summary>
		/// <param name="flagName">フラグ名、nullにするとメンバ名と同じになります。</param>
		/// <param name="helpMessage">ヘルプで表示するメッセージです。</param>
		// ReSharper disable once MemberCanBeProtected.Global
		public FlagAugmentAttribute(string flagName = null, string helpMessage = "") {
			this.FlagName = flagName;
			this.HelpMessage = helpMessage;
		}
		
	}
}

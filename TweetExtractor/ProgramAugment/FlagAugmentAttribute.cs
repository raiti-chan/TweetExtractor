using System;

namespace TweetExtractor.ProgramAugment {
	/// <inheritdoc />
	/// <summary>
	/// フラグの有無を記録するプロパティ、フィールド、およびフラグが存在した場合に実行されるメソッドにつける属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method)]
	public class FlagAugmentAttribute : Attribute {
		
		/// <summary>
		/// フラグ名
		/// </summary>
		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		public string FlagName { get; }
		
		/// <inheritdoc />
		/// <summary>
		/// プロパティ、フィールド、メソッドをフラグ定義とします。
		/// メソッドの場合引数無しの戻り値がvoidです。
		/// プロパティおよびフィールドの型はboolです。
		/// フラグ名はプロパティ、フィールド、メソッド名と一緒になります。
		/// </summary>
		// ReSharper disable once MemberCanBeProtected.Global
		public FlagAugmentAttribute() {
			this.FlagName = null;
		}
		
		/// <inheritdoc />
		/// <summary>
		/// プロパティ、フィールド、メソッドをフラグ定義とします。
		/// メソッドの場合引数無しの戻り値がvoidです。
		/// プロパティおよびフィールドの型はboolです。
		/// </summary>
		/// <param name="flagName">フラグ名</param>
		public FlagAugmentAttribute(string flagName) {
			this.FlagName = flagName;
		}
		
	}
}

namespace TweetExtractor.ProgramAugment {
	/// <inheritdoc />
	/// <summary>
	/// プロパティを持ったフラグのプロパティを格納するプロパティ、フィールドおよびプロパティを引数として呼び出されるメソッドにつける属性
	/// </summary>
	public class PropertyFlagAugmentAttribute : FlagAugmentAttribute {

		/// <inheritdoc />
		/// <summary>
		/// プロパティ、フィールド、メソッドをプロパティ付きフラグ定義とします。
		/// メソッドの場合string引数のみのメソッドです。
		/// プロパティおよびフィールドの型はstringです。
		/// フラグ名はプロパティ、フィールド、メソッド名と一緒になります。
		/// </summary>
		// ReSharper disable once UnusedMember.Global
		public PropertyFlagAugmentAttribute() {
			
		}
		
		/// <inheritdoc />
		/// <summary>
		/// プロパティ、フィールド、メソッドをプロパティ付きフラグ定義とします。
		/// メソッドの場合string引数のみのメソッドです。
		/// プロパティおよびフィールドの型はstringです。
		/// </summary>
		/// <param name="name">フラグ名</param>
		public PropertyFlagAugmentAttribute(string name) : base(name) {
			
		}
	}
}

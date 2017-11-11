using System;
using System.Reflection;

namespace TweetExtractor.ProgramAugment.Exceptions {
	/// <summary>
	/// フラグの定義が重複した場合に発生する例外クラス
	/// </summary>
	public class DuplicateFlagDefinitionsException : Exception {
		
		/// <summary>
		/// 重複したフラグの名前
		/// </summary>
		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		public string FlagName { get; }
		
		/// <summary>
		/// 定義しようとしたフラグのメンバー
		/// </summary>
		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		// ReSharper disable once MemberCanBePrivate.Global
		public MemberInfo MemberInfo { get; }
		
		/// <summary>
		/// 既に定義されていたフラグのメンバー
		/// </summary>
		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		public MemberInfo AlreadyMemberInfo { get; }
		
		/// <summary>
		/// フラグ名、各メンバを設定し例外を生成します。
		/// </summary>
		/// <param name="flagName">重複したフラグ名</param>
		/// <param name="memberInfo">新たに定義しようとしたフラグのメンバ</param>
		/// <param name="alreadyMemberInfo">既に定義されていたフラグのメンバ</param>
		public DuplicateFlagDefinitionsException(string flagName, MemberInfo memberInfo, MemberInfo alreadyMemberInfo) : base(
			$"既に定義されている名前で新たなフラグを宣言しようとしました。 new={memberInfo.Name} already={alreadyMemberInfo.Name}"
			) {
			this.FlagName = flagName;
			this.MemberInfo = memberInfo;
			this.AlreadyMemberInfo = alreadyMemberInfo;
		}


		/// <summary>
		/// フラグ名、各メンバ、例外メッセージを設定し例外を生成します。
		/// </summary>
		/// <param name="flagName">重複したフラグ名</param>
		/// <param name="memberInfo">新たに定義しようとしたフラグのメンバ</param>
		/// <param name="alreadyMemberInfo">既に定義されていたフラグのメンバ</param>
		/// <param name="message">例外メッセージ</param>
		// ReSharper disable once UnusedMember.Global
		public DuplicateFlagDefinitionsException(string flagName, MemberInfo memberInfo, MemberInfo alreadyMemberInfo, string message) : base(message) {
			this.FlagName = flagName;
			this.MemberInfo = memberInfo;
			this.AlreadyMemberInfo = alreadyMemberInfo;
		}
		
	}
}

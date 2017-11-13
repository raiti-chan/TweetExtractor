using System;

namespace TweetExtractor.ProgramAugment.Exceptions {
	/// <inheritdoc />
	/// <summary>
	/// タイプが不正の場合にスローされる例外クラス
	/// </summary>
	public class WrongTypeOfFlagElementException : Exception {
		
		/// <summary>
		/// 間違った型
		/// </summary>
		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		public Type WrongType { get; }

		/// <summary>
		/// 正しい型
		/// </summary>
		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		public Type RightType { get; }

		/// <inheritdoc />
		/// <summary>
		/// 間違った型と正しい型を指定して例外を生成します。
		/// </summary>
		/// <param name="wrongType">間違った型</param>
		/// <param name="rightType">正しい型</param>
		public WrongTypeOfFlagElementException(Type wrongType, Type rightType) {
			this.WrongType = wrongType;
			this.RightType = rightType;
		}
		
		/// <inheritdoc />
		/// <summary>
		/// 間違った型と正しい型とメッセージを指定して例外を生成します。
		/// </summary>
		/// <param name="wrongType">間違った型</param>
		/// <param name="rightType">正しい型</param>
		/// <param name="message">メッセージ</param>
		public WrongTypeOfFlagElementException(Type wrongType, Type rightType, string message) : base(message) {
			this.WrongType = wrongType;
			this.RightType = rightType;
		}
	}
}

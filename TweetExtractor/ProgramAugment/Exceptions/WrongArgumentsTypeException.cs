using System;

namespace TweetExtractor.ProgramAugment.Exceptions {
	public class WrongArgumentsTypeException : Exception {
		private readonly Type[] _wrongArgumentsTypes;

		/// <summary>
		/// 間違った型
		/// </summary>
		// ReSharper disable once ConvertToAutoProperty
		// ReSharper disable once UnusedMember.Global
		public Type[] WrongArgumentsTypes => this._wrongArgumentsTypes;

		private readonly Type[] _rightArgumentsTypes;

		/// <summary>
		/// 正しい型
		/// </summary>
		// ReSharper disable once ConvertToAutoProperty
		// ReSharper disable once UnusedMember.Global
		public Type[] RightType => this._rightArgumentsTypes;

		/// <inheritdoc />
		/// <summary>
		/// 間違った型と正しい型を指定して例外を生成します。
		/// </summary>
		/// <param name="wrongArgumentsTypes">間違った型</param>
		/// <param name="rightType">正しい型</param>
		public WrongArgumentsTypeException(Type[] wrongArgumentsTypes, Type[] rightType) {
			this._wrongArgumentsTypes = wrongArgumentsTypes;
			this._rightArgumentsTypes = rightType;
		}

		/// <inheritdoc />
		/// <summary>
		/// 間違った型と正しい型とメッセージを指定して例外を生成します。
		/// </summary>
		/// <param name="wrongArgumentsTypes">間違った型</param>
		/// <param name="rightType">正しい型</param>
		/// <param name="message">メッセージ</param>
		public WrongArgumentsTypeException(Type[] wrongArgumentsTypes, Type[] rightType, string message) : base(message) {
			this._wrongArgumentsTypes = wrongArgumentsTypes;
			this._rightArgumentsTypes = rightType;
		}
	}
}
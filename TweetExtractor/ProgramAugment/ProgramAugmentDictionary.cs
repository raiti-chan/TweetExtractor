using System.Collections.Generic;

namespace TweetExtractor.ProgramAugment {
	/// <summary>
	/// プログラム引数のディクショナリ
	/// </summary>
	public class ProgramAugmentDictionary {

		/// <summary>
		/// 格納予定のタグのフラグ
		/// </summary>
		private string _oldTag;

		/// <summary>
		/// タグと値が対になってるデータ格納場所
		/// </summary>
		private Dictionary<string, string> _dictionary = new Dictionary<string, string>();

		/// <summary>
		/// パラメーターの格納場所
		/// </summary>
		private List<string> _parameterList = new List<string>(2);

		/// <summary>
		/// 指定されたタグの値を取得します。
		/// タグが存在しなければnullが返ります、
		/// またタグが宣言されただけの場合"true"文字列が返されます。
		/// </summary>
		/// <param name="tag">タグ名</param>
		/// <returns>タグに格納されたデータ</returns>
		public string this[string tag] {
			get {
				bool result = this._dictionary.TryGetValue(tag, out string retstr);
				return retstr;
			}
		}

		/// <summary>
		/// 指定されたインデックスのパラメーターを取得します。
		/// </summary>
		/// <param name="index">パラメーターのインデックス</param>
		/// <returns>パラメーター</returns>
		public string this[int index] => this._parameterList[index];

		/// <summary>
		/// パラメーターの要素数。
		/// </summary>
		public int ParameterCount => this._parameterList.Count;

		

		/// <summary>
		/// プログラム引数を渡しディクショナリを生成します。
		/// </summary>
		/// <param name="args">プログラム引数</param>
		public ProgramAugmentDictionary(string[] args) {
			foreach (string arg in args) {
				if (arg[0] == '-') {
					if (this._oldTag != null) {
						this._dictionary.Add(this._oldTag, "true");
					}
					this._oldTag = arg.Substring(1);
				} else {
					if (this._oldTag != null) {
						this._dictionary.Add(this._oldTag, arg);
						this._oldTag = null;
					} else {
						this._parameterList.Add(arg);
					}
				}
			}

			if (this._oldTag != null) {
				this._dictionary.Add(this._oldTag, "true");
			}

		}



	}
}

using System.Collections.Generic;

namespace TweetExtractor.ProgramAugment {
	/// <summary>
	/// 使用される拡張メソッド
	/// </summary>
	public static class ExtensionMethods {
		/// <summary>
		/// 指定されたキーの値を取得します。
		/// キーが存在しない場合参照型ならnull、それ以外はデフォルトの値が返されます。
		/// </summary>
		/// <param name="dictionary">参照する<see cref="IReadOnlyDictionary{TKey,TValue}"/></param>
		/// <param name="key">取得する値のキー</param>
		/// <typeparam name="TKey">キーのタイプ</typeparam>
		/// <typeparam name="TSource">値のタイプ</typeparam>
		/// <returns></returns>
		public static TSource GetValue<TKey, TSource>(this IReadOnlyDictionary<TKey, TSource> dictionary, TKey key) {
			dictionary.TryGetValue(key, out var tSource);
			return tSource;
		}
		
	}
}

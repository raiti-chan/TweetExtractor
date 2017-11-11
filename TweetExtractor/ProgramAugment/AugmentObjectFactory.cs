using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TweetExtractor.ProgramAugment.Attributes;
using TweetExtractor.ProgramAugment.Exceptions;

namespace TweetExtractor.ProgramAugment {
	/// <summary>
	/// 引数オブジェクトを生成するFactoryです
	/// </summary>
	// ReSharper disable once UnusedMember.Global
	public class AugmentObjectFactory<T> where T : AugmentsObjectBase {
		/// <summary>
		/// 引数オブジェクトのクラス
		/// </summary>
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly Type _augmentsObjectType;

		/// <summary>
		/// 定義されたフラグのセット
		/// </summary>
		private readonly HashSet<string> _flagSet = new HashSet<string>();

		/// <summary>
		/// フラグ名とフラグメンバーオブジェクトのディクショナリ
		/// </summary>
		private readonly Dictionary<string, _MemberObj<FlagAugmentAttribute>> _flagDictionary = new Dictionary<string, _MemberObj<FlagAugmentAttribute>>();

		/// <summary>
		/// フラグ名とパラメータ付きフラグメンバーオブジェクトのディクショナリ
		/// </summary>
		private readonly Dictionary<string, _MemberObj<ParameterFlagAugmentAttribute>> _parameterFlagDictionary =
			new Dictionary<string, _MemberObj<ParameterFlagAugmentAttribute>>();

		/// <summary>
		/// プロパティのフィールドのディクショナリ
		/// </summary>
		private readonly Dictionary<PropertyInfo, FieldInfo> _propertysFieldDictionary = new Dictionary<PropertyInfo, FieldInfo>();


		/// <summary>
		/// 指定されたタイプの引数オブイジェクトのファクトリを生成します。
		/// </summary>
		/// <param name="type">生成する引数オブジェクトクラス</param>
		public AugmentObjectFactory() {
			this._augmentsObjectType = typeof(T);
			//プロパティのバッキングフィールドをプロパティの名でディクショナリ化
			Dictionary<string, MemberInfo> privateFieldInfos = this._augmentsObjectType
				.GetMembers(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
				.Where(info => info.MemberType == MemberTypes.Field && info.Name.IndexOf('<') == 0).ToDictionary(_GetNameOfPropertysField);

			foreach (var info in this._augmentsObjectType.GetMembers(
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)) {
				//メンバのアノテーションから、フラグアノテーションだけを抽出し、IEnumerableとして取得
				IEnumerable<FlagAugmentAttribute> enumerable = Attribute.GetCustomAttributes(info).OfType<FlagAugmentAttribute>();
				//配列に変換
				FlagAugmentAttribute[] flagAugmentAttributes = enumerable as FlagAugmentAttribute[] ?? enumerable.ToArray();
				//要素が空の場合フラグアノテーションが付いてないのでスキップ
				if (!flagAugmentAttributes.Any()) continue;
				//本来要素は必ず一つになるはずなのでその要素単体を取得する
				FlagAugmentAttribute attribute = flagAugmentAttributes.First();
				//アノテーションの名前要素がnullじゃない場合その名前を、nullの場合メソッド名をフラグ名とする
				string flagName = attribute.FlagName ?? info.Name;
				//フラグのセットに新たなフラグの追加を試みる
				bool addResult = this._flagSet.Add(flagName);
				//追加できなかった場合既にフラグが存在するので例外を投げる	
				if (!addResult)
					throw new DuplicateFlagDefinitionsException(flagName, info,
						this._flagDictionary.GetValue(flagName)?.Member ?? this._parameterFlagDictionary.GetValue(flagName).Member);
				bool hasParameter = false;
				if (attribute is ParameterFlagAugmentAttribute augmentAttribute) {
					//パラメータ持ちのフラグの場合パラメータ付きのフラグディクショナリに登録
					this._parameterFlagDictionary.Add(flagName, new _MemberObj<ParameterFlagAugmentAttribute>(info, augmentAttribute));
					hasParameter = true;
				} else {
					//普通のフラグの場合通常のフラグに登録
					this._flagDictionary.Add(flagName, new _MemberObj<FlagAugmentAttribute>(info, attribute));
				}

				switch (info.MemberType) {
					case MemberTypes.Field: {
						//フィールドフラグの型チェック
						FieldInfo fieldInfo = (FieldInfo) info;
						if (hasParameter) {
							if (fieldInfo.FieldType != typeof(string)) throw new Exception("フィールドの型が違います。string");
						} else {
							if (fieldInfo.FieldType != typeof(bool)) throw new Exception("フィールドの型が違います。bool");
						}
					}
						break;
					case MemberTypes.Method: {
						//メソッドフラグの型チェック
						MethodInfo methodInfo = (MethodInfo) info;
						ParameterInfo[] parameterInfos = methodInfo.GetParameters();
						if (hasParameter) {
							if (parameterInfos.Length != 1) throw new Exception("パラメータ付きフラグメソッドは引数がstringのみのメソッドでなくてはいけません。");
							if (parameterInfos[0].ParameterType != typeof(string)) throw new Exception("パラメータ付きフラグメソッドは引数がstringのみのメソッドでなくてはいけません。");
						} else {
							if (parameterInfos.Length != 0) throw new Exception("フラグメソッドは引数が無しのメソッドでなくてはいけません。");
						}
					}
						break;
					case MemberTypes.Property:
						//プロパティフラグの型チェック
						PropertyInfo propertyInfo = (PropertyInfo) info;
						if (hasParameter) {
							if (propertyInfo.PropertyType != typeof(string)) throw new Exception("プロパティの型が違います。string");
						} else {
							if (propertyInfo.PropertyType != typeof(bool)) throw new Exception("プロパティの型が違います。bool");
						}
						//プロパティとプロパティのバッキングフィールドを関連付けます
						this._propertysFieldDictionary.Add(propertyInfo, (FieldInfo) privateFieldInfos[propertyInfo.Name]);
						break;
				}
			}
		}

		public T GeneratAugmentObject(string [] args, params object[] customConstructorArgs) {
			IEnumerable<object> invokeArgs = new object[]{args}.Concat(customConstructorArgs);
			T tObj = (T)Activator.CreateInstance(this._augmentsObjectType, invokeArgs);
			return tObj;
		}

		private static string _GetNameOfPropertysField(MemberInfo fieldInfo) {
			string fieldName = fieldInfo.Name;
			int endIndex = fieldName.IndexOf('>');
			return fieldName.Substring(1, endIndex - 1);
		}

		/// <summary>
		/// ディクショナリに格納する<see cref="MemberInfo"/>とそのメンバにつけられた<see cref="FlagAugmentAttribute"/>を保持するクラス
		/// </summary>
		/// <typeparam name="T">格納するアノテーションの型。<see cref="FlagAugmentAttribute"/>をベースとしたアノテーションクラス</typeparam>
		// ReSharper disable once InconsistentNaming
		private class _MemberObj<T> where T : FlagAugmentAttribute {
			/// <summary>
			/// メンバ
			/// </summary>
			public MemberInfo Member { get; }

			/// <summary>
			/// アノテーション
			/// </summary>
			// ReSharper disable once UnusedAutoPropertyAccessor.Local
			// ReSharper disable once MemberCanBePrivate.Local
			public T Attribute { get; }

			/// <summary>
			/// メンバとフラグアノテーションからメンバオブジェクトを生成します。
			/// </summary>
			/// <param name="member">メンバ</param>
			/// <param name="attribute">フラグアノテーション</param>
			public _MemberObj(MemberInfo member, T attribute) {
				this.Member = member;
				this.Attribute = attribute;
			}
		}
	}
}

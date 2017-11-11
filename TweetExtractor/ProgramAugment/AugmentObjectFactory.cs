using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
		public AugmentObjectFactory() {
			this._augmentsObjectType = typeof(T);
			//プロパティのバッキングフィールドをプロパティの名でディクショナリ化
			Dictionary<string, MemberInfo> privateFieldInfos =
			(from info in this._augmentsObjectType.GetMembers(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
				where info.MemberType == MemberTypes.Field && info.Name.IndexOf('<') == 0
				select info).ToDictionary(_GetNameOfPropertysField);

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
						break;
					}
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
						break;
					}
					case MemberTypes.Property: {
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
		}

		/// <summary>
		/// 引数オブジェクトを生成します。
		/// </summary>
		/// <param name="args">プログラム引数</param>
		/// <param name="customConstructorArgs">引数オブジェクトのコンストラクタのstring配列以降の引数</param>
		/// <returns>生成された引数オブジェクト</returns>
		public T GeneratAugmentObject(string[] args, params object[] customConstructorArgs) {
			List<string> arguments = new List<string>();
			List<_IReflectFunc> reflectFuncs = new List<_IReflectFunc>();
			for (int i = 0; i < args.Length; i++) {
				var str = args[i];
				if (str[0] == '-') {
					string flag = str.Substring(1);
					if (!this._flagSet.Contains(flag)) throw new Exception($"フラグが不正です。 {str}");
					if (this._flagDictionary.ContainsKey(flag)) {
						//通常フラグの場合
						_MemberObj<FlagAugmentAttribute> memberObj = this._flagDictionary[flag];
						MemberInfo memberInfo = memberObj.Member;
						switch (memberInfo.MemberType) {
							case MemberTypes.Field: {
								reflectFuncs.Add(new _FieldReflect((FieldInfo) memberInfo));
								break;
							}
							case MemberTypes.Method: {
								reflectFuncs.Add(new _MethodReflect((MethodInfo) memberInfo));
								break;
							}
							case MemberTypes.Property: {
								PropertyInfo propertyInfo = (PropertyInfo) memberInfo;
								reflectFuncs.Add(new _PropertyReflect(propertyInfo, _propertysFieldDictionary.GetValue(propertyInfo)));
								break;
							}
						}
					} else {
						//パラメータ付きフラグの場合
						_MemberObj<ParameterFlagAugmentAttribute> memberObj = this._parameterFlagDictionary[flag];
						i++;
						string parameter = args[i];
						MemberInfo memberInfo = memberObj.Member;
						switch (memberInfo.MemberType) {
							case MemberTypes.Field: {
								reflectFuncs.Add(new _PFieldReflect((FieldInfo) memberInfo, parameter));
								break;
							}
							case MemberTypes.Method: {
								reflectFuncs.Add(new _PMethodReflect((MethodInfo) memberInfo, parameter));
								break;
							}
							case MemberTypes.Property: {
								PropertyInfo propertyInfo = (PropertyInfo) memberInfo;
								reflectFuncs.Add(new _PPropertyReflect(propertyInfo, _propertysFieldDictionary.GetValue(propertyInfo), parameter));
								break;
							}
						}
					}
					continue;
				}
				arguments.Add(str);
			}


			IEnumerable<object> invokeArgs = new object[] {arguments.ToArray()}.Concat(customConstructorArgs);
			T tObj = (T) Activator.CreateInstance(this._augmentsObjectType, invokeArgs.ToArray());

			reflectFuncs.ForEach(reflectFunc => reflectFunc.Reflect(tObj));

			return tObj;
		}

		/// <summary>
		/// ヘルプで表示するテキストを取得します。
		/// </summary>
		/// <returns></returns>
		public string GetHelpText() {
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("Paramater");
			foreach (var flag in this._flagSet) {
				builder.Append("  ");
				if (this._parameterFlagDictionary.ContainsKey(flag)) {
					_MemberObj<ParameterFlagAugmentAttribute> memberObj = this._parameterFlagDictionary[flag];
					builder.AppendLine($"-{flag}:{memberObj.Attribute.ParameterMessage}  {memberObj.Attribute.HelpMessage}");
				} else {
					_MemberObj<FlagAugmentAttribute> memberObj = this._flagDictionary[flag];
					builder.AppendLine($"-{flag}  {memberObj.Attribute.HelpMessage}");
				}
			}

			return builder.ToString();
		}

		private static string _GetNameOfPropertysField(MemberInfo fieldInfo) {
			string fieldName = fieldInfo.Name;
			int endIndex = fieldName.IndexOf('>');
			return fieldName.Substring(1, endIndex - 1);
		}

		// ReSharper disable once InconsistentNaming
		private class _MemberObj<TAttribute> where TAttribute : FlagAugmentAttribute {
			public MemberInfo Member { get; }


			// ReSharper disable once MemberCanBePrivate.Local
			// ReSharper disable once UnusedAutoPropertyAccessor.Local
			public TAttribute Attribute { get; }

			public _MemberObj(MemberInfo member, TAttribute attribute) {
				this.Member = member;
				this.Attribute = attribute;
			}
		}

		// ReSharper disable once InconsistentNaming
		private interface _IReflectFunc {
			void Reflect(object o);
		}

		// ReSharper disable once InconsistentNaming
		private class _FieldReflect : _IReflectFunc {
			// ReSharper disable once InconsistentNaming
			protected readonly FieldInfo _fieldInfo;

			public _FieldReflect(FieldInfo fieldInfo) {
				this._fieldInfo = fieldInfo;
			}

			public virtual void Reflect(object o) {
				this._fieldInfo.SetValue(o, true);
			}
		}

		// ReSharper disable once InconsistentNaming
		private class _PFieldReflect : _FieldReflect {
			private readonly string _paramater;

			public _PFieldReflect(FieldInfo fieldInfo, string paramater) : base(fieldInfo) {
				this._paramater = paramater;
			}

			public override void Reflect(object o) {
				this._fieldInfo.SetValue(o, this._paramater);
			}
		}

		// ReSharper disable once InconsistentNaming
		private class _MethodReflect : _IReflectFunc {
			// ReSharper disable once InconsistentNaming
			protected readonly MethodInfo _methodInfo;

			public _MethodReflect(MethodInfo methodInfo) {
				this._methodInfo = methodInfo;
			}

			public virtual void Reflect(object o) {
				this._methodInfo.Invoke(o, null);
			}
		}

		// ReSharper disable once InconsistentNaming
		private class _PMethodReflect : _MethodReflect {
			private readonly string _paramater;

			public _PMethodReflect(MethodInfo methodInfo, string paramater) : base(methodInfo) {
				this._paramater = paramater;
			}

			public override void Reflect(object o) {
				this._methodInfo.Invoke(o, new object[] {_paramater});
			}
		}

		// ReSharper disable once InconsistentNaming
		private class _PropertyReflect : _IReflectFunc {
			// ReSharper disable once InconsistentNaming
			protected readonly PropertyInfo _propertyInfo;

			// ReSharper disable once InconsistentNaming
			protected readonly FieldInfo _backkingFieldInfo;

			// ReSharper disable once InconsistentNaming
			public _PropertyReflect(PropertyInfo propertyInfo, FieldInfo _backkingFieldInfo) {
				this._propertyInfo = propertyInfo;
				this._backkingFieldInfo = _backkingFieldInfo;
			}

			public virtual void Reflect(object o) {
				if (this._propertyInfo.SetMethod == null) {
					this._backkingFieldInfo.SetValue(o, true);
				} else {
					this._propertyInfo.SetValue(o, true);
				}
			}
		}

		// ReSharper disable once InconsistentNaming
		private class _PPropertyReflect : _PropertyReflect {
			private readonly string _paramater;

			public _PPropertyReflect(PropertyInfo propertyInfo, FieldInfo backkingFieldInfo, string paramater) : base(propertyInfo, backkingFieldInfo) {
				this._paramater = paramater;
			}

			public override void Reflect(object o) {
				if (this._propertyInfo.SetMethod == null) {
					this._backkingFieldInfo.SetValue(o, this._paramater);
				} else {
					this._propertyInfo.SetValue(o, this._paramater);
				}
			}
		}
	}
}

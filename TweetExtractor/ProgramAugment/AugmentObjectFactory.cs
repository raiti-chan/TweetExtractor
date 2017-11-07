using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TweetExtractor.ProgramAugment {
	/// <summary>
	/// 引数オブジェクトを生成するFactoryです
	/// </summary>
	// ReSharper disable once UnusedMember.Global
	public class AugmentObjectFactory {

		/// <summary>
		/// 引数オブジェクトのクラス
		/// </summary>
		// ReSharper disable once NotAccessedField.Local
		private readonly Type _augmentsObjectType;

		// ReSharper disable once UnusedMember.Local
		private readonly HashSet<string> _flagSet = new HashSet<string>();
		
		// ReSharper disable once UnusedMember.Local
		private readonly Dictionary<string, MemberInfo> _flagDictionary = new Dictionary<string, MemberInfo>();
		
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		public AugmentObjectFactory(Type type) {
			this._augmentsObjectType = type;
			foreach (var info in this._augmentsObjectType.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)) {
				IEnumerable<FlagAugmentAttribute> enumerable = Attribute.GetCustomAttributes(info).OfType<FlagAugmentAttribute>();
				FlagAugmentAttribute[] flagAugmentAttributes = enumerable as FlagAugmentAttribute[] ?? enumerable.ToArray();
				if (!flagAugmentAttributes.Any()) continue;
				string flagName = flagAugmentAttributes.First().FlagName ?? info.Name;
				bool addResult = this._flagSet.Add(flagName);
				//TODO:同じ名前のフラグ宣言が重複してる例外を独自のものにする。
				if (!addResult) throw new Exception($"既に同じ名前のフラグが宣言されています:{flagName}={_flagDictionary[flagName].Name}");
			}
			
		}
		
		
	}
}

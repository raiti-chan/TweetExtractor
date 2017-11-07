using System;
using System.Data.SQLite;

namespace TweetExtractor.DataBase {
	/// <inheritdoc />
	/// <summary>
	/// ツイートを格納するデーターベース
	/// </summary>
	public class TweetDataBase : IDisposable {
		/// <summary>
		/// DBのファイルパス
		/// </summary>
		private readonly SQLiteConnection _dataBase;

		public TweetDataBase(string dataBaseFilePath) {
			this._dataBase = new SQLiteConnection(dataBaseFilePath);
			this._dataBase.Open();
			
			using (var cmd = this._dataBase.CreateCommand()) {
				cmd.CommandText = "SELECT * FROM sqlite_master WHERE type='table'";
				using (SQLiteDataReader reader = cmd.ExecuteReader()) {
					//reader["tbl_name"];
				}
			}
			
		}
		
		

		
		public void Dispose() {
			this._dataBase?.Dispose();
		}
	}
}

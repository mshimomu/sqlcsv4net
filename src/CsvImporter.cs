using Microsoft.VisualBasic.FileIO;
using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace sqlcsv4net.src
{
	/// <summary>
	/// csvインポータ
	/// </summary>
	static class CsvImporter
	{
		/// <summary>
		/// csvファイルと同じテーブル名、カラム名のテーブルを作成.
		/// csvのデータをテーブルにインポート.
		/// </summary>
		/// <param name="file">file name</param>
		/// <param name="conn">SQLite connection</param>
		public static void ImportCsvToTable(String file, SQLiteConnection conn)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			FileInfo f = new FileInfo(file);
			String tableName = f.Name.Replace(".csv", "");

			TextFieldParser parser = new TextFieldParser(f.Name, System.Text.Encoding.GetEncoding("Shift_JIS"));
			parser.TextFieldType = FieldType.Delimited;
			parser.SetDelimiters(",");
			String[] columns = parser.ReadFields();

			createTable(conn, tableName, columns);
			insertData(conn, parser, tableName, columns);

			sw.Stop();
			Console.WriteLine(createCreateTableSQL(tableName, columns) + " and import completed " + sw.Elapsed.TotalSeconds + "sec");
		}

		/// <summary>
		/// テーブル作成
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="tableName"></param>
		/// <param name="columns"></param>
		private static void createTable(SQLiteConnection conn, String tableName, String[] columns)
		{
			SQLiteCommand command = conn.CreateCommand();
			command.CommandText = createCreateTableSQL(tableName, columns);
			command.ExecuteNonQuery();
		}

		/// <summary>
		/// データインサート
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="parser"></param>
		/// <param name="tableName"></param>
		/// <param name="columns"></param>
		private static void insertData(SQLiteConnection conn, TextFieldParser parser, String tableName, String[] columns)
		{
			SQLiteCommand command = conn.CreateCommand();
			command.CommandText = createInsertPsmt(tableName, columns.Length);

			SQLiteTransaction tran = conn.BeginTransaction();
			// カラム情報は読んでいるのでそのまま続けてデータ部分を読む
			while (!parser.EndOfData)
			{
				String[] row = parser.ReadFields();

				command.Parameters.Clear();
				for (int i = 0; i < row.Length; i++)
				{
					SQLiteParameter parameter = command.CreateParameter();
					parameter.Value = row[i];
					command.Parameters.Add(parameter);
				}
				command.Prepare();
				command.ExecuteNonQuery();
			}
			tran.Commit();

		}

		/// <summary>
		/// CREATE TABLE文を作成
		/// </summary>
		/// <param name="tableName">テーブル名</param>
		/// <param name="columns">カラム</param>
		/// <returns>CREATE TABLE文</returns>
		private static String createCreateTableSQL(String tableName, String[] columns)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("CREATE TABLE ");
			sb.Append(tableName);
			sb.Append("(");
			sb.Append(columns[0]);
			for (int i = 1; i < columns.Length; i++)
			{
				sb.Append(",");
				sb.Append(columns[i]);
			}
			sb.Append(")");

			return sb.ToString();
		}

		/// <summary>
		/// INSERT文のプリペアドステートメントを作成
		/// </summary>
		/// <param name="tableName">テーブル名</param>
		/// <param name="columnLength">カラム数</param>
		/// <returns>INSERT文のプリペアドステートメント</returns>
		private static String createInsertPsmt(String tableName, int columnLength)
		{
			StringBuilder sb = new StringBuilder(String.Format("INSERT INTO {0} values(?", tableName));
			for (int i = 0; i < (columnLength - 1); i++)
			{
				sb.Append(",?");
			}
			sb.Append(")");

			return sb.ToString();

		}
	}
}

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace sqlcsv4net.src
{
	/// <summary>
	/// コマンドマネージャ
	/// </summary>
	static class CommandManager
	{
		/// <summary>
		/// コマンドを実行.
		/// </summary>
		/// <param name="inputCmd">inputed command</param>
		/// <param name="conn">SQLite connection</param>
		public static void executeCommand(String inputCmd, SQLiteConnection conn)
		{
			if (inputCmd.ToLower().StartsWith("select"))
			{
				try
				{
					SQLiteCommand command = conn.CreateCommand();
					command = conn.CreateCommand();
					command.CommandText = inputCmd;
					SQLiteDataReader reader = command.ExecuteReader();
					if (!reader.HasRows)
					{
						Console.WriteLine("no row selected");
					}
					int fc = reader.FieldCount;
					while (reader.Read())
					{
						String[] row = new String[fc];
						for (int i = 0; i < fc; i++)
						{
							row[i] = reader.GetValue(i).ToString();
						}
						Console.WriteLine(String.Join("\t", row));
					}
					Console.WriteLine();
					Console.WriteLine(reader.StepCount + " row(s) selected");
				}
				catch (SQLiteException e)
				{
					Console.WriteLine(e.Message);
				}
			}
		}
	}
}

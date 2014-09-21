using System;
using System.Data.SQLite;
using System.IO;

namespace sqlcsv4net.src
{
	/// <summary>
	/// main class
	/// </summary>
    class SqlcsvMain
    {
		/// <summary>
		/// main method
		/// </summary>
		/// <param name="args"></param>
		static void Main(String[] args)
        {
			showTitle();

			SQLiteConnection conn = new SQLiteConnection("Data Source=:memory:;Version=3;New=True;");
			conn.Open();

            try
            {
                //analyzeOption(args);

                // csv自動取り込み
				String[] csvFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csv");
				Console.WriteLine("now importing csv file(s)...");
                foreach (String f in csvFiles)
                {
                    CsvImporter.ImportCsvToTable(f, conn);
                }

                String input = "";
				while (!"exit".Equals(input.ToLower()))
				{
					Console.Write(">");
					input = Console.ReadLine();

					if ("".Equals(input.ToLower()))
					{
						continue;
					}
					else if ("exit".Equals(input.ToLower()))
					{
						continue;
					}
					else if ("quit".Equals(input.ToLower()))
					{
						input = "exit";
						continue;
					}
					else if (input.ToLower().StartsWith("import"))
					{
						continue;
					}
					else if (input.ToLower().StartsWith("select"))
					{
						try
						{
							SQLiteCommand command = conn.CreateCommand();
							command = conn.CreateCommand();
							command.CommandText = input;
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
			finally
			{
				conn.Close();
			}
        }

		/// <summary>
		/// アプリのタイトルを表示します
		/// </summary>
		private static void showTitle()
		{
			Console.WriteLine("######################");
			Console.WriteLine();
			Console.WriteLine("Sqlcsv");
			Console.WriteLine();
			Console.WriteLine("######################");
			Console.WriteLine();
			Console.WriteLine();
		}
    }
}

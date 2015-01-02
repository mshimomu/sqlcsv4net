using System;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

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

				Boolean isEnd = false;
				while (!isEnd)
				{
					String input = "";
					while (!(input.Trim()).EndsWith(";"))
					{
						Console.Write("SQL>");
						input += Console.ReadLine().Trim() + " ";

						if ("exit".Equals(input.Trim().ToLower()))
						{
							isEnd = true;
							break;
						}
						else if ("quit".Equals(input.Trim().ToLower()))
						{
							isEnd = true;
							break;
						}
					}

					CommandManager.executeCommand(input, conn);

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
			var assm = Assembly.GetExecutingAssembly();


			Console.WriteLine("SQL*CSV: Release " + assm.GetName().Version);
			Console.WriteLine();
		}
    }
}

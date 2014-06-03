using System.Diagnostics;
using System.Data.SqlClient;

using CsvHelper;
using CsvHelper.Configuration;
using CommandLine;
using CommandLine.Text;

class Options
{
	[Option('o', "output", Required = true, HelpText = "Output file path")]
	public string OutputFile { get; set; }

	[Option('c', "connection-string", Required = true, HelpText = "Connection string")]
	public string ConnectionString { get; set; }

	[Option('q', "query", Required = true, HelpText = "Query")]
	public string Query { get; set; }

	[HelpOption]
	public string GetUsage()
	{
		return HelpText.AutoBuild(this,
		  (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
	}

}
public static int ExportToCsv(string connectionString, string filePath, string queryString)
{
	int records = 0;
	var encoding = Encoding.UTF8;
	using (SqlConnection connection = new SqlConnection(connectionString))
	{
		using (SqlCommand command = new SqlCommand(queryString, connection))
		{
			connection.Open();
			using (var reader = command.ExecuteReader())
			{
				var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
				using (var textWriter = new StreamWriter(filePath, append: false, encoding: Encoding.UTF8))
				{
					//    Console.WriteLine(String.Join(",", columns.ToArray()));
					var configuration = new CsvConfiguration
					{
						Quote = '"',
						Delimiter = ";"
					};
					using (var csv = new CsvWriter(textWriter, configuration))
					{
						columns.ForEach(c => csv.WriteField(c));
						csv.NextRecord();
						while (reader.Read())
						{
							++records;
							foreach (var it in columns.Select((value, index) => new { value, index }))
							{
								csv.WriteField(reader.GetValue(it.index));
							}
							csv.NextRecord();
						}
					}
				}
			}
		}
	}
	return records;
}
		
var options = new Options();
if (CommandLine.Parser.Default.ParseArguments(Env.ScriptArgs.ToArray(), options))
{
	var connectionString = options.ConnectionString;
	var filePath = options.OutputFile;
	var queryString = options.Query;
	var sw = Stopwatch.StartNew();
	int recordCount= ExportToCsv(connectionString, filePath, queryString);
	sw.Stop();
	Console.WriteLine("Zapisano {0} rekordów w czasie {1}", recordCount, sw.Elapsed);	
}
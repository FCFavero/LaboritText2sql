using LaboritText2sql.Service;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace LaboritText2sql.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TextToSqlController : Controller
	{
		private readonly IDbConnection _dbConnection;
		private readonly TextToSqlService _textToSqlService;
		private readonly OpenAIService _OpenAIService;

		public TextToSqlController(IDbConnection dbConnection, TextToSqlService textToSqlService, OpenAIService openAIService)
		{
			_dbConnection = dbConnection;
			_textToSqlService = textToSqlService;
			_OpenAIService = openAIService;
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] UserQueryDto userQuery)
		{
			//var sqlQuery = await _textToSqlService.GenerateSqlQuery(userQuery.Query);
			var sqlQuery = await _OpenAIService.GetSqlQueryAsync(userQuery.Query);

			_dbConnection.Open();
			using var command = new MySqlCommand(sqlQuery, (MySqlConnection)_dbConnection);
			using var reader = command.ExecuteReader();

			var result = new List<Dictionary<string, object>>();
			while (reader.Read())
			{
				var row = new Dictionary<string, object>();
				for (var i = 0; i < reader.FieldCount; i++)
				{
					row[reader.GetName(i)] = reader.GetValue(i);
				}
				result.Add(row);
			}

			_dbConnection.Close();

			return Ok(result);
		}
	}

	public class UserQueryDto
	{
		public string Query { get; set; }
	}
}
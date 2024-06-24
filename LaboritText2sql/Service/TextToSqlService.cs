using System.Threading.Tasks;
using OpenAI_API.Completions; // Verifique se este namespace está disponível
using OpenAI_API;
using OpenAI_API.Models;

namespace LaboritText2sql.Service
{
	public class TextToSqlService
	{
		private readonly OpenAIAPI _openAiApi;

		public TextToSqlService(string apiKey)
		{
			_openAiApi = new OpenAIAPI(apiKey);
		}

		public async Task<string> GenerateSqlQuery(string userQuery)
		{
			var prompt = $"Convert the following text to a SQL query: '{userQuery}'";

			var completionRequest = new CompletionRequest
			{
				Model = "text-davinci-003",
				Prompt = prompt,
				Temperature = 0.5,
				MaxTokens = 150
			};

			var result = await _openAiApi.Completions.CreateCompletionAsync(new CompletionRequest(completionRequest));
			return result.Completions[0].Text.Trim();
		}
	}
}
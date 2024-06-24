using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace LaboritText2sql.Service
{
	public class OpenAIService
	{
		private static readonly HttpClient client = new HttpClient();
		private readonly string apiKey;

		public OpenAIService(string apiKey)
		{
			this.apiKey = apiKey;
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
		}

		public async Task<string> GetSqlQueryAsync(string question)
		{
			var requestBody = new
			{
				model = "gpt-3.5-turbo-0125", 
				prompt = $"Convert this natural language query to SQL: {question}",
				temperature = 0.5,
				max_tokens = 1
			};

			var jsonString = JsonConvert.SerializeObject(requestBody);
			var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

			// Log da URL e do corpo da solicitação para verificação
			Console.WriteLine("Request URL: https://api.openai.com/v1/completions");
			Console.WriteLine($"Request Body: {jsonString}");

			var response = await client.PostAsync("https://api.openai.com/v1/completions", content);

			// Log do status da resposta e do conteúdo em caso de erro
			if (!response.IsSuccessStatusCode)
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				Console.WriteLine($"Error Status Code: {response.StatusCode}");
				Console.WriteLine($"Error Response: {errorContent}");
				throw new Exception($"Error: {response.StatusCode}, Details: {errorContent}");
			}

			var responseString = await response.Content.ReadAsStringAsync();
			dynamic jsonResponse = JObject.Parse(responseString);

			return jsonResponse.choices[0].text.ToString();
		}
	}

}
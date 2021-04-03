using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Chess.Api.DataContracts;
using System.Text.Json;
using System.Text;

namespace Chess.Api.Client
{
    public class GameService
    {
        private readonly HttpClient client;

        public GameService(IHttpClientFactory factory)
        {
            client = factory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:6001/api/game");
        }

        public async Task<string> StartNewGameAsync(string whitePlayerId, string blackPlayerId)
        {
            var request = new StartNewGameRequestDto
            {
                WhitePlayerId = whitePlayerId,
                BlackPlayerId = blackPlayerId
            };

            var jsonRequest = ToJsonContent(request);
            var response = await client.PostAsync("", jsonRequest);
            response.EnsureSuccessStatusCode();

            var game = await response.Content.ReadFromJsonAsync<GameResponseDto>();

            return game.GameId;
        }

        public async Task<GameResponseDto> GetGameAsync(string gameId)
        {
            var response = await client.GetAsync($"{gameId}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<GameResponseDto>();
        }

        private static StringContent ToJsonContent<T>(T item)
        {
            var jsonContent = JsonSerializer.Serialize(item);

            return new StringContent(jsonContent, Encoding.UTF8, "application/json");
        }
    }
}

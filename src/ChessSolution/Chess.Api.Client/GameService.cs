using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Chess.Api.DataContracts;

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

        public async Task<string> StartNewGameAsync()
        {
            var response = await client.PostAsync("", null);
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


    }
}

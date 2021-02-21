using Chess.Api.DataContracts;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chess.Api.Client
{
    public class MovementService
    {
        private readonly HttpClient client;

        public MovementService(IHttpClientFactory httpClientFactory)
        {
            client = httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:6001/api/game/");
        }

        public async Task<PieceMovesResponseDto> GetGameMovesAsync(string gameId)
        {
            var response = await client.GetAsync($"{gameId}/moves");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PieceMovesResponseDto>();
        }

        public async Task<PieceMoveResponseDto> GetPieceMoveAsync(string gameId, int sequenceNumber)
        {
            var response = await client.GetAsync($"{gameId}/moves/{sequenceNumber}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PieceMoveResponseDto>();
        }

        public async Task<PieceMoveResponseDto> MovePieceAsync(string gameId, MovePieceRequestDto pieceMove)
        {
            var jsonContent = ToJsonContent(pieceMove);

            var response = await client.PostAsync($"{gameId}/moves", jsonContent);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PieceMoveResponseDto>();
        }

        private static StringContent ToJsonContent<T>(T item)
        {
            var jsonContent = JsonSerializer.Serialize(item);

            return new StringContent(jsonContent, Encoding.UTF8, "application/json");
        }

    }
}

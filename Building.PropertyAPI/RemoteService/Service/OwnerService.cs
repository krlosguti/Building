
using Building.PropertyAPI.RemoteService.Entities;
using Building.PropertyAPI.RemoteService.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Building.PropertyAPI.RemoteService.Service
{
    public class OwnerService : IOwnerService
    {
        /// <summary>
        /// Inject httpclient to communicate with Owner Microservice
        /// </summary>
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<OwnerService> _logger;

        public OwnerService(IHttpClientFactory httpClient, ILogger<OwnerService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<(bool result, OwnerRemote owner, string ErrorMessage)> GetOwner(Guid Id)
        {
            try
            {
                var client = _httpClient.CreateClient("Owners");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                //var token = _httpContextAccessor.HttpContext.Session.GetString("token");
                //var token = _userSession.GetTokenSession();
                // Acquire the access token.
                string[] scopes = new string[] { "user.read" };
                string token = "1111";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                



                var response = await client.GetAsync($"api/Owners/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<OwnerRemote>(content, options);
                    return (true, result, null);
                }
                return (false, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}

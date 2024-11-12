using BidIP.Models;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace BidIP.Services
{
    public class BidIpService(HttpClient http) : IBidIpService
    {
        public async Task<List<BidIpModel>> GetBidIpAsync()
        {
            return await http.GetFromJsonAsync<List<BidIpModel>>("getStatus") ?? [];
        }
    }
}

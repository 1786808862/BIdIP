using BidIP.Models;

namespace BidIP.Services
{
    public partial interface IBidIpService
    {
        Task<List<BidIpModel>> GetBidIpAsync ();
    }
}

using BidIP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using System.Linq;
using System.Reflection;

namespace BidIP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgentController : ControllerBase
    {
        private readonly IDbContextFactory<BidIP.Data.BidIPContext> _dbFactory;
        public AgentController(IDbContextFactory<BidIP.Data.BidIPContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }
        [HttpGet("status")]
        public async Task<List<BidIP.Models.BidIpModel>> GetStatus(string? Name = null, string? IpAddress = null, int? LastTime = null, int? LastSSYTime = null, int? SsyCount = null)
        {
            List<BidIpModel> bidIpModels = new List<BidIpModel>();
            using var context = _dbFactory.CreateDbContext();
            var myAgentInfo = await context.BidIpModel.ToListAsync();
            //查询符合Name条件的设备，当Name为空字符串时查询所有
            List<BidIpModel> filterName = string.IsNullOrEmpty(Name)
            ? myAgentInfo.ToList() // 查询所有模型
            : myAgentInfo.Where(m => m.Name.Contains(Name)).ToList();

            //查询符合IpAddress条件的设备，当IpAddress为空字符串时查询所有
            List<BidIpModel> filterIpAddress = string.IsNullOrEmpty(IpAddress)
            ? filterName.ToList() // 查询所有模型
            : filterName.Where(m => m.IpAddress.Contains(IpAddress)).ToList();

            //查询符合LastTime条件的设备，当LastTime为-1时查询所有
            List<BidIpModel> filterLastTime = LastTime.HasValue
            ? filterIpAddress.Where(m => m.LastTime.AddSeconds((double)LastTime.Value) < DateTime.Now).ToList()
            : filterIpAddress; // 查询所有模型


            //查询符合LastSSYTime条件的设备，当LastSSYTime为-1时查询所有
            List<BidIpModel> filterLastSSYTime = LastSSYTime == null
            ? filterLastTime // 查询所有模型
            : filterLastTime.Where(m => m.LastSSYTime.AddSeconds((double)LastSSYTime) < DateTime.Now).ToList();

            //查询符合SsyCount条件的设备，当SsyCount为-1时查询所有
            List<BidIpModel> filterSsyCount = SsyCount == null
            ? filterLastSSYTime // 查询所有模型
            : filterLastTime.Where(m => m.SsyCount > SsyCount).ToList();

            List<BidIpModel> result = filterSsyCount.OrderBy(x => new string(x.Name.TakeWhile(c => !char.IsDigit(c)).ToArray()))
                             .ThenBy(x => int.TryParse(new string(x.Name.SkipWhile(c => !char.IsDigit(c)).ToArray()), out int result) ? result : 0)
                             .ToList();

            return result;
        }

    }
}

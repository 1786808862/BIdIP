using BidIP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MudBlazor;


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
        public async Task<List<MachineDetailInfo>> GetStatus(string? Name = null, string? IpAddress = null, int? LastTime = null, int? LastSSYTime = null, int? SsyCount = null)
        {
            List<MachineDetailInfo> machineDetailInfos = new List<MachineDetailInfo>();
            var context = _dbFactory.CreateDbContext();
            var myAgentInfo = await context.MachineDetailInfo.ToListAsync();

            //查询符合Name条件的设备，当Name为空字符串时查询所有
            List<MachineDetailInfo> filterName = string.IsNullOrEmpty(Name)
            ? myAgentInfo.ToList() // 查询所有模型
            : myAgentInfo.Where(m => m.Name.Contains(Name)).ToList();

            //查询符合IpAddress条件的设备，当IpAddress为空字符串时查询所有
            List<MachineDetailInfo> filterIpAddress = string.IsNullOrEmpty(IpAddress)
            ? filterName.ToList() // 查询所有模型
            : filterName.Where(m => m.IpAddress.Contains(IpAddress)).ToList();

            //查询符合LastTime条件的设备，当LastTime为-1时查询所有
            List<MachineDetailInfo> filterLastTime = LastTime.HasValue
            ? filterIpAddress.Where(m => m.LastTime.AddSeconds((double)LastTime.Value) < DateTime.Now).ToList()
            : filterIpAddress; // 查询所有模型


            //查询符合LastSSYTime条件的设备，当LastSSYTime为-1时查询所有
            List<MachineDetailInfo> filterLastSSYTime = LastSSYTime == null
            ? filterLastTime // 查询所有模型
            : filterLastTime.Where(m => m.LastSSYTime.AddSeconds((double)LastSSYTime) < DateTime.Now).ToList();

            //查询符合SsyCount条件的设备，当SsyCount为-1时查询所有
            List<MachineDetailInfo> filterSsyCount = SsyCount == null
            ? filterLastSSYTime // 查询所有模型
            : filterLastTime.Where(m => m.SsyCount > SsyCount).ToList();

            List<MachineCategory> resultCategory = new List<MachineCategory>();
            foreach (var machineDetailInfo in filterSsyCount)
            {
                var a = await context.MachineCategory.FirstOrDefaultAsync(m => m.Id == machineDetailInfo.MachineCategoryId);
                machineDetailInfo.MachineCategory.Id = a.Id;
                machineDetailInfo.MachineCategory.Name = a.Name;
                machineDetailInfo.MachineCategory.MachineDetailInfo = null;
            }

            List<MachineDetailInfo> result = filterSsyCount.OrderBy(x => new string(x.Name.TakeWhile(c => !char.IsDigit(c)).ToArray()))
                             .ThenBy(x => int.TryParse(new string(x.Name.SkipWhile(c => !char.IsDigit(c)).ToArray()), out int result) ? result : 0)
                             .ToList();

            return result;
        }

        [HttpPost("setStatus")]
        public async Task<IActionResult> SetStatus([FromBody] MachineDetailInfo machineDetailInfo)
        {

            using var context = _dbFactory.CreateDbContext();
            var existingModel = await context.MachineDetailInfo.FirstOrDefaultAsync(m => m.Name == machineDetailInfo.Name);
            if (existingModel == null)
            {
                machineDetailInfo.LastTime = DateTime.Now;
                context.MachineDetailInfo.Add(machineDetailInfo);
            }
            else
            {
                if (!string.IsNullOrEmpty(machineDetailInfo.IpAddress))
                {
                    existingModel.IpAddress = machineDetailInfo.IpAddress;
                }
                if (machineDetailInfo.LastTime != DateTime.MinValue)
                {
                    existingModel.LastTime = machineDetailInfo.LastTime;
                }
                if (machineDetailInfo.LastSSYTime != DateTime.MinValue)
                {
                    existingModel.LastSSYTime = machineDetailInfo.LastSSYTime;
                }
                if (machineDetailInfo.SsyCount != 0)
                {
                    existingModel.SsyCount = machineDetailInfo.SsyCount;
                }
                context.Attach(existingModel).State = EntityState.Modified;
            }
            int result = await context.SaveChangesAsync();

            if (result > 0)
            {
                return Ok(new { statusCode = 200, message = "OK" });
            }
            else
            {
                return Ok(new { statusCode = 200, message = "Fail" });
            }
        }

    }
}

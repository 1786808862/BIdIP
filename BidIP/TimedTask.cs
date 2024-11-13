using BidIP;
using BidIP.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

public class TimedTask
{
    private Timer _timer;
    private readonly IDbContextFactory<BidIP.Data.BidIPContext> _dbFactory;
    private readonly IMemoryCache _cache;
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    public TimedTask(IDbContextFactory<BidIP.Data.BidIPContext> dbFactory
        //, IMemoryCache cache
        )
    {
        _dbFactory = dbFactory;
        _timer = new Timer(ExecuteTask, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        //_cache = cache;
    }

    //public async Task InitializeCacheAsync()
    //{
    //    // 检查缓存是否已存在数据
    //    if (!_cache.TryGetValue("NameCache", out List<string> names))
    //    {
    //        // 如果缓存不存在，则从数据库获取数据
    //        using (var context = _dbFactory.CreateDbContext())
    //        {
    //            names = await context.MachineDetailInfo.Select(b => b.Name).ToListAsync();
    //        }

    //        // 将数据存入缓存
    //        _cache.Set("NameCache", names);
    //    }
    //}

    private async void ExecuteTask(object state)
    {
        Random random = new Random();
        //var names = _cache.Get<List<string>>("NameCache") ?? new List<string>();

        // 确保在任务执行完成后关闭 DbContext 并释放连接
        await using (var context = _dbFactory.CreateDbContext())
        {
            await using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    for (int i = 0; i < 20; i++)
                    {
                        int randomNumber = random.Next(1, 141);
                        var machineDetailInfo = new MachineDetailInfo
                        {
                            Name = "JA" + randomNumber,
                            IpAddress = "192.168.0." + randomNumber,
                            LastTime = DateTime.Now.AddSeconds(-random.Next(1, 20)),
                            LastSSYTime = DateTime.Now.AddSeconds(-random.Next(1, 20)),
                            SsyCount = random.Next(1, 1111),
                            MachineCategoryId = 1
                        };
                        var existingModel = await context.MachineDetailInfo.AsNoTracking()
                                            .FirstOrDefaultAsync(b => b.Name == machineDetailInfo.Name);

                        if (existingModel == null)
                        {
                            //names.Add(machineDetailInfo.Name);
                            context.MachineDetailInfo.Add(machineDetailInfo);
                        }
                        else
                        {
                            existingModel.IpAddress = machineDetailInfo.IpAddress;
                            existingModel.LastTime = machineDetailInfo.LastTime;
                            existingModel.LastSSYTime = machineDetailInfo.LastSSYTime;
                            existingModel.SsyCount = machineDetailInfo.SsyCount;
                            existingModel.MachineCategoryId = machineDetailInfo.MachineCategoryId;
                            context.MachineDetailInfo.Update(existingModel);
                        }
                        // 提交所有变更
                        await context.SaveChangesAsync();
                        context.ChangeTracker.Clear();  // 清除所有跟踪的实体

                        randomNumber = random.Next(141, 281);
                        machineDetailInfo = new MachineDetailInfo
                        {
                            Name = "JD" + (randomNumber - 140),
                            IpAddress = "192.168.1." + (randomNumber - 140),
                            LastTime = DateTime.Now.AddSeconds(-random.Next(1, 20)),
                            LastSSYTime = DateTime.Now.AddSeconds(-random.Next(1, 20)),
                            SsyCount = random.Next(1, 1111),
                            MachineCategoryId = 1
                        };
                        existingModel = await context.MachineDetailInfo.AsNoTracking()
                                            .FirstOrDefaultAsync(b => b.Name == machineDetailInfo.Name);
                        if (existingModel == null)
                        {
                            //names.Add(machineDetailInfo.Name);
                            context.MachineDetailInfo.Add(machineDetailInfo);
                        }
                        else
                        {
                            existingModel.IpAddress = machineDetailInfo.IpAddress;
                            existingModel.LastTime = machineDetailInfo.LastTime;
                            existingModel.LastSSYTime = machineDetailInfo.LastSSYTime;
                            existingModel.SsyCount = machineDetailInfo.SsyCount;
                            existingModel.MachineCategoryId = machineDetailInfo.MachineCategoryId;
                            context.MachineDetailInfo.Update(existingModel);
                        }
                        // 提交所有变更
                        await context.SaveChangesAsync();
                        context.ChangeTracker.Clear();  // 清除所有跟踪的实体

                    }


                    // 提交事务
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    // 如果发生异常则回滚
                    await transaction.RollbackAsync();
                    throw;
                }
                finally
                {
                    // 显式调用释放资源
                    await transaction.DisposeAsync();
                    await context.DisposeAsync();
                    //_cache.Set("NameCache", names);
                }
            }
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BidIP.Models
{
    public class MachineDetailInfo
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string? IpAddress { get; set; }

        public DateTime LastTime { get; set; }

        public DateTime LastSSYTime { get; set; }

        public int SsyCount { get; set; }

        [ForeignKey("Author")]
        public int MachineCategoryId { get; set; }

        public MachineCategory MachineCategory { get; set; }

        public MachineDetailInfo(int id, string name, string ipAddress, DateTime lastTime, DateTime lastSSYTime, int ssyCount)
        {
            Id = id;
            Name = name;
            IpAddress = ipAddress;
            LastTime = lastTime;
            LastSSYTime = lastSSYTime;
            SsyCount = ssyCount;
        }

        public MachineDetailInfo()
        {
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BidIP.Models
{
    public class BidIpModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string IpAddress { get; set; }

        public DateTime LastTime { get; set; }

        public DateTime LastSSYTime { get; set; }

        public int SsyCount { get; set; }

        public BidIpModel(int id, string name, string ipAddress, DateTime lastTime, DateTime lastSSYTime, int ssyCount)
        {
            Id = id;
            Name = name;
            IpAddress = ipAddress;
            LastTime = lastTime;
            LastSSYTime = lastSSYTime;
            SsyCount = ssyCount;
        }

        public BidIpModel()
        {
        }
    }
}

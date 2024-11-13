using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BidIP.Models
{
    public class MachineCategory
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<MachineDetailInfo> MachineDetailInfo { get; set; }
    }
}

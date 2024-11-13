using System.ComponentModel.DataAnnotations;

namespace BidIP.Models
{
    public class CustomerInfo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

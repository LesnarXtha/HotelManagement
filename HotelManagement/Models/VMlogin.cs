using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class VMlogin
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
      
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }


    }
}

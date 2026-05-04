using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maintainancebackend.Models
{
    public class Admin
    {
        [Key]
        public int Admin_id { get; set; }

        [Required]
        public string admin_name { get; set; } = "";

        [Required]
        public string mobile_number { get; set; } = "";

        [Required]
        public string password { get; set; } = "";

        [Required]
        public int building_id { get; set; }

        // Navigation property (optional but good practice)
        public Building? Building { get; set; }
    }
}
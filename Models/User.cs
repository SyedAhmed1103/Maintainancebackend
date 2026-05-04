namespace Maintainancebackend.Models
{
    public class User
    {
        public int user_id { get; set; }

        public string user_name { get; set; } = "";

        public string mobile_number { get; set; } = "";

        public string password { get; set; } = "";

        public int building_id { get; set; }
    }
}
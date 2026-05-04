namespace Maintainancebackend.Models
{
    public class UserFlat
    {
        public int user_flat_id { get; set; }

        public int user_id { get; set; }

        public int building_id { get; set; }

        public string wing { get; set; } = "";

        public string flat_number { get; set; } = "";
    }
}
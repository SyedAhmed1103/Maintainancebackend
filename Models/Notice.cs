namespace Maintainancebackend.Models
{
    public class Notice
    {
        public int notice_id { get; set; }

        public int building_id { get; set; }

        public string? title { get; set; }

        public string? description { get; set; }

        public DateTime date { get; set; }
    }
}
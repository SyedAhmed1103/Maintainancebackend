namespace Maintainancebackend.Models
{
    public class Fund
    {
        public int fund_id { get; set; }

        public int building_id { get; set; }

        public string? title { get; set; }

        public decimal amount { get; set; }

        public string? description { get; set; }

        public string? transaction_proof { get; set; }

        public DateTime date { get; set; }
    }
}
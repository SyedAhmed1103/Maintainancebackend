namespace Maintainancebackend.Models
{
    public class Payment
    {
        public int payment_id { get; set; }

        public int user_flat_id { get; set; }

        public int building_id { get; set; }

        public decimal amount { get; set; }

        public int month { get; set; }

        public int year { get; set; }

        public string status { get; set; } = "Pending";

        public string? transaction_id { get; set; }

        public DateTime date { get; set; }
    }
}
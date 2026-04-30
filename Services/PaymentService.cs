using Microsoft.Data.SqlClient;

public class PaymentService
{
    private readonly DatabaseService _db;

    public PaymentService(DatabaseService db)
    {
        _db = db;
    }

    public void PayOldest(int userFlatId)
    {
        using (var conn = _db.GetConnection())
        {
            conn.Open();

            var cmd = new SqlCommand(@"
                SELECT TOP 1 payment_id 
                FROM PAYMENTS
                WHERE user_flat_id = @id AND status = 'Pending'
                ORDER BY year, month", conn);

            cmd.Parameters.AddWithValue("@id", userFlatId);

            var result = cmd.ExecuteScalar();

            if (result != null)
            {
                var update = new SqlCommand(@"
                    UPDATE PAYMENTS
                    SET status = 'Paid', date = GETDATE()
                    WHERE payment_id = @pid", conn);

                update.Parameters.AddWithValue("@pid", result);
                update.ExecuteNonQuery();
            }
        }
    }
}
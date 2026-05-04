using Maintainancebackend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

public class PaymentService
{
    private readonly DatabaseService _db;

    public PaymentService(DatabaseService db)
    {
        _db = db;
    }

    // ✅ GET ALL PAYMENTS (tenant-safe)
    public async Task<List<Payment>> GetAllAsync(int buildingId)
    {
        var list = new List<Payment>();

        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            "SELECT * FROM payments WHERE building_id = @buildingId", conn);

        cmd.Parameters.Add("@buildingId", SqlDbType.Int).Value = buildingId;

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(Map(reader));
        }

        return list;
    }

    // ✅ GET BY FLAT (history)
    public async Task<List<Payment>> GetByFlatAsync(int userFlatId, int buildingId)
    {
        var list = new List<Payment>();

        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            @"SELECT * FROM payments 
              WHERE user_flat_id = @flat AND building_id = @buildingId", conn);

        cmd.Parameters.Add("@flat", SqlDbType.Int).Value = userFlatId;
        cmd.Parameters.Add("@buildingId", SqlDbType.Int).Value = buildingId;

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(Map(reader));
        }

        return list;
    }

    // 💰 CREATE PAYMENT (SAFE + NO DUPLICATE)
    public async Task CreateAsync(Payment p)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        using var transaction = conn.BeginTransaction();

        try
        {
            // ❗ Check duplicate (same flat, month, year)
            var checkCmd = new SqlCommand(
                @"SELECT COUNT(*) FROM payments 
                  WHERE user_flat_id = @flat 
                  AND month = @month 
                  AND year = @year", conn, transaction);

            checkCmd.Parameters.Add("@flat", SqlDbType.Int).Value = p.user_flat_id;
            checkCmd.Parameters.Add("@month", SqlDbType.Int).Value = p.month;
            checkCmd.Parameters.Add("@year", SqlDbType.Int).Value = p.year;

            int exists = (int)await checkCmd.ExecuteScalarAsync();

            if (exists > 0)
                throw new Exception("Payment already exists for this month");

            // ✅ Insert
            var cmd = new SqlCommand(
                @"INSERT INTO payments 
                  (user_flat_id, building_id, amount, month, year, status, transaction_id)
                  VALUES (@flat, @building, @amount, @month, @year, @status, @txn)", conn, transaction);

            cmd.Parameters.Add("@flat", SqlDbType.Int).Value = p.user_flat_id;
            cmd.Parameters.Add("@building", SqlDbType.Int).Value = p.building_id;
            cmd.Parameters.Add("@amount", SqlDbType.Decimal).Value = p.amount;
            cmd.Parameters.Add("@month", SqlDbType.Int).Value = p.month;
            cmd.Parameters.Add("@year", SqlDbType.Int).Value = p.year;
            cmd.Parameters.Add("@status", SqlDbType.VarChar, 10).Value = p.status;
            cmd.Parameters.Add("@txn", SqlDbType.VarChar, 100).Value =
                (object?)p.transaction_id ?? DBNull.Value;

            await cmd.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // ✅ MARK AS PAID
    public async Task MarkAsPaidAsync(int paymentId, string transactionId, int buildingId)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            @"UPDATE payments 
              SET status = 'Paid', transaction_id = @txn, date = GETDATE()
              WHERE payment_id = @id AND building_id = @buildingId", conn);

        cmd.Parameters.Add("@id", SqlDbType.Int).Value = paymentId;
        cmd.Parameters.Add("@txn", SqlDbType.VarChar, 100).Value = transactionId;
        cmd.Parameters.Add("@buildingId", SqlDbType.Int).Value = buildingId;

        await cmd.ExecuteNonQueryAsync();
    }

    // 🧼 MAPPING
    private Payment Map(SqlDataReader reader)
    {
        return new Payment
        {
            payment_id = (int)reader["payment_id"],
            user_flat_id = (int)reader["user_flat_id"],
            building_id = (int)reader["building_id"],
            amount = (decimal)reader["amount"],
            month = (int)reader["month"],
            year = (int)reader["year"],
            status = reader["status"]?.ToString() ?? "",
            transaction_id = reader["transaction_id"]?.ToString(),
            date = (DateTime)reader["date"]
        };
    }
}
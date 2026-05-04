using Maintainancebackend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

public class FundService
{
    private readonly DatabaseService _db;

    public FundService(DatabaseService db)
    {
        _db = db;
    }

    // ✅ GET ALL
    public async Task<List<Fund>> GetAllAsync(int buildingId)
    {
        var list = new List<Fund>();

        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            @"SELECT * FROM fund 
              WHERE building_id = @buildingId
              ORDER BY date DESC", conn);

        cmd.Parameters.Add("@buildingId", SqlDbType.Int).Value = buildingId;

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(Map(reader));
        }

        return list;
    }

    // ✅ CREATE (ONLY ADD, NO DELETE)
    public async Task CreateAsync(Fund f)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            @"INSERT INTO fund 
              (building_id, title, amount, description, transaction_proof)
              VALUES (@building, @title, @amount, @desc, @proof)", conn);

        cmd.Parameters.Add("@building", SqlDbType.Int).Value = f.building_id;

        cmd.Parameters.Add("@title", SqlDbType.VarChar, 150).Value =
            (object?)f.title ?? DBNull.Value;

        cmd.Parameters.Add("@amount", SqlDbType.Decimal).Value = f.amount;

        cmd.Parameters.Add("@desc", SqlDbType.VarChar).Value =
            (object?)f.description ?? DBNull.Value;

        cmd.Parameters.Add("@proof", SqlDbType.VarChar, 255).Value =
            (object?)f.transaction_proof ?? DBNull.Value;

        await cmd.ExecuteNonQueryAsync();
    }

    // 🧼 MAP
    private Fund Map(SqlDataReader reader)
    {
        return new Fund
        {
            fund_id = (int)reader["fund_id"],
            building_id = (int)reader["building_id"],
            title = reader["title"]?.ToString(),
            amount = (decimal)reader["amount"],
            description = reader["description"]?.ToString(),
            transaction_proof = reader["transaction_proof"]?.ToString(),
            date = (DateTime)reader["date"]
        };
    }
}
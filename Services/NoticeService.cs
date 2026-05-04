using Maintainancebackend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

public class NoticeService
{
    private readonly DatabaseService _db;

    public NoticeService(DatabaseService db)
    {
        _db = db;
    }

    // ✅ GET ALL (latest first)
    public async Task<List<Notice>> GetAllAsync(int buildingId)
    {
        var list = new List<Notice>();

        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            @"SELECT * FROM notices 
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

    // ✅ CREATE
    public async Task CreateAsync(Notice n)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            @"INSERT INTO notices (building_id, title, description)
              VALUES (@building, @title, @desc)", conn);

        cmd.Parameters.Add("@building", SqlDbType.Int).Value = n.building_id;
        cmd.Parameters.Add("@title", SqlDbType.VarChar, 150).Value =
            (object?)n.title ?? DBNull.Value;

        cmd.Parameters.Add("@desc", SqlDbType.VarChar).Value =
            (object?)n.description ?? DBNull.Value;

        await cmd.ExecuteNonQueryAsync();
    }

    // ✅ DELETE (hard delete)
    public async Task DeleteAsync(int id, int buildingId)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            @"DELETE FROM notices 
              WHERE notice_id = @id AND building_id = @buildingId", conn);

        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        cmd.Parameters.Add("@buildingId", SqlDbType.Int).Value = buildingId;

        await cmd.ExecuteNonQueryAsync();
    }

    // 🧼 MAP
    private Notice Map(SqlDataReader reader)
    {
        return new Notice
        {
            notice_id = (int)reader["notice_id"],
            building_id = (int)reader["building_id"],
            title = reader["title"]?.ToString(),
            description = reader["description"]?.ToString(),
            date = (DateTime)reader["date"]
        };
    }
}
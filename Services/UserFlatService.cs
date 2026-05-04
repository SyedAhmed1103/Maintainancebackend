using Maintainancebackend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

public class UserFlatService
{
    private readonly DatabaseService _db;

    public UserFlatService(DatabaseService db)
    {
        _db = db;
    }

    // ✅ GET ALL (by building)
    public async Task<List<UserFlat>> GetAllAsync(int buildingId)
    {
        var list = new List<UserFlat>();

        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            "SELECT * FROM user_flats WHERE building_id = @buildingId", conn);

        cmd.Parameters.Add("@buildingId", SqlDbType.Int).Value = buildingId;

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(Map(reader));
        }

        return list;
    }

    // ✅ GET BY USER
    public async Task<List<UserFlat>> GetByUserAsync(int userId, int buildingId)
    {
        var list = new List<UserFlat>();

        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            @"SELECT * FROM user_flats 
              WHERE user_id = @userId AND building_id = @buildingId", conn);

        cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
        cmd.Parameters.Add("@buildingId", SqlDbType.Int).Value = buildingId;

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(Map(reader));
        }

        return list;
    }

    // ✅ CREATE
    public async Task CreateAsync(UserFlat f)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            @"INSERT INTO user_flats 
              (user_id, building_id, wing, flat_number)
              VALUES (@user, @building, @wing, @flat)", conn);

        cmd.Parameters.Add("@user", SqlDbType.Int).Value = f.user_id;
        cmd.Parameters.Add("@building", SqlDbType.Int).Value = f.building_id;
        cmd.Parameters.Add("@wing", SqlDbType.Char, 1).Value = f.wing;
        cmd.Parameters.Add("@flat", SqlDbType.VarChar, 10).Value = f.flat_number;

        await cmd.ExecuteNonQueryAsync();
    }

    // ✅ DELETE
    public async Task DeleteAsync(int id, int buildingId)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            @"DELETE FROM user_flats 
              WHERE user_flat_id = @id AND building_id = @buildingId", conn);

        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        cmd.Parameters.Add("@buildingId", SqlDbType.Int).Value = buildingId;

        await cmd.ExecuteNonQueryAsync();
    }

    // 🧼 MAPPING
    private UserFlat Map(SqlDataReader reader)
    {
        return new UserFlat
        {
            user_flat_id = (int)reader["user_flat_id"],
            user_id = (int)reader["user_id"],
            building_id = (int)reader["building_id"],
            wing = reader["wing"]?.ToString() ?? "",
            flat_number = reader["flat_number"]?.ToString() ?? ""
        };
    }
}

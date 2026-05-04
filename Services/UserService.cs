using Maintainancebackend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

public class UserService
{
    private readonly DatabaseService _db;

    public UserService(DatabaseService db)
    {
        _db = db;
    }

    // ✅ GET ALL
    public async Task<List<User>> GetAllAsync(int buildingId)
    {
        var list = new List<User>();

        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            "SELECT * FROM users WHERE building_id = @buildingId", conn);

        cmd.Parameters.Add("@buildingId", SqlDbType.Int).Value = buildingId;

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(Map(reader));
        }

        return list;
    }

    // ✅ CREATE
    public async Task CreateAsync(User u)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            @"INSERT INTO users (user_name, mobile_number, password, building_id)
              VALUES (@name, @mobile, @pass, @building)", conn);

        cmd.Parameters.Add("@name", SqlDbType.VarChar, 100).Value = u.user_name;
        cmd.Parameters.Add("@mobile", SqlDbType.VarChar, 15).Value = u.mobile_number;
        cmd.Parameters.Add("@pass", SqlDbType.VarChar, 255).Value = u.password;
        cmd.Parameters.Add("@building", SqlDbType.Int).Value = u.building_id;

        await cmd.ExecuteNonQueryAsync();
    }

    // ✅ LOGIN
    public async Task<User?> LoginAsync(string mobile, string password)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            "SELECT * FROM users WHERE mobile_number = @mobile", conn);

        cmd.Parameters.Add("@mobile", SqlDbType.VarChar, 15).Value = mobile;

        using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var user = Map(reader);

            // ⚠️ Plain text comparison (as per your requirement)
            if (user.password == password)
                return user;
        }

        return null;
    }

    // ✅ DELETE
    public async Task DeleteAsync(int id, int buildingId)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            @"DELETE FROM users 
              WHERE user_id = @id AND building_id = @buildingId", conn);

        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        cmd.Parameters.Add("@buildingId", SqlDbType.Int).Value = buildingId;

        await cmd.ExecuteNonQueryAsync();
    }

    // 🧼 MAP
    private User Map(SqlDataReader reader)
    {
        return new User
        {
            user_id = (int)reader["user_id"],
            user_name = reader["user_name"]?.ToString() ?? "",
            mobile_number = reader["mobile_number"]?.ToString() ?? "",
            password = reader["password"]?.ToString() ?? "",
            building_id = (int)reader["building_id"]
        };
    }
}
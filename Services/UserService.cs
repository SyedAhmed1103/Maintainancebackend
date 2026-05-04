using BCrypt.Net;
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

    // ✅ GET USERS (tenant-safe)
    public async Task<List<User>> GetAllAsync(int buildingId)
    {
        var list = new List<User>();

        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            "SELECT * FROM users WHERE building_id = @buildingId AND is_active = 1", conn);

        cmd.Parameters.Add("@buildingId", SqlDbType.Int).Value = buildingId;

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(MapUser(reader));
        }

        return list;
    }

    // ✅ CREATE USER
    public async Task CreateAsync(User u)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            @"INSERT INTO users 
              (user_name, mobile_number, password, building_id, role) 
              VALUES (@name, @mobile, @pass, @building, @role)", conn);

        cmd.Parameters.Add("@name", SqlDbType.VarChar, 100).Value = u.user_name;
        cmd.Parameters.Add("@mobile", SqlDbType.VarChar, 15).Value = u.mobile_number;

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(u.password);
        cmd.Parameters.Add("@pass", SqlDbType.VarChar, 255).Value = hashedPassword;

        cmd.Parameters.Add("@building", SqlDbType.Int).Value = u.building_id;
        cmd.Parameters.Add("@role", SqlDbType.VarChar, 20).Value = u.role ?? "Member";

        await cmd.ExecuteNonQueryAsync();
    }

    // ✅ LOGIN
    public async Task<User?> LoginAsync(string mobile, string password)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            "SELECT * FROM users WHERE mobile_number = @mobile AND is_active = 1", conn);

        cmd.Parameters.Add("@mobile", SqlDbType.VarChar, 15).Value = mobile;

        using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var user = MapUser(reader);

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.password);

            if (isValid)
                return user;
        }

        return null;
    }

    // ✅ SOFT DELETE (Deactivate)
    public async Task DeactivateAsync(int userId, int buildingId)
    {
        using var conn = _db.GetConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            @"UPDATE users 
              SET is_active = 0 
              WHERE user_id = @id AND building_id = @buildingId", conn);

        cmd.Parameters.Add("@id", SqlDbType.Int).Value = userId;
        cmd.Parameters.Add("@buildingId", SqlDbType.Int).Value = buildingId;

        await cmd.ExecuteNonQueryAsync();
    }

    // 🧼 MAPPING
    private User MapUser(SqlDataReader reader)
    {
        return new User
        {
            user_id = (int)reader["user_id"],
            user_name = reader["user_name"]?.ToString() ?? "",
            mobile_number = reader["mobile_number"]?.ToString() ?? "",
            password = reader["password"]?.ToString() ?? "",
            building_id = (int)reader["building_id"],
            role = reader["role"]?.ToString() ?? "Member"
        };
    }
}
using Microsoft.Data.SqlClient;

public class UserService
{
    private readonly DatabaseService _db;

    public UserService(DatabaseService db)
    {
        _db = db;
    }

    public List<object> GetUsers()
    {
        var users = new List<object>();

        using (var conn = _db.GetConnection())
        {
            conn.Open();

            var cmd = new SqlCommand("SELECT * FROM USERS", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new
                {
                    user_id = reader["user_id"],
                    user_name = reader["user_name"],
                    mobile = reader["mobile_number"]
                });
            }
        }

        return users;
    }
}
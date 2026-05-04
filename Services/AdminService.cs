using Microsoft.Data.SqlClient;

public class AdminService
{
    private readonly DatabaseService _db;

    public AdminService(DatabaseService db)
    {
        _db = db;
    }

    // GET ALL
    public List<Admin> GetAll()
    {
        var list = new List<Admin>();

        using (var conn = _db.GetConnection())
        {
            conn.Open();

            var cmd = new SqlCommand("SELECT * FROM admin", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Admin
                {
                    AdminId = (int)reader["admin_id"],
                    AdminName = reader["admin_name"]?.ToString() ?? "",
                    MobileNumber = reader["mobile_number"]?.ToString() ?? "",
                    Password = reader["password"]?.ToString() ?? "",
                    BuildingId = (int)reader["building_id"]
                });
            }
        }

        return list;
    }

    // CREATE
    public void Create(Admin a)
    {
        using (var conn = _db.GetConnection())
        {
            conn.Open();

            var cmd = new SqlCommand(@"
                INSERT INTO admin (admin_name, mobile_number, password, building_id)
                VALUES (@name, @mobile, @pass, @bid)", conn);

            cmd.Parameters.AddWithValue("@name", a.AdminName);
            cmd.Parameters.AddWithValue("@mobile", a.MobileNumber);
            cmd.Parameters.AddWithValue("@pass", a.Password);
            cmd.Parameters.AddWithValue("@bid", a.BuildingId);

            cmd.ExecuteNonQuery();
        }
    }
}
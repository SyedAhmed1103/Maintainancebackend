using Microsoft.Data.SqlClient;

public class BuildingService
{
    private readonly DatabaseService _db;

    public BuildingService(DatabaseService db)
    {
        _db = db;
    }

    // GET ALL
    public List<Building> GetAll()
    {
        var list = new List<Building>();

        using (var conn = _db.GetConnection())
        {
            conn.Open();

            var cmd = new SqlCommand("SELECT * FROM BUILDING", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Building
                {
                    BuildingId = (int)reader["building_id"],
                    BuildingName = reader["building_name"].ToString(),
                    Location = reader["location"].ToString()
                });
            }
        }

        return list;
    }

    // GET BY ID
    public Building? GetById(int id)
    {
        using (var conn = _db.GetConnection())
        {
            conn.Open();

            var cmd = new SqlCommand(
                "SELECT * FROM BUILDING WHERE building_id=@id", conn);

            cmd.Parameters.AddWithValue("@id", id);

            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Building
                {
                    BuildingId = (int)reader["building_id"],
                    BuildingName = reader["building_name"].ToString(),
                    Location = reader["location"].ToString()
                };
            }
        }

        return null;
    }

    // CREATE
    public void Create(Building b)
    {
        using (var conn = _db.GetConnection())
        {
            conn.Open();

            var cmd = new SqlCommand(
                "INSERT INTO BUILDING (building_name, location) VALUES (@name,@loc)", conn);

            cmd.Parameters.AddWithValue("@name", b.BuildingName);
            cmd.Parameters.AddWithValue("@loc", b.Location);

            cmd.ExecuteNonQuery();
        }
    }

    // UPDATE
    public void Update(int id, Building b)
    {
        using (var conn = _db.GetConnection())
        {
            conn.Open();

            var cmd = new SqlCommand(
                "UPDATE BUILDING SET building_name=@name, location=@loc WHERE building_id=@id", conn);

            cmd.Parameters.AddWithValue("@name", b.BuildingName);
            cmd.Parameters.AddWithValue("@loc", b.Location);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }

    // DELETE
    public void Delete(int id)
    {
        using (var conn = _db.GetConnection())
        {
            conn.Open();

            var cmd = new SqlCommand(
                "DELETE FROM BUILDING WHERE building_id=@id", conn);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
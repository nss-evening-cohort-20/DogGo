using DogGo.Models;
using Microsoft.Data.SqlClient;

namespace DogGo.Repositories;

public class WalkRepository : BaseRepository, IWalkRepository
{
    public WalkRepository(IConfiguration config) : base(config)
    {
    }

    public List<Walk> GetAll()
    {
        using SqlConnection conn = Connection;
        conn.Open();

        using SqlCommand cmd = conn.CreateCommand();
        cmd.CommandText = @"
                            SELECT [Id]
                                  ,[Date]
                                  ,[Duration]
                                  ,[WalkerId]
                                  ,[DogId]
                            FROM [DogWalkerMVC].[dbo].[Walks]";

        using SqlDataReader reader = cmd.ExecuteReader();
        List<Walk> result = new List<Walk>();
        while (reader.Read())
        {
            result.Add(new Walk()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
            });
        }

        return result;
    }

    public List<Walk> GetByWalkerId(int id)
    {
        using SqlConnection conn = Connection;
        conn.Open();

        using SqlCommand cmd = conn.CreateCommand();
        cmd.CommandText = @"
                            SELECT [Id]
                                  ,[Date]
                                  ,[Duration]
                                  ,[WalkerId]
                                  ,[DogId]
                            FROM [DogWalkerMVC].[dbo].[Walks]
                            WHERE WalkerId = @id";
        cmd.Parameters.AddWithValue("@id", id);

        using SqlDataReader reader = cmd.ExecuteReader();
        List<Walk> result = new List<Walk>();
        while (reader.Read())
        {
            result.Add(new Walk()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
            });
        }

        return result;
    }

    public void InsertWalk(Walk walk)
    {
        using SqlConnection conn = Connection;
        conn.Open();

        using SqlCommand cmd = conn.CreateCommand();
        cmd.CommandText = @"
                            INSERT INTO Walks
                            ([Date], [Duration], [WalkerId], [DogId])
                            OUTPUT INSERTED.ID
                            VALUES
                            (@date, @duration, @walkerId, @dogId)";
        cmd.Parameters.AddWithValue("@date", walk.Date);
        cmd.Parameters.AddWithValue("@duration", walk.Duration);
        cmd.Parameters.AddWithValue("@walkerId", walk.WalkerId);
        cmd.Parameters.AddWithValue("@dogId", walk.DogId);

        int id = (int)cmd.ExecuteScalar();

        walk.Id = id;
    }

}

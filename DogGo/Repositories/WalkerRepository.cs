using DogGo.Models;
using Microsoft.Data.SqlClient;

namespace DogGo.Repositories;

public class WalkerRepository : BaseRepository, IWalkerRepository
{
    public WalkerRepository(IConfiguration config) : base(config) { }

    public List<Walker> GetAllWalkers()
    {
        using SqlConnection conn = Connection;
        conn.Open();
        using SqlCommand cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT w.Id as WalkerId
                                  ,w.[Name] as WalkerName
                            	  ,w.ImageUrl
                            	  ,w.NeighborhoodId
                            	  ,n.[Name] as NeighborhoodName
                            FROM Walker w
                            LEFT JOIN Neighborhood n ON n.Id = w.NeighborhoodId";

        using SqlDataReader reader = cmd.ExecuteReader();

        List<Walker> walkers = new List<Walker>();
        while (reader.Read())
        {
            Walker walker = new Walker
            {
                Id = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                Name = reader.GetString(reader.GetOrdinal("WalkerName")),
                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                Neighborhood = new Neighborhood()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                    Name = reader.GetString(reader.GetOrdinal("NeighborhoodName"))
                }
            };

            walkers.Add(walker);
        }

        return walkers;
    }

    public Walker? GetWalkerById(int id)
    {
        using SqlConnection conn = Connection;
        conn.Open();
        using SqlCommand cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT w.Id as WalkerId
                                  ,w.Name as WalkerName
                            	  ,w.ImageUrl as WalkerImageUrl
                            	  ,w.NeighborhoodId
                            	  ,d.Id as DogId
                            	  ,d.Name as DogName
                            	  ,d.OwnerId as OwnerId
                            	  ,d.Breed
                            	  ,d.Notes
                            	  ,d.ImageUrl as DogImageUrl
                                  ,n.[Name] as NeighborhoodName
                            FROM Walker w
                            LEFT JOIN Neighborhood n on n.Id = w.NeighborhoodId
                            LEFT JOIN [Owner] o on o.NeighborhoodId = n.Id
                            LEFT JOIN Dog d on d.OwnerId = o.Id
                            WHERE w.Id = @id";

        cmd.Parameters.AddWithValue("@id", id);

        using SqlDataReader reader = cmd.ExecuteReader();
        Walker? walker = null;

        while (reader.Read())
        {
            if (walker is null)
            {
                walker = new Walker
                {
                    Id = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                    Name = reader.GetString(reader.GetOrdinal("WalkerName")),
                    ImageUrl = reader.GetString(reader.GetOrdinal("WalkerImageUrl")),
                    NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                    Neighborhood = new Neighborhood()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                        Name = reader.GetString(reader.GetOrdinal("NeighborhoodName"))
                    }
                };
            }

            if (!reader.IsDBNull(reader.GetOrdinal("DogId")))
            {
                int notesRow = reader.GetOrdinal("Notes");
                int imageRow = reader.GetOrdinal("DogImageUrl");
                Dog dog = new Dog()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("DogId")),
                    Name = reader.GetString(reader.GetOrdinal("DogName")),
                    OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                    Breed = reader.GetString(reader.GetOrdinal("Breed")),
                    Notes = reader.IsDBNull(notesRow) ? null : reader.GetString(notesRow),
                    ImageUrl = reader.IsDBNull(imageRow) ? null : reader.GetString(imageRow),
                };
                walker.Dogs.Add(dog);
            }
        }
        return walker;
    }
}
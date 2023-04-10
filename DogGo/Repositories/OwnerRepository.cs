using DogGo.Models;

namespace DogGo.Repositories;

public class OwnerRepository : BaseRepository, IOwnerRepository
{
    public OwnerRepository(IConfiguration config) : base(config)
    {
    }

    public List<Owner> GetAllOwners()
    {
        using var conn = Connection;
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT [Id]
                                  ,[Email]
                                  ,[Name]
                                  ,[Address]
                                  ,[NeighborhoodId]
                                  ,[Phone]
                            FROM [DogWalkerMVC].[dbo].[Owner]";

        using var rdr = cmd.ExecuteReader();
        List<Owner> owners = new List<Owner>();
        while (rdr.Read())
        {
            Owner owner = new Owner()
            {
                Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                Email = rdr.GetString(rdr.GetOrdinal("Email")),
                Name = rdr.GetString(rdr.GetOrdinal("Name")),
                Address = rdr.GetString(rdr.GetOrdinal("Address")),
                NeighborhoodId = rdr.IsDBNull(rdr.GetOrdinal("NeighborhoodId")) ? null : rdr.GetInt32(rdr.GetOrdinal("NeighborhoodId")),
                Phone = rdr.GetString(rdr.GetOrdinal("Phone"))
            };
            owners.Add(owner);
        }

        return owners;
    }

    public Owner? GetById(int id)
    {
        using var conn = Connection;
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT o.[Id]
                                  ,[Email]
                                  ,o.[Name]
                                  ,[Address]
                                  ,[NeighborhoodId]
                                  ,[Phone]
                            	  ,d.[Name] as DogName
                            	  ,d.Breed
                            FROM [DogWalkerMVC].[dbo].[Owner] o
                            LEFT JOIN Dog d ON d.OwnerId = o.Id
                            WHERE o.Id = @id";
        cmd.Parameters.AddWithValue("@id", id);

        using var rdr = cmd.ExecuteReader();
        Owner? owner = null;
        while (rdr.Read())
        {
            if (owner is null)
            {
                owner = new Owner()
                {
                    Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                    Email = rdr.GetString(rdr.GetOrdinal("Email")),
                    Name = rdr.GetString(rdr.GetOrdinal("Name")),
                    Address = rdr.GetString(rdr.GetOrdinal("Address")),
                    NeighborhoodId = rdr.IsDBNull(rdr.GetOrdinal("NeighborhoodId")) ? null : rdr.GetInt32(rdr.GetOrdinal("NeighborhoodId")),
                    Phone = rdr.GetString(rdr.GetOrdinal("Phone"))
                };
            }

            if (!rdr.IsDBNull(rdr.GetOrdinal("DogName")))
            {
                Dog dog = new Dog()
                {
                    Name = rdr.GetString(rdr.GetOrdinal("DogName")),
                    Breed = rdr.GetString(rdr.GetOrdinal("Breed")),
                };
                owner.Dogs.Add(dog);
            }
        }

        return owner;
    }
}

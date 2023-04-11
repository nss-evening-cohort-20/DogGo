using DogGo.Models;
using Microsoft.Data.SqlClient;

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

    public Owner? GetOwnerByEmail(string email)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();

            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                        SELECT Id, [Name], Email, Address, Phone, NeighborhoodId
                        FROM Owner
                        WHERE Email = @email";

                cmd.Parameters.AddWithValue("@email", email);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Owner owner = new Owner()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Address = reader.GetString(reader.GetOrdinal("Address")),
                        Phone = reader.GetString(reader.GetOrdinal("Phone")),
                        NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                    };

                    reader.Close();
                    return owner;
                }

                reader.Close();
                return null;
            }
        }
    }

    public void AddOwner(Owner owner)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    INSERT INTO Owner ([Name], Email, Phone, Address, NeighborhoodId)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @email, @phoneNumber, @address, @neighborhoodId);
                ";

                cmd.Parameters.AddWithValue("@name", owner.Name);
                cmd.Parameters.AddWithValue("@email", owner.Email);
                cmd.Parameters.AddWithValue("@phoneNumber", owner.Phone);
                cmd.Parameters.AddWithValue("@address", owner.Address);
                cmd.Parameters.AddWithValue("@neighborhoodId", owner.NeighborhoodId);

                int id = (int)cmd.ExecuteScalar();

                owner.Id = id;
            }
        }
    }

    public void UpdateOwner(Owner owner)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();

            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                            UPDATE Owner
                            SET 
                                [Name] = @name, 
                                Email = @email, 
                                Address = @address, 
                                Phone = @phone, 
                                NeighborhoodId = @neighborhoodId
                            WHERE Id = @id";

                cmd.Parameters.AddWithValue("@name", owner.Name);
                cmd.Parameters.AddWithValue("@email", owner.Email);
                cmd.Parameters.AddWithValue("@address", owner.Address);
                cmd.Parameters.AddWithValue("@phone", owner.Phone);
                cmd.Parameters.AddWithValue("@neighborhoodId", owner.NeighborhoodId);
                cmd.Parameters.AddWithValue("@id", owner.Id);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public void DeleteOwner(int ownerId)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();

            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                            DELETE FROM Owner
                            WHERE Id = @id
                        ";

                cmd.Parameters.AddWithValue("@id", ownerId);

                cmd.ExecuteNonQuery();
            }
        }
    }
}

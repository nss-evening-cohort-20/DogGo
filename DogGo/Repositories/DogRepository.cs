using DogGo.Models;
using Microsoft.Data.SqlClient;

namespace DogGo.Repositories;

public class DogRepository : BaseRepository, IDogRepository
{
    public DogRepository(IConfiguration config) : base(config)
    {



    }

    public List<Dog> GetAllDogs()
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                        Select Id
                        ,[Name]
                        ,OwnerId
                        ,Breed
                        ,Notes
                        ,ImageUrl
                        From Dog
                    ";

                SqlDataReader reader = cmd.ExecuteReader();

                List<Dog> dogs = new List<Dog>();
                while (reader.Read())
                {
                    Dog dog = new Dog
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                        Breed = reader.GetString(reader.GetOrdinal("Breed")),
                        Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                        ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
                    };

                    dogs.Add(dog);




                }

                reader.Close();

                return dogs;
            }
        }
    }

    public Dog? GetDogById(int id)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                        Select Id
                        ,[Name]
                        ,OwnerId
                        ,Breed
                        ,Notes
                        ,ImageUrl
                        From Dog
                        WHERE id = @id";

                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Dog dog = new Dog()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                        Breed = reader.GetString(reader.GetOrdinal("Breed")),
                        Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                        ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),

                    };

                    reader.Close();
                    return dog;
                }
                reader.Close();
                return null;
            }
        }

    }

    public void AddDog(Dog dog)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    INSERT INTO Dog ([Name],OwnerId,Breed,Notes,ImageUrl)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @OwnerId,@breed,@notes, @imageurl);
";

                cmd.Parameters.AddWithValue("@name", dog.Name);
                cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                cmd.Parameters.AddWithValue("@Breed", dog.Breed);
                cmd.Parameters.AddWithValue("@notes", dog.Notes);
                cmd.Parameters.AddWithValue("@Imageurl", dog.ImageUrl);

                int id = (int)cmd.ExecuteScalar();

                dog.Id = id;
            }
        }
    }

    public void UpdateDog(Dog dog)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();

            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                        UPDATE Dog
                        SET 
                            [Name] = @name,
                            OwnerId = @ownerid,
                            Breed = @breed,
                            Notes = @notes,
                            ImageUrl = @imageurl
                        WHERE Id = @id";

                cmd.Parameters.AddWithValue("@id", dog.Id);
                cmd.Parameters.AddWithValue("@name", dog.Name);
                cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                cmd.Parameters.AddWithValue("@Breed", dog.Breed);
                cmd.Parameters.AddWithValue("@notes", string.IsNullOrWhiteSpace(dog.Notes) ? DBNull.Value : dog.Notes);
                cmd.Parameters.AddWithValue("@imageurl", string.IsNullOrWhiteSpace(dog.ImageUrl) ? DBNull.Value : dog.ImageUrl);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public void DeleteDog(int dogId)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();

            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                            DELETE FROM Dog
                            WHERE Id = @id
                        ";

                cmd.Parameters.AddWithValue("@id", dogId);

                cmd.ExecuteNonQuery();
            }
        }
    }

}

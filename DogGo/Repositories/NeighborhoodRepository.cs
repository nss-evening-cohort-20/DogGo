using DogGo.Models;
using Microsoft.Data.SqlClient;

namespace DogGo.Repositories
{
    public class NeighborhoodRepository : BaseRepository, INeighborhoodRepository
    {
        public NeighborhoodRepository(IConfiguration config) : base(config)
        {
        }

        public List<Neighborhood> GetAll()
        {
            using SqlConnection conn = Connection;
            conn.Open();
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT Id, Name FROM Neighborhood";

            using SqlDataReader reader = cmd.ExecuteReader();

            List<Neighborhood> neighborhoods = new List<Neighborhood>();

            while (reader.Read())
            {
                Neighborhood neighborhood = new Neighborhood()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name"))
                };
                neighborhoods.Add(neighborhood);
            }

            return neighborhoods;
        }
    }
}

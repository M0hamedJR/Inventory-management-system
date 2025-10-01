using Entities.Models;
using Shared.DTO.Shelf;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ShelfRepoForSignalR
    {
        string connectionString;

        public ShelfRepoForSignalR(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<Entities.Models.Shelf> GetShelf()
        {
            List<Entities.Models.Shelf> shelfs = new List<Entities.Models.Shelf>();
            Entities.Models.Shelf shelf;

            var data = GetShelfDetailsFromDb();

            foreach (DataRow row in data.Rows)
            {
                shelf = new Entities.Models.Shelf
                {
                    Id = Guid.Parse(row["Id"].ToString()), // Convert ID to Guid
                    Name = row["Name"].ToString(),       // Convert Name to string
                    IsAvailable = bool.Parse(row["IsAvailable"].ToString())
                };

                shelfs.Add(shelf);
            }

            return shelfs;
        }


        private DataTable GetShelfDetailsFromDb()
        {
            var query = "SELECT Id,Name,IsAvailable FROM Shelfs";
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }

                    return dataTable;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }









    }
}

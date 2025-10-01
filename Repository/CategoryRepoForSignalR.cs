using Entities.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CategoryRepoForSignalR
    {
        string connectionString;

        public CategoryRepoForSignalR(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<Category> GetCategory()
        {
            List<Category> categories = new List<Category>();
            Category category;

            var data = GetCategoryDetailsFromDb();

            foreach (DataRow row in data.Rows)
            {
                category = new Category
                {
                    Id = Guid.Parse(row["Id"].ToString()), // Convert ID to Guid
                    Name = row["Name"].ToString()          // Convert Name to string

                };

                categories.Add(category);
            }

            return categories;
        }


        private DataTable GetCategoryDetailsFromDb()
        {
            var query = "SELECT Id, Name FROM Categories";
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

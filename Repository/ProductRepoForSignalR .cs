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
    public class ProductRepoForSignalR
    {
        string connectionString;

        public ProductRepoForSignalR(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<Product> GetProduct()
        {
            List<Product> categories = new List<Product>();
            Product category;

            var data = GetProductDetailsFromDb();

            foreach (DataRow row in data.Rows)
            {
                category = new Product
                {
                    Id = Guid.Parse(row["Id"].ToString()), // Convert ID to Guid
                    Name = row["Name"].ToString()          // Convert Name to string
                };
                categories.Add(category);
            }
            return categories;
        }

        private DataTable GetProductDetailsFromDb()
        {
            var query = "SELECT Id, Name,SerialNumber,CategoryId,Weight,Shipped_From,Price,Shipped_To,Shipped_Address_From,Shipped_Address_To,ShelfId,WarehouseId FROM Products";
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

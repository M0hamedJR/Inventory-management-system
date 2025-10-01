using Entities.Models;
using InventrySystem.Hubs;
using TableDependency.SqlClient;

namespace InventrySystem.SubscribeTableDependencies
{
    public class SubscribeCategoryTableDependency : ISubscribeTableDependency
    {

        SqlTableDependency<Category> tableDependency;
        DashboardHub dashboardHub;

        public SubscribeCategoryTableDependency(DashboardHub dashboardHub)
        {
            this.dashboardHub = dashboardHub;
        }


        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<Category>(connectionString, "Categories");
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Category)} SqlTableDependency error: {e.Error.Message}");
        }

        private void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Category> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                dashboardHub.SendCategories();
            }
        }


    }
}

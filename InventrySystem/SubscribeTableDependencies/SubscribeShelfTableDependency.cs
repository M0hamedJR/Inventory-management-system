using Entities.Models;
using InventrySystem.Hubs;
using TableDependency.SqlClient;

namespace InventrySystem.SubscribeTableDependencies
{
    public class SubscribeShelfTableDependency : ISubscribeTableDependency
    {

        SqlTableDependency<Shelf> tableDependency;
        DashboardHub dashboardHub;

        public SubscribeShelfTableDependency(DashboardHub dashboardHub)
        {
            this.dashboardHub = dashboardHub;
        }


        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<Shelf>(connectionString, "Shelfs");
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Shelf)} SqlTableDependency error: {e.Error.Message}");
        }

        private void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Shelf> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                dashboardHub.SendShelfves();
            }
        }


    }
}

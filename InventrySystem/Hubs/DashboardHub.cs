using Microsoft.AspNetCore.SignalR;
using Repository;

namespace InventrySystem.Hubs
{
    public class DashboardHub : Hub
    {
        CategoryRepoForSignalR categoryRepository;
        ProductRepoForSignalR productyRepository;
        ShelfRepoForSignalR shelfRepoForSignalR;

        public DashboardHub(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            categoryRepository = new CategoryRepoForSignalR(connectionString);

            productyRepository = new ProductRepoForSignalR(connectionString);
            shelfRepoForSignalR = new ShelfRepoForSignalR(connectionString);
        }

        public async Task SendCategories()
        {
            var categories = categoryRepository.GetCategory();
            await Clients.All.SendAsync("ReceivedCtegory", categories);
            




        }
        public async Task SendProducts()
        {
            var products = productyRepository.GetProduct();
            await Clients.All.SendAsync("ReceivedProduct", products);

        }
        public async Task SendShelfves()
        {
            var shelfs = shelfRepoForSignalR.GetShelf();
            await Clients.All.SendAsync("SendShelfves", shelfs);
        }

        public async Task SendDashboardData(object data)
        {
            await Clients.All.SendAsync("ReceiveDashboardData", data);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
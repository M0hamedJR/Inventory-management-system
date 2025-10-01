using InventrySystem.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace InventrySystem.Services
{
    public class DashboardService
    {
        private readonly IHubContext<DashboardHub> _hubContext;

        public DashboardService(IHubContext<DashboardHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task BroadcastDashboardUpdate(object dashboardData)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveDashboardUpdate", dashboardData);
        }
    }
}
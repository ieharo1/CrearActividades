using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EnterpriseMediaVault.API.Hubs;

[Authorize]
public sealed class NotificationHub : Hub
{
}

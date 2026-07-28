using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Middleware;

public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICurrentTenantService tenantService, ICurrentUserService userService, IApplicationDbContext dbContext)
    {
        if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdHeader) &&
            Guid.TryParse(tenantIdHeader.ToString(), out var tenantId))
        {
            tenantService.TenantId = tenantId;

            if (userService.IsAuthenticated && userService.UserId.HasValue)
            {
                var membership = await dbContext.OrganizationMembers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.OrganizationId == tenantId && m.UserId == userService.UserId.Value);

                if (membership != null)
                {
                    tenantService.UserRole = membership.Role;
                }
            }
        }

        await _next(context);
    }
}

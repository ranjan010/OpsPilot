using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ICurrentTenantService _currentTenantService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentTenantService currentTenantService) : base(options)
    {
        _currentTenantService = currentTenantService;
    }

    public DbSet<Users> Users => Set<Users>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<OrganizationMember> OrganizationMembers => Set<OrganizationMember>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<TicketComment> TicketComments => Set<TicketComment>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<TicketTag> TicketTags => Set<TicketTag>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Composite Key for TicketTag
        modelBuilder.Entity<TicketTag>()
            .HasKey(tt => new { tt.TicketId, tt.TagId });

        modelBuilder.Entity<TicketTag>()
            .HasOne(tt => tt.Ticket)
            .WithMany(t => t.TicketTags)
            .HasForeignKey(tt => tt.TicketId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TicketTag>()
            .HasOne(tt => tt.Tag)
            .WithMany(t => t.TicketTags)
            .HasForeignKey(tt => tt.TagId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ticket Relationships (Preventing Multiple Cascade Paths in SQL Server)
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Organization)
            .WithMany(o => o.Tickets)
            .HasForeignKey(t => t.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tickets)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Assignee)
            .WithMany()
            .HasForeignKey(t => t.AssigneeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Reporter)
            .WithMany()
            .HasForeignKey(t => t.ReporterId)
            .OnDelete(DeleteBehavior.Restrict);

        // Project Relationships
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Organization)
            .WithMany(o => o.Projects)
            .HasForeignKey(p => p.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);

        // TicketComment Relationships
        modelBuilder.Entity<TicketComment>()
            .HasOne(tc => tc.Organization)
            .WithMany()
            .HasForeignKey(tc => tc.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TicketComment>()
            .HasOne(tc => tc.Ticket)
            .WithMany(t => t.Comments)
            .HasForeignKey(tc => tc.TicketId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TicketComment>()
            .HasOne(tc => tc.Author)
            .WithMany()
            .HasForeignKey(tc => tc.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Tag Relationships
        modelBuilder.Entity<Tag>()
            .HasOne(t => t.Organization)
            .WithMany(o => o.Tags)
            .HasForeignKey(t => t.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);

        // AuditLog Relationships
        modelBuilder.Entity<AuditLog>()
            .HasOne(a => a.Organization)
            .WithMany()
            .HasForeignKey(a => a.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AuditLog>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // OrganizationMember Relationships
        modelBuilder.Entity<OrganizationMember>()
            .HasOne(om => om.User)
            .WithMany(u => u.OrganizationMemberships)
            .HasForeignKey(om => om.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrganizationMember>()
            .HasOne(om => om.Organization)
            .WithMany(o => o.Members)
            .HasForeignKey(om => om.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);

        // RefreshToken Relationships
        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Tenant Global Query Filters
        modelBuilder.Entity<Project>()
            .HasQueryFilter(p => !_currentTenantService.HasTenant || p.OrganizationId == _currentTenantService.TenantId);

        modelBuilder.Entity<Ticket>()
            .HasQueryFilter(t => !_currentTenantService.HasTenant || t.OrganizationId == _currentTenantService.TenantId);

        modelBuilder.Entity<TicketComment>()
            .HasQueryFilter(tc => !_currentTenantService.HasTenant || tc.OrganizationId == _currentTenantService.TenantId);

        modelBuilder.Entity<Tag>()
            .HasQueryFilter(t => !_currentTenantService.HasTenant || t.OrganizationId == _currentTenantService.TenantId);

        modelBuilder.Entity<AuditLog>()
            .HasQueryFilter(a => !_currentTenantService.HasTenant || a.OrganizationId == _currentTenantService.TenantId);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is ITenantScoped tenantScoped && entry.State == EntityState.Added)
            {
                if (tenantScoped.OrganizationId == Guid.Empty && _currentTenantService.HasTenant)
                {
                    tenantScoped.OrganizationId = _currentTenantService.TenantId!.Value;
                }
            }

            if (entry.Entity is BaseEntity baseEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    if (baseEntity.CreatedAt == default)
                        baseEntity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    baseEntity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
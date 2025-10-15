using Microsoft.EntityFrameworkCore;
using CRM.Domain.Entities;
using CRM.Domain.Common;

namespace CRM.Infrastructure.Data
{
      public class ApplicationDbContext : DbContext
      {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {
            }

            // DbSets para Seguridad
            public DbSet<User> Users => Set<User>();
            public DbSet<Role> Roles => Set<Role>();
            public DbSet<Permission> Permissions => Set<Permission>();
            public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

            // DbSets para Núcleo CRM
            public DbSet<Lead> Leads => Set<Lead>();
            public DbSet<Contact> Contacts => Set<Contact>();
            public DbSet<Account> Accounts => Set<Account>();
            public DbSet<Industry> Industries => Set<Industry>();
            public DbSet<LeadSource> LeadSources => Set<LeadSource>();
            public DbSet<Opportunity> Opportunities => Set<Opportunity>();

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                  base.OnModelCreating(modelBuilder);

                  modelBuilder.Entity<Role>().ToTable("Roles");
                  modelBuilder.Entity<User>().ToTable("Users"); 
                  modelBuilder.Entity<Permission>().ToTable("Permissions");
                  modelBuilder.Entity<RolePermission>().ToTable("RolePermissions");
                  modelBuilder.Entity<Account>().ToTable("Accounts");
                  modelBuilder.Entity<Contact>().ToTable("Contacts");
                  modelBuilder.Entity<Lead>().ToTable("Leads");
                  modelBuilder.Entity<Industry>().ToTable("Industries");
                  modelBuilder.Entity<LeadSource>().ToTable("LeadSources");
                  modelBuilder.Entity<Opportunity>().ToTable("Opportunities");

                  // Configuración de User
                  modelBuilder.Entity<User>(entity =>
                  {
                        entity.HasIndex(u => u.Username).IsUnique();
                        entity.HasIndex(u => u.Email).IsUnique();

                        // Relación con Role
                        entity.HasOne(u => u.Role)
                        .WithMany(r => r.Users)
                        .HasForeignKey(u => u.RoleId)
                        .OnDelete(DeleteBehavior.Restrict);
                  });

                  // Configuración de Role
                  modelBuilder.Entity<Role>(entity =>
                  {
                        entity.ToTable("roles");
                        entity.HasIndex(r => r.Name).IsUnique();
                  });

                  // Configuración de Permission
                  modelBuilder.Entity<Permission>(entity =>
                  {
                        entity.HasIndex(p => new { p.Module, p.Action }).IsUnique();
                  });

                  // Configuración de RolePermission
                  modelBuilder.Entity<RolePermission>(entity =>
                  {
                        entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });

                        entity.HasOne(rp => rp.Role)
                        .WithMany(r => r.RolePermissions)
                        .HasForeignKey(rp => rp.RoleId);

                        entity.HasOne(rp => rp.Permission)
                        .WithMany(p => p.RolePermissions)
                        .HasForeignKey(rp => rp.PermissionId);
                  });

                  // Configuración de Account
                  modelBuilder.Entity<Account>(entity =>
                  {
                        // Relaciones
                        entity.HasOne(a => a.Industry)
                        .WithMany(i => i.Accounts)
                        .HasForeignKey(a => a.IndustryId)
                        .OnDelete(DeleteBehavior.SetNull);

                        entity.HasOne(a => a.AssignedToUser)
                        .WithMany(u => u.Accounts)
                        .HasForeignKey(a => a.AssignedToUserId)
                        .OnDelete(DeleteBehavior.SetNull);

                        entity.HasOne(a => a.CreatedByUser)
                        .WithMany()
                        .HasForeignKey(a => a.CreatedByUserId)
                        .OnDelete(DeleteBehavior.Restrict);
                  });

                  // Configuración de Contact
                  modelBuilder.Entity<Contact>(entity =>
                  {
                        // Relaciones
                        entity.HasOne(c => c.Account)
                        .WithMany(a => a.Contacts)
                        .HasForeignKey(c => c.AccountId)
                        .OnDelete(DeleteBehavior.SetNull);

                        entity.HasOne(c => c.AssignedToUser)
                        .WithMany(u => u.Contacts)
                        .HasForeignKey(c => c.AssignedToUserId)
                        .OnDelete(DeleteBehavior.SetNull);

                        entity.HasOne(c => c.CreatedByUser)
                        .WithMany()
                        .HasForeignKey(c => c.CreatedByUserId)
                        .OnDelete(DeleteBehavior.Restrict);
                  });

                  // Configuración de Lead
                  modelBuilder.Entity<Lead>(entity =>
                  {
                        // Relaciones
                        entity.HasOne(l => l.LeadSource)
                        .WithMany(ls => ls.Leads)
                        .HasForeignKey(l => l.LeadSourceId)
                        .OnDelete(DeleteBehavior.SetNull);

                        entity.HasOne(l => l.Industry)
                        .WithMany(i => i.Leads)
                        .HasForeignKey(l => l.IndustryId)
                        .OnDelete(DeleteBehavior.SetNull);

                        entity.HasOne(l => l.AssignedToUser)
                        .WithMany(u => u.Leads)
                        .HasForeignKey(l => l.AssignedToUserId)
                        .OnDelete(DeleteBehavior.SetNull);

                        entity.HasOne(l => l.CreatedByUser)
                        .WithMany()
                        .HasForeignKey(l => l.CreatedByUserId)
                        .OnDelete(DeleteBehavior.Restrict);
                  });

                  // Configuración de Industry
                  modelBuilder.Entity<Industry>(entity =>
                  {
                        entity.HasIndex(i => i.Name).IsUnique();
                  });

                  // Configuración de LeadSource
                  modelBuilder.Entity<LeadSource>(entity =>
                  {
                        entity.HasIndex(ls => ls.Name).IsUnique();
                  });

                  // Configuración de Opportunity
                  modelBuilder.Entity<Opportunity>(entity =>
                  {
                        // Relaciones
                        entity.HasOne(o => o.Account)
                        .WithMany(a => a.Opportunities)
                        .HasForeignKey(o => o.AccountId)
                        .OnDelete(DeleteBehavior.Restrict);

                        entity.HasOne(o => o.Contact)
                        .WithMany()
                        .HasForeignKey(o => o.ContactId)
                        .OnDelete(DeleteBehavior.SetNull);

                        entity.HasOne(o => o.AssignedToUser)
                        .WithMany()
                        .HasForeignKey(o => o.AssignedToUserId)
                        .OnDelete(DeleteBehavior.Restrict);

                        entity.HasOne(o => o.CreatedByUser)
                        .WithMany()
                        .HasForeignKey(o => o.CreatedByUserId)
                        .OnDelete(DeleteBehavior.Restrict);
                  });
            }

            public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            {
                  // Actualizar ModifiedAt antes de guardar
                  var entries = ChangeTracker.Entries<BaseEntity>();

                  foreach (var entry in entries)
                  {
                        if (entry.State == EntityState.Modified)
                        {
                              entry.Entity.ModifiedAt = DateTime.UtcNow;
                        }
                  }

                  return await base.SaveChangesAsync(cancellationToken);
            }
      }
}
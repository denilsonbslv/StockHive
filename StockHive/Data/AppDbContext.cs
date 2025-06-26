using Microsoft.EntityFrameworkCore;
using StockHive.Interfaces;
using StockHive.Models;

namespace StockHive.Data
{
    /// <summary>
    /// Contexto principal de acesso ao banco de dados, gerenciando entidades e configurações.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Construtor que recebe opções de configuração do contexto.
        /// </summary>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Fornecedores cadastrados.
        /// </summary>
        public DbSet<Supplier> Suppliers { get; set; }

        /// <summary>
        /// Locais de armazenamento.
        /// </summary>
        public DbSet<Location> Locations { get; set; }

        /// <summary>
        /// Categorias de produtos.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Produtos cadastrados.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Atributos de produtos.
        /// </summary>
        public DbSet<ProductAttribute> ProductAttributes { get; set; }

        /// <summary>
        /// Inventário de produtos por local.
        /// </summary>
        public DbSet<Inventory> Inventories { get; set; }

        /// <summary>
        /// Movimentações de estoque registradas.
        /// </summary>
        public DbSet<StockMovement> StockMovements { get; set; }

        /// <summary>
        /// Aplica filtros globais para soft delete e configura relacionamentos.
        /// </summary>
        /// <param name="modelBuilder">Construtor de modelos do EF Core.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasQueryFilter(p => p.DeletedAt == null);
            modelBuilder.Entity<Supplier>().HasQueryFilter(s => s.DeletedAt == null);
            modelBuilder.Entity<Category>().HasQueryFilter(c => c.DeletedAt == null);
            modelBuilder.Entity<ProductAttribute>().HasQueryFilter(pa => pa.DeletedAt == null);
            modelBuilder.Entity<Location>().HasQueryFilter(l => l.DeletedAt == null);

            modelBuilder.Entity<Inventory>().HasQueryFilter(i => i.Location.DeletedAt == null);

            modelBuilder.Entity<StockMovement>().HasQueryFilter(sm =>
                sm.Product.DeletedAt == null &&
                (sm.FromLocation == null || sm.FromLocation.DeletedAt == null) &&
                (sm.ToLocation == null || sm.ToLocation.DeletedAt == null));
        }

        /// <summary>
        /// Salva alterações definindo automaticamente campos de auditoria (CreatedAt, UpdatedAt).
        /// </summary>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Número de entidades afetadas.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries<IAuditable>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                entityEntry.Entity.UpdatedAt = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Entity.CreatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
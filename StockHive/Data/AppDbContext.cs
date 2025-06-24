using Microsoft.EntityFrameworkCore;
using StockHive.Interfaces;
using StockHive.Models;

namespace StockHive.Data
{
    /// <summary>
    /// Representa o contexto do banco de dados principal da aplicação, gerenciando as entidades e suas configurações.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Inicializa uma nova instância de <see cref="AppDbContext"/> com as opções especificadas.
        /// </summary>
        /// <param name="options">Opções de configuração do contexto.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Tabela de fornecedores.
        /// </summary>
        public DbSet<Supplier> Suppliers { get; set; }

        /// <summary>
        /// Tabela de locais de armazenamento.
        /// </summary>
        public DbSet<Location> Locations { get; set; }

        /// <summary>
        /// Tabela de categorias de produtos.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Tabela de produtos.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Tabela de atributos de produtos.
        /// </summary>
        public DbSet<ProductAttribute> ProductAttributes { get; set; }

        /// <summary>
        /// Tabela de inventário.
        /// </summary>
        public DbSet<Inventory> Inventories { get; set; }

        /// <summary>
        /// Tabela de movimentações de estoque.
        /// </summary>
        public DbSet<StockMovement> StockMovements { get; set; }

        /// <summary>
        /// Configurações avançadas do modelo, incluindo filtros globais para soft delete e relacionamentos.
        /// </summary>
        /// <param name="modelBuilder">Construtor de modelos do Entity Framework.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Filtros Globais para as entidades principais (soft delete)
            modelBuilder.Entity<Product>().HasQueryFilter(p => p.DeletedAt == null);
            modelBuilder.Entity<Supplier>().HasQueryFilter(s => s.DeletedAt == null);
            modelBuilder.Entity<Category>().HasQueryFilter(c => c.DeletedAt == null);
            modelBuilder.Entity<ProductAttribute>().HasQueryFilter(pa => pa.DeletedAt == null);
            modelBuilder.Entity<Location>().HasQueryFilter(l => l.DeletedAt == null);

            // Filtros correspondentes para as entidades dependentes
            modelBuilder.Entity<Inventory>().HasQueryFilter(i => i.Location.DeletedAt == null);

            modelBuilder.Entity<StockMovement>().HasQueryFilter(sm =>
                sm.Product.DeletedAt == null &&
                (sm.FromLocation == null || sm.FromLocation.DeletedAt == null) &&
                (sm.ToLocation == null || sm.ToLocation.DeletedAt == null));
        }

        /// <summary>
        /// Salva as alterações no banco de dados, atualizando automaticamente os campos de auditoria.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Número de entidades afetadas.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Pega todas as entidades que estão sendo Adicionadas ou Modificadas
            var entries = ChangeTracker
                .Entries<IAuditable>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                // Define a data de atualização para todas as entradas (novas ou modificadas).
                entityEntry.Entity.UpdatedAt = DateTime.UtcNow;

                // Se a entidade está sendo ADICIONADA, define a data de criação.
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Entity.CreatedAt = DateTime.UtcNow;
                }
            }

            // Chama o método original para efetivamente salvar os dados no banco
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
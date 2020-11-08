using System.Linq;
using app.Models;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Context
{
    public class MeuDbContext : DbContext
    {

        public MeuDbContext(DbContextOptions<MeuDbContext> options) : base(options) { }

        // Adicionamos o mapeamento das entidades para o context permitindo assim as operações pelo EF
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }

        // - Registro do mappings das entidades para o DbContext -
        // -> Método que será chamado durante a criação dos models no db
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // - Não deixamos que uma propriedade seja "VARCHAR(MAX)" Caso esqueçamos de mapear alguma propriedade em "EntityNameMapping.cs" -
            foreach (var property in modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                 .Where(p => p.ClrType == typeof(string))))
            {
                // -> property.Relational ().ColumnType = "varchar(100)"; // NET CORE 2
                property.SetColumnType("varchar(100)"); // NET CORE 3
            }

            // - Pega o MeuDbContext, Busca todas as entidades que estão mapeadas no context (por DbSet<NomeEntidade>) e depois busca as entidades mappings para cada MeuDbContext (ProdutoMapping.cs etc que herdam da interface: IEntityTypeConfiguration<>): E fará o registro de uma vez só dos mappings dessas entidades! -
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MeuDbContext).Assembly);

            // - Desabilitando o "CASCADE DELETE" -
            // -> Não é recomendado deixar as classes mapeadas com o "CASCADE DELETE" ativada, então vamos desativar!
            // -> Ex: 1 fornecedor possui n produtos, ao excluir um fornecedor com o "CASCADE DELETE" ativado, ao remover 1 fornecedor excluímos vários produtos juntos!
            foreach (var relationship in modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                // relationship.DeleteBehavior = DeleteBehavior.Cascade;
                // relationship.DeleteBehavior = DeleteBehavior.Restrict;
                // relationship.DeleteBehavior = DeleteBehavior.SetNull;
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

            base.OnModelCreating(modelBuilder);
        }

    }
}

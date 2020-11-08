using app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/*
- FLUENTE API -
-> Faremos o mapeamento das entidades com a fluente API (tamanho coluna, tipo campo etc) para o banco de dados
-> Dessa forma não precisamos poluir as "models" com "data annotations" | Forma ideal de mapeamento de aplicação com camadas!

- OBS -
-> Faremos o mapeamento somente do propriedades "STRINGS" pois é importante definirmos tamanhos adequados para esses campos nas tabelas do db
-> Os campos int, decimal, date, e boolean não vamos mapear pois o próprio EF vai defini-los do tamanho correto!
-> Na classe FornecedorMapping definimos os relacionamento 1:1 e 1:n necessários | O EF já faz isso sozinho, mas mesmo assim o implementamos!
-> Sobre mapeamento de relacionamentos de entidades: A classe que possui filhas é que configuramos o mapeamento de relacionamentos!
 */

namespace DevIO.Data.Mappings
{
    /* MAPEAMENTO ENTIDADE FORNECEDOR */
    public class FornecedorMapping : IEntityTypeConfiguration<Fornecedor>
    {

        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            // Faremos o mapeamento através do parâmetro: builder
            //
            // Mapeamento das colunas
            //
            // Mapeamento da FK de Fornecedores
            builder.HasKey(p => p.Id);
            // Mapeamento da propriedade Nome de Fornecesores
            builder.Property(p => p.Nome).IsRequired().HasColumnType("varchar(200)");
            // Mapeamento da propriedade Documento de Fornecesores
            builder.Property(p => p.Documento).IsRequired().HasColumnType("varchar(14)");
            // Definimos o relacionamento 1 : 1 de fornecedor para endereço
            builder.HasOne(f => f.Endereco).WithOne(e => e.Fornecedor);
            // Definimos o relacionamento 1 : n onde fornecedor tem vários produtos
            builder.HasMany(f => f.Produtos).WithOne(p => p.Fornecedor).HasForeignKey(p => p.FornecedorId);
            // Definimos o nome da tabela
            builder.ToTable("Fornecedores");
        }
    }
}

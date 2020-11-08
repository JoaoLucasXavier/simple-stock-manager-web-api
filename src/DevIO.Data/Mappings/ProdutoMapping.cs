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
 */

namespace DevIO.Data.Mappings
{

    /* MAPEAMENTO ENTIDADE PRODUTO */
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {

        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            // Faremos o mapeamento através do parâmetro: builder
            //
            // Mapeamento das colunas
            //
            // Mapeamento da FK de Produtos
            builder.HasKey(p => p.Id);
            // Mapeamento da propriedade Nome de Produto
            builder.Property(p => p.Nome).IsRequired().HasColumnType("varchar(200)");
            // Mapeamento da propriedade Descricao de Produto
            builder.Property(p => p.Descricao).IsRequired().HasColumnType("varchar(1000)");
            // Mapeamento da propriedade Imagem de Produto
            builder.Property(p => p.Imagem).IsRequired().HasColumnType("varchar(100)");
            // Mapeamento da propriedade Valor de Produto
            builder.Property(p => p.Valor).IsRequired().HasColumnType("decimal");
            //  Propriedade DataCadastro não precisa mapear
            //  Propriedade Ativo não precisa mapear
            //
            // Definimos o nome da tabela
            builder.ToTable("Produtos");
        }
    }
}

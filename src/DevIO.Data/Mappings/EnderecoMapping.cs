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
    /* MAPEAMENTO ENTIDADE ENDERECO */
    public class EnderecoMapping : IEntityTypeConfiguration<Endereco>
    {

        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            // Faremos o mapeamento através do parâmetro: builder
            //
            // Mapeamento das colunas
            //
            // Mapeamento da FK de Enderecos
            builder.HasKey(p => p.Id);
            // Mapeamento da propriedade Logradouro de Enderecos
            builder.Property(p => p.Logradouro).IsRequired().HasColumnType("varchar(200)");
            // Mapeamento da propriedade Numero de Enderecos
            builder.Property(p => p.Numero).IsRequired().HasColumnType("varchar(50)");
            // Mapeamento da propriedade Cep de Enderecos
            builder.Property(p => p.Cep).IsRequired().HasColumnType("varchar(8)");
            // Mapeamento da propriedade Complemento de Enderecos
            builder.Property(p => p.Complemento).IsRequired().HasColumnType("varchar(250)");
            // Mapeamento da propriedade Bairro de Enderecos
            builder.Property(p => p.Bairro).IsRequired().HasColumnType("varchar(100)");
            // Mapeamento da propriedade Cidade de Enderecos
            builder.Property(p => p.Cidade).IsRequired().HasColumnType("varchar(100)");
            // Mapeamento da propriedade Estado de Enderecos
            builder.Property(p => p.Estado).IsRequired().HasColumnType("varchar(50)");
            // Definimos o nome da tabela
            builder.ToTable("Enderecos");
        }
    }
}

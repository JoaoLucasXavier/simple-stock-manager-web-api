using System;
using System.Threading.Tasks;
using app.Models;
using DevIO.Business.Interfaces;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Repository
{
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {

        // Passamos o context para o construtor da classe base: Repository.cs
        public EnderecoRepository(MeuDbContext context) : base(context) { }

        public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId)
        {
            // AsNoTracking: Retorna a consulta do banco mas sem o Tracking/Rastreio o que nos proporciona mais performanse
            // FirstOrDefaultAsync: Retorna apenas 1 registro
            return await Db.Enderecos
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.FornecedorId == fornecedorId);
        }
    }
}

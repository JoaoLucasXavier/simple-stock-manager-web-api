using System;
using System.Threading.Tasks;
using app.Models;
using DevIO.Business.Interfaces;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {

        // Passamos o context para o construtor da classe base: Repository.cs
        public FornecedorRepository(MeuDbContext context) : base(context) { }

        public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
        {
            // AsNoTracking: Retorna a consulta do banco mas sem o Tracking/Rastreio o que nos proporciona mais performanse
            // Include : Faz o inner join com fornecedor
            // FirstOrDefaultAsync: Retorna apenas 1 registro
            return await Db.Fornecedores.AsNoTracking()
                .Include(c => c.Endereco)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking()
                // AsNoTracking: Retorna a consulta do banco mas sem o Tracking/Rastreio o que nos proporciona mais performanse
                // Include : Faz o inner join com fornecedor
                // FirstOrDefaultAsync: Retorna apenas 1 registro
                .Include(c => c.Produtos)
                .Include(c => c.Endereco)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}

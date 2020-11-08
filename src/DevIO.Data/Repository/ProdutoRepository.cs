using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Models;
using DevIO.Business.Interfaces;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {

        // Passamos o context para o construtor da classe base: Repository.cs
        public ProdutoRepository(MeuDbContext context) : base(context) { }

        public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId)
        {
            return await Buscar(p => p.FornecedorId == fornecedorId);
        }

        public async Task<IEnumerable<Produto>> ObterProdutosFornecedores()
        {
            // AsNoTracking: Retorna a consulta do banco mas sem o Tracking/Rastreio o que nos proporciona mais performanse
            // Include : Faz o inner join com fornecedor
            // OrderBy: Faz a ordenação pelo nome do produto
            return await Db.Produtos.AsNoTracking()
                .Include(f => f.Fornecedor)
                .OrderBy(p => p.Nome).ToListAsync();
        }

        public async Task<Produto> ObterProdutoFornecedor(Guid id)
        {
            // AsNoTracking: Retorna a consulta do banco mas sem o Tracking/Rastreio o que nos proporciona mais performanse
            //Include : Faz o inner join com fornecedor
            // FirstOrDefaultAsync: Retorna apenas 1 registro
            return await Db.Produtos.AsNoTracking()
                .Include(f => f.Fornecedor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}

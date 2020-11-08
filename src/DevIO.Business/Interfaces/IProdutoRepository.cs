using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using app.Models;

namespace DevIO.Business.Interfaces
{
    // OBS: Essa interface IProdutoRepository estende todos os métodos de IRepository
    public interface IProdutoRepository : IRepository<Produto>
    {

        // Criaremos métodos especializados para interface IProdutoRepository
        Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId);
        Task<IEnumerable<Produto>> ObterProdutosFornecedores();
        Task<Produto> ObterProdutoFornecedor(Guid id);
    }
}

using System;
using System.Threading.Tasks;
using app.Models;

namespace DevIO.Business.Interfaces
{
    // OBS: Essa interface IFornecedorRepository estende todos os métodos de IRepository
    public interface IFornecedorRepository : IRepository<Fornecedor>
    {

        // Criaremos métodos especializados para interface IFornecedorRepository
        Task<Fornecedor> ObterFornecedorEndereco(Guid id);
        Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id);
    }
}

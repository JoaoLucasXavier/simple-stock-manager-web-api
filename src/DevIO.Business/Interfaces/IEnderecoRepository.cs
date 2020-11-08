using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using app.Models;

namespace DevIO.Business.Interfaces
{
    // OBS: Essa interface IEnderecoRepository estende todos os métodos de IRepository
    public interface IEnderecoRepository : IRepository<Endereco>
    {

        // Criaremos métodos especializados para interface IEnderecoRepository
        Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId);
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using app.Models;
using DevIO.Business.Interfaces;
using DevIO.Business.Models.Validations;

//  Class service com seus métodos com suas respectivas resgras de negócios de validação que implementa IFornecedorService

namespace DevIO.Business.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {

        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public FornecedorService(IFornecedorRepository fornecedorRepository,
            IEnderecoRepository enderecoRepository,
            INotificador notificador) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepository = enderecoRepository;
        }

        public async Task<bool> Adicionar(Fornecedor fornecedor)
        {
            // Validar o estado da entidade
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor) ||
                !ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco)) return false;
            // Validar se não existe fornecedor com o mesmo documento
            if (_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento).Result.Any())
            {
                Notificar("Já existe um fornecedor com este documento informado.");
                return false;
            }
            await _fornecedorRepository.Adicionar(fornecedor);
            return true;
        }

        public async Task<bool> Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)) return false;

            if (_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
            {
                Notificar("Já existe um fornecedor com este documento informado.");
                return false;
            }
            await _fornecedorRepository.Atualizar(fornecedor);
            return true;
        }

        public async Task AtualizarEndereco(Endereco endereco)
        {
            if (!ExecutarValidacao(new EnderecoValidation(), endereco)) return;
            await _enderecoRepository.Atualizar(endereco);
        }

        public async Task<bool> Remover(Guid id)
        {
            if (_fornecedorRepository.ObterFornecedorProdutosEndereco(id).Result.Produtos.Any())
            {
                Notificar("O fornecedor possui produtos cadastrados!");
                return false;
            }
            var endereco = await _enderecoRepository.ObterEnderecoPorFornecedor(id);
            if (endereco != null)
            {
                await _enderecoRepository.Remover(endereco.Id);
            }
            await _fornecedorRepository.Remover(id);
            return true;
        }

        // "Disposable" | Implementar o método Dispose é principalmente para liberar recursos não gerenciados. Ao trabalhar com membros de instância que são implementações IDisposable , é comum fazer chamadas Dispose em cascata . Existem razões adicionais para implementar Dispose , por exemplo, para liberar memória que foi alocada, remover um item que foi adicionado a uma coleção ou sinalizar a liberação de um bloqueio que foi adquirido.
        // https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
        // ? | Valida se é nulo ou não | Se _fornecedorRepository ou _enderecoRepository for null não teremos um NullHasReferenceException

        public void Dispose()
        {
            _fornecedorRepository?.Dispose();
            _enderecoRepository?.Dispose();
        }
    }
}

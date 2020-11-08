using System;
using System.Threading.Tasks;
using app.Models;
using DevIO.Business.Interfaces;
using DevIO.Business.Models.Validations;

//  Class service com seus métodos com suas respectivas resgras de negócios de validação que implementa IProdutoService

namespace DevIO.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {

        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository,
            INotificador notificador) : base(notificador)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task Adicionar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;
            await _produtoRepository.Adicionar(produto);
        }

        public async Task Atualizar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;
            await _produtoRepository.Atualizar(produto);
        }

        public async Task Remover(Guid id)
        {
            await _produtoRepository.Remover(id);
        }

        // "Disposable" | Implementar o método Dispose é principalmente para liberar recursos não gerenciados. Ao trabalhar com membros de instância que são implementações IDisposable , é comum fazer chamadas Dispose em cascata . Existem razões adicionais para implementar Dispose , por exemplo, para liberar memória que foi alocada, remover um item que foi adicionado a uma coleção ou sinalizar a liberação de um bloqueio que foi adquirido.
        // https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
        // ? | Valida se é nulo ou não | Se _produtoRepository for null não teremos um NullHasReferenceException
        public void Dispose()
        {
            _produtoRepository?.Dispose();
        }
    }
}

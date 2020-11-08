using System;
using System.Threading.Tasks;
using app.Models;

namespace DevIO.Business.Interfaces
{
    // As classes "Services" são classes que possuem métodos que modificam o estado da entidade no banco
    // Para isso vamos implementar métododos que são obrigatórios passar por um "Service" para fazer alguma coisa que modifique uma entidade no db
    // "Disposable" | Implementar o método Dispose é principalmente para liberar recursos não gerenciados. Ao trabalhar com membros de instância que são implementações IDisposable , é comum fazer chamadas Dispose em cascata . Existem razões adicionais para implementar Dispose , por exemplo, para liberar memória que foi alocada, remover um item que foi adicionado a uma coleção ou sinalizar a liberação de um bloqueio que foi adquirido.
    // https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose

    public interface IProdutoService : IDisposable
    {
        Task Adicionar(Produto produto);
        Task Atualizar(Produto produto);
        Task Remover(Guid id);
    }
}

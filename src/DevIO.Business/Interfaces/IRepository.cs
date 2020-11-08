using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using app.Models;

namespace DevIO.Business.Interfaces
{
    // As classes "Repository" são classes que possuem métodos que acessam o estado da entidade no banco (Obs: Não devem modificar, para isso tem as classes "Services")

    // <TEntity>: Representa qualquer entity
    // IDisposable: Obriga que essa interface faça o disposable para liberar memória
    // Where: "TEntity" só pode ser utilizada ser for uma filha de "Entity" de app.Models
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task Adicionar(TEntity entity);
        Task<TEntity> ObterPorId(Guid id);
        Task<List<TEntity>> ObterTodos();
        Task Atualizar(TEntity entity);
        Task Remover(Guid id);
        Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate); // Método que recebe uma expressão lambda, que vai trabalhar com uma function, que vai comparar a TEntity com alguma coisa desde que ela retorna um boolean e que nos chamamos de predicate!
        Task<int> SaveChanges(); // Retorna sempre um int que é o número de linhas afetadas
    }
}

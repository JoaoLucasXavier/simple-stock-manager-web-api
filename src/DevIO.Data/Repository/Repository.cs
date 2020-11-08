using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using app.Models;
using DevIO.Business.Interfaces;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Repository
{

    // Repository<TEntity>: Representa qualquer entity
    // IRepository<TEntity>: Implementa a interface /DevIO.Business/Interfaces/IRepository.cs de "TEntity" sendo a "TEntity" filha de Entity" /DevIO.Business/models/Entity.cs
    // Where: "TEntity" só pode ser utilizada ser for uma filha de "Entity" de /DevIO.Business/models/Entity.cs
    // new(): Permite fazer a instância de um objeto de "Entity"
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {

        // Obtém acesso ao DbContex
        protected readonly MeuDbContext Db;

        // Atalho para o DbSet
        protected readonly DbSet<TEntity> DbSet;

        // Injeção Db | Configuração DbSet onde adicionamos as entidades ao contexto
        public Repository(MeuDbContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        /* REALIZAR MODIFICACOES NECESSÁRIA NOS MÉTODOS DO REPOSITORY */

        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            // AsNoTracking: Retorna a consulta do banco mas sem o Tracking/Rastreio o que nos proporciona mais performanse
            // Param predicate: Expressão
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> ObterPorId(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task Adicionar(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChanges();
        }

        public virtual async Task Atualizar(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChanges();
        }

        public virtual async Task Remover(Guid id)
        {
            // DbSet.Remove (await DbSet.FindAsync (id)); // 1° forma
            DbSet.Remove(new TEntity { Id = id }); // 2° forma | OBS: Para 2° forma funcionar temos instânciar/new() em Entity |  public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new () {}
            await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db?.Dispose(); // Db? : Se existir faz o dispose
        }
    }
}

using System;
using System.Threading.Tasks;

namespace ConsoleApp1.UoW
{
    public interface IUnitOfWorkBuilder : ITransactional
    {
        IUnitOfWorkBuilder AddStart(Func<Task> unit, Func<Task>? rollback = null);
        IUnitOfWorkBuilder AddStart<T>(Func<Task<T>> unit, Func<T, Task>? rollback = null) where T : class;
        IUnitOfWorkBuilder AddStart(UnitOfWork unit);
        IUnitOfWorkBuilder Add(Func<Task> unit, Func<Task>? rollback = null);
        IUnitOfWorkBuilder Add<T>(Func<Task<T>> unit, Func<T, Task>? rollback = null) where T : class;
        IUnitOfWorkBuilder Add(UnitOfWork unit);
        Task<T> AddImmediateAsync<T>(Func<Task<T>> unit, Func<T, Task>? rollback = null) where T : class;
        IUnitOfWorkBuilder AddEnd(Func<Task> unit, Func<Task>? rollback = null);
        IUnitOfWorkBuilder AddEnd<T>(Func<Task<T>> unit, Func<T, Task>? rollback = null) where T : class;
        IUnitOfWorkBuilder AddEnd(UnitOfWork unit);
        void Subscribe(params ITransactional[] transactionals);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanzaiTransactionalDemo.UoW
{
    public class UnitOfWorkBuilder : IUnitOfWorkBuilder
    {
        protected readonly List<UnitOfWork> Actions = new List<UnitOfWork>();
        protected readonly List<UnitOfWork> OnEndActions = new List<UnitOfWork>();
        protected readonly List<UnitOfWork> OnStartActions = new List<UnitOfWork>();

        protected virtual IEnumerable<UnitOfWork> WaitingActions => OnStartActions.Concat(Actions).Concat(OnEndActions)
            .Where(uow => uow.Status == UnitOfWorkStatus.Waiting);

        protected virtual IEnumerable<UnitOfWork> RunActions => OnStartActions.Concat(Actions).Concat(OnEndActions)
            .Where(uow => uow.Status == UnitOfWorkStatus.Run);

        public virtual async Task StartAsync() {
            foreach (var uow in OnStartActions.Where(uow => uow.Status == UnitOfWorkStatus.Waiting)) {
                uow.Status = UnitOfWorkStatus.Running;
                var result = await uow.Action();
                uow.Output = result;
                uow.Status = UnitOfWorkStatus.Run;
            }
        }

        public virtual IUnitOfWorkBuilder AddStart(Func<Task> unit, Func<Task>? rollback = null)
            => AddStart(rollback == null
                ? new UnitOfWork(EmptyOutputUnit(unit))
                : new UnitOfWork(EmptyOutputUnit(unit), empty => rollback()));

        /// <inheritdoc />
        public IUnitOfWorkBuilder AddStart<T>(Func<Task<T>> unit, Func<T, Task> rollback = null) where T : class =>
            AddStart(rollback == null
                ? new UnitOfWork(OutputUnit(unit))
                : new UnitOfWork(OutputUnit(unit), obj => rollback(obj as T)));

        public virtual IUnitOfWorkBuilder AddStart(UnitOfWork unit) {
            OnStartActions.Add(unit);
            return this;
        }

        /// <inheritdoc />
        public virtual IUnitOfWorkBuilder Add(Func<Task> unit, Func<Task>? rollback = null) =>
            Add(rollback == null
                ? new UnitOfWork(EmptyOutputUnit(unit))
                : new UnitOfWork(EmptyOutputUnit(unit), empty => rollback()));

        /// <inheritdoc />
        public IUnitOfWorkBuilder Add<T>(Func<Task<T>> unit, Func<T, Task>? rollback = null) where T : class
            => Add(rollback == null
                ? new UnitOfWork(OutputUnit(unit))
                : new UnitOfWork(OutputUnit(unit), obj => rollback(obj as T)));

        /// <inheritdoc />
        public virtual IUnitOfWorkBuilder Add(UnitOfWork unit) {
            Actions.Add(unit);
            return this;
        }

        /// <inheritdoc />
        public virtual async Task<T> AddImmediateAsync<T>(Func<Task<T>> unit, Func<T, Task>? rollback = null)
            where T : class {
            if (unit == null) return null;
            var result = await unit();
            Add(rollback == null
                ? new UnitOfWork(OutputUnit(unit)) {
                    Status = UnitOfWorkStatus.Run
                }
                : new UnitOfWork(OutputUnit(unit), obj => rollback(obj as T)) {
                    Status = UnitOfWorkStatus.Run, Output = result
                });
            return result;
        }

        /// <inheritdoc />
        public virtual IUnitOfWorkBuilder AddEnd(Func<Task> unit, Func<Task>? rollback = null) =>
            Add(rollback == null
                ? new UnitOfWork(EmptyOutputUnit(unit))
                : new UnitOfWork(EmptyOutputUnit(unit), empty => rollback()));

        /// <inheritdoc />
        public virtual IUnitOfWorkBuilder AddEnd(UnitOfWork unit) {
            OnEndActions.Add(unit);
            return this;
        }

        /// <inheritdoc />
        public IUnitOfWorkBuilder AddEnd<T>(Func<Task<T>> unit, Func<T, Task>? rollback = null) where T : class
            => Add(rollback == null
                ? new UnitOfWork(OutputUnit(unit))
                : new UnitOfWork(OutputUnit(unit), obj => rollback(obj as T)));

        /// <inheritdoc />
        public virtual void Subscribe(params ITransactional[] transactionals) {
            foreach (var r in transactionals) {
                AddStart(async () => await r.StartAsync(),
                    async () => await r.RollbackAsync());
                AddEnd(async () => await r.CommitAsync(true),
                    async () => await r.RollbackAsync());
            }
        }

        public virtual async Task CommitAsync(bool autoRollback = true) {
            try {
                foreach (var action in WaitingActions) {
                    action.Status = UnitOfWorkStatus.Running;
                    var result = await action.Action();
                    action.Output = result;
                    action.Status = UnitOfWorkStatus.Run;
                }
            } catch {
                if (autoRollback) await RollbackAsync();
                throw;
            }
        }

        public virtual async Task RollbackAsync() {
            var rollbackExceptions = new List<Exception>();
            foreach (var action in RunActions.Reverse()) {
                action.Status = UnitOfWorkStatus.RollingBack;
                try {
                    if (action.Rollback != null) await action.Rollback(action.Output);
                } catch (Exception e) {
                    rollbackExceptions.Add(e);
                    Console.WriteLine(e);
                    action.Status = UnitOfWorkStatus.RollbackFailed;
                    continue;
                }

                action.Status = UnitOfWorkStatus.RolledBack;
            }

            if (rollbackExceptions.Any())
                throw new AggregateException("Unit of work rollback has failed", rollbackExceptions);
        }

        protected static Func<Task<object?>> EmptyOutputUnit(Func<Task> unit)
            => async () => {
                await unit();
                return null;
            };

        protected static Func<Task<object?>> OutputUnit<T>(Func<Task<T>> unit) where T : class => async () =>
            await unit();
    }
}

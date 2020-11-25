using System;
using System.Threading.Tasks;

namespace ConsoleApp1.UoW
{
    public class UnitOfWork
    {
        public UnitOfWork(Func<Task<object?>> action, Func<Task>? rollback) {
            Action = action;
            if (rollback != null) Rollback = empty => rollback();
            Status = UnitOfWorkStatus.Waiting;
        }

        public UnitOfWork(Func<Task<object?>> action, Func<object?, Task>? rollback = null) {
            Action = action;
            Rollback = rollback;
            Status = UnitOfWorkStatus.Waiting;
        }

        public Func<Task<object?>> Action { get; }
        public Func<object?, Task>? Rollback { get; }
        public object? Output { get; set; }
        public UnitOfWorkStatus Status { get; set; }
    }
}
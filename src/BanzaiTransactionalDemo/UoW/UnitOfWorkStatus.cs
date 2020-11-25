namespace BanzaiTransactionalDemo.UoW
{
    public enum UnitOfWorkStatus
    {
        Waiting,
        Running,
        Run,
        Canceled,
        RollingBack,
        RolledBack,
        RollbackFailed
    }
}
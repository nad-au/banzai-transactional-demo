namespace BanzaiTransactionalDemo.Workflow
{
    public interface IWorkflowContext<T>
    {
        public T Data { get; }
    }
}
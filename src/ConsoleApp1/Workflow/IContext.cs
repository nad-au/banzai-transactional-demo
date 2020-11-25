namespace ConsoleApp1.Workflow
{
    public interface IWorkflowContext<T>
    {
        public T Data { get; }
    }
}
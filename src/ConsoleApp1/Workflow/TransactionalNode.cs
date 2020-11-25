using Banzai;
using ConsoleApp1.UoW;

namespace ConsoleApp1.Workflow
{
    public interface ITransactionalNode
    {
        ITransactional[] GetTransactionals();
    }

    public abstract class TransactionalNode<T> : Node<T>, ITransactionalNode
    {
        public abstract ITransactional[] GetTransactionals();
    }
}
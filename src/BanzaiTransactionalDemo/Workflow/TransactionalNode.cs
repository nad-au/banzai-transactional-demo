using Banzai;
using BanzaiTransactionalDemo.UoW;

namespace BanzaiTransactionalDemo.Workflow
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
using Banzai;
using Banzai.Factories;

namespace BanzaiTransactionalDemo.Workflow
{
    public interface IWorkflowBuilder<T>
    {
        IWorkflowBuilder<T> AddNode<TNode>() where TNode : INode<T>;
        IPipelineNode<T> Build();
    }

    public class WorkflowBuilder<T> : IWorkflowBuilder<T>
    {
        private readonly INodeFactory<T> _nodeFactory;
        private readonly PipelineNode<T> _pipelineNode;

        public WorkflowBuilder(INodeFactory<T> nodeFactory)
        {
            _nodeFactory = nodeFactory;
            _pipelineNode = new PipelineNode<T>();
        }

        public IWorkflowBuilder<T> AddNode<TNode>() where TNode : INode<T>
        {
            _pipelineNode.AddChild(_nodeFactory.GetNode<TNode>());
            return this;
        }

        public IPipelineNode<T> Build() => _pipelineNode;
    }
}
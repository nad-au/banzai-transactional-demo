using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Banzai;
using BanzaiTransactionalDemo.UoW;

namespace BanzaiTransactionalDemo.Workflow
{
    public static class NodeExtensions
    {
        public static async Task<NodeResult> ExecuteAsTransactional<T>(this IMultiNode<T> multiNode, IUnitOfWorkBuilder uow, T context)
        {
            var transactionals = multiNode.GetUniqueTransactionals();
            foreach (var transactional in transactionals)
            {
                await Console.Out.WriteLineAsync($"Subscribing to UoW: {transactional.GetType().Name}");
            }

            uow.Subscribe(transactionals);
            
            await uow.StartAsync();

            try
            {
                var result = await multiNode.ExecuteAsync(context);
                if (result.Status == NodeResultStatus.Succeeded)
                {
                    await uow.CommitAsync(true);
                }

                return result;
            } catch (Exception e) {
                await uow.RollbackAsync();
                
                throw;
            }
        }

        public static ITransactional[] GetUniqueTransactionals<T>(this IMultiNode<T> multiNode)
        {
            var transactionals = new List<ITransactional>();
            foreach (var node in multiNode.Children)
            {
                if (!(node is ITransactionalNode transactionalNode)) continue;
                
                foreach (var transactional in transactionalNode.GetTransactionals())
                {
                    if (transactionals.Contains(transactional)) continue;
                    transactionals.Add(transactional);
                }
            }

            return transactionals.ToArray();
        }

        public static IEnumerable<string> GetErrorMessages(this NodeResult nodeResult)
        {
            var messages = new List<string>();
            if (nodeResult.Exception != null)
            {
                messages.Add(nodeResult.Exception.Message);
            }

            foreach (var childResult in nodeResult.ChildResults)
            {
                if (childResult.Exception != null)
                {
                    messages.Add(childResult.Exception.Message);
                }
            }

            return messages;
        }
    }
}
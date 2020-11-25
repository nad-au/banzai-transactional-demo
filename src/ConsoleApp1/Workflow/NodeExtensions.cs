using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Banzai;
using ConsoleApp1.UoW;

namespace ConsoleApp1.Workflow
{
    public static class NodeExtensions
    {
        public static async Task<NodeResult> ExecuteAsTransactional<T>(this IMultiNode<T> multiNode, IUnitOfWorkBuilder uow, T context)
        {
            var transactionals = multiNode.GetUniqueTransactionals();

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
                
                // TODO: Should not throw, but return error, or set overall status as failed
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
    }
}
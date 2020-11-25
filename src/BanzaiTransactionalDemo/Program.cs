using System.Threading.Tasks;
using Autofac;
using Banzai.Autofac;
using BanzaiTransactionalDemo.Commands;
using BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer;
using BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer.Nodes;
using BanzaiTransactionalDemo.UoW;
using BanzaiTransactionalDemo.Workflow;

namespace BanzaiTransactionalDemo
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using var container = GetContainer();
            
            var handler = container.Resolve<ICommandHandler<CreateBankAccountForPayerCommand>>();
                
            await handler.Handle(new CreateBankAccountForPayerCommand());
        }

        private static IContainer GetContainer()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterBanzaiNodes(typeof(GetPayerDetails).Assembly, true);
            containerBuilder.RegisterType<Transactional1>().SingleInstance();
            containerBuilder.RegisterType<Transactional3>().SingleInstance();
            containerBuilder.RegisterType<Transactional3>().SingleInstance();
            containerBuilder.RegisterGeneric(typeof(WorkflowBuilder<>))
                .As(typeof(IWorkflowBuilder<>));
            containerBuilder.RegisterType<CreateBankAccountForPayerCommandHandler>().AsImplementedInterfaces();
            containerBuilder.RegisterType<UnitOfWorkBuilder>().AsImplementedInterfaces();
            return containerBuilder.Build();
        }
    }

}
using System.Threading.Tasks;
using Autofac;
using Banzai.Autofac;
using ConsoleApp1.Commands;
using ConsoleApp1.Commands.CreateBankAccountForPayer;
using ConsoleApp1.Commands.CreateBankAccountForPayer.Nodes;
using ConsoleApp1.UoW;
using ConsoleApp1.Workflow;

namespace ConsoleApp1
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
using System;
using System.Threading.Tasks;
using Autofac;
using Banzai.Autofac;
using BanzaiTransactionalDemo.Commands;
using BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer;
using BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer.Nodes;
using BanzaiTransactionalDemo.UoW;
using BanzaiTransactionalDemo.Workflow;
using MediatR;

namespace BanzaiTransactionalDemo
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using var container = GetContainer();

            var mediator = container.Resolve<IMediator>();
            
            var result = await mediator.Send(new CreateBankAccountForPayerCommand());
            await Console.Out.WriteLineAsync($"Success: {result}");
        }

        private static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterBanzaiNodes(typeof(GetPayerDetails).Assembly, true);
            builder.RegisterGeneric(typeof(WorkflowBuilder<>))
                .As(typeof(IWorkflowBuilder<>));

            // Mediator
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterType<CreateBankAccountForPayerCommandHandler>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            builder.RegisterType<UnitOfWorkBuilder>().AsImplementedInterfaces();
            builder.RegisterType<Transactional1>().SingleInstance();
            builder.RegisterType<Transactional3>().SingleInstance();
            builder.RegisterType<Transactional3>().SingleInstance();

            return builder.Build();
        }
    }
}
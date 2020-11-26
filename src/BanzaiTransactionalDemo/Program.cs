using System;
using System.Threading.Tasks;
using Autofac;
using Banzai.Autofac;
using BanzaiTransactionalDemo.Commands;
using BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer;
using BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer.Nodes;
using BanzaiTransactionalDemo.Commands.CreateBankAccountForPayer.Validators;
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

            var command = new CreateBankAccountForPayerCommand
            {
                PayerId = 1,
                Bsb = "123-456",
                AccountNumber = "4567890",
                AccountName = "TEST",
                IsPrimary = true
            };
            var result = await mediator.Send(command);
            
            await Console.Out.WriteLineAsync($"Success: {result.Success}");
            foreach (var message in result.Messages)
            {
                await Console.Out.WriteLineAsync($"Message: {message}");
            }
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
            builder.RegisterType<PayerRepository>().SingleInstance();
            builder.RegisterType<BankAccountRepository>().SingleInstance();
            
            builder.RegisterType<BankAccountValidator>();

            return builder.Build();
        }
    }
}
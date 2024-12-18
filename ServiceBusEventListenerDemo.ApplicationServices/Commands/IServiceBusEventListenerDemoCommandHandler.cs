namespace ServiceBusEventListenerDemo.ApplicationServices.Commands
{
    public interface IServiceBusEventListenerDemoCommandHandler<in TCommand> where TCommand : ICommands
    {
         Task Execute(TCommand command);
    }
}

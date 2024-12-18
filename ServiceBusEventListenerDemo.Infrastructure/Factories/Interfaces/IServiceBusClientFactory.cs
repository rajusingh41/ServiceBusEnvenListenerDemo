using Azure.Messaging.ServiceBus;

namespace ServiceBusEventListenerDemo.Infrastructure.Factories.Interfaces
{
    public interface IServiceBusClientFactory
    {
        ServiceBusClient GetServiceBusConnection();
    }
}

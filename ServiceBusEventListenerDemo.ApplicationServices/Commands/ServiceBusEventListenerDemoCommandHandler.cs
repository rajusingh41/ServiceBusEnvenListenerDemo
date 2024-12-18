using Microsoft.Extensions.Logging;

namespace ServiceBusEventListenerDemo.ApplicationServices.Commands
{
    public class ServiceBusEventListenerDemoCommandHandler : IServiceBusEventListenerDemoCommandHandler<ServiceBusEventListenerDemoCommand>
    {
        private readonly ILogger<ServiceBusEventListenerDemoCommandHandler> _logger;

        public ServiceBusEventListenerDemoCommandHandler(ILogger<ServiceBusEventListenerDemoCommandHandler> logger)
        {
                _logger = logger;
        }
        public async Task Execute(ServiceBusEventListenerDemoCommand command)
        {
            _logger.LogInformation($"ServiceBusEventListener - CH. Execute: JobNumber: {command.JobNumber}, ContractorSiteIndentifier: {command.ContractorSiteIdentifier}, Time Stamp: {command.TimeStamp}");

            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ServiceBusEventListener - CH [{command.JobNumber}] Exception");
            }
            finally 
            {
                _logger.LogInformation($"ServiceBusEventListener - CH [{command.JobNumber}]. Execution finished");

            }
        }
    }
}

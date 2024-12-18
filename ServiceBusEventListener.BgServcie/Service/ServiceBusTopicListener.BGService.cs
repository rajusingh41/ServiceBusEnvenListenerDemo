using Azure.Messaging.ServiceBus;
using ServiceBusEventListenerDemo.ApplicationServices.Commands;
using ServiceBusEventListenerDemo.Infrastructure.Factories.Interfaces;
using ServiceBusEventListenerDemo.Model.Configuration;
using ServiceBusEventListenerDemo.Model.Entities;
using System.Text;
using System.Text.Json;

namespace ServiceBusEventListener.BgServcie.Service
{
    public class ServiceBusTopicListener : BackgroundService
    {
        private readonly ILogger<ServiceBusTopicListener> _logger;
        private readonly BGServiceSettings _bGServiceSettings;
        private readonly IServiceBusClientFactory _serviceBusClientFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly ServiceBusConfiguration _serviceBusConfiguration;

        public ServiceBusTopicListener(ILogger<ServiceBusTopicListener> logger,
            BGServiceSettings bGServiceSettings,
            IServiceBusClientFactory serviceBusClientFactory,
            IServiceProvider serviceProvider,
            ServiceBusConfiguration serviceBusConfiguration)
        {
            _logger = logger;
            _bGServiceSettings = bGServiceSettings;
            _serviceBusClientFactory = serviceBusClientFactory;
            _serviceProvider = serviceProvider;
            _serviceBusConfiguration = serviceBusConfiguration;

        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ServiceBusEventListener - ExecuteAsync - start");
            var options = new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = true,
                MaxConcurrentCalls = _bGServiceSettings.MaxConcurrentCalls
            };

            ServiceBusClient subScriptionClient = _serviceBusClientFactory.GetServiceBusConnection();
            ServiceBusProcessor serviceBusProcessor = subScriptionClient.CreateProcessor(_serviceBusConfiguration.ServiceBusDemoTopicName, _serviceBusConfiguration.ServiceBustDemoTopicSubscriber, options);
            serviceBusProcessor.ProcessMessageAsync += MessageHandler;
            serviceBusProcessor.ProcessErrorAsync += ErrorHandler;
            await serviceBusProcessor.StartProcessingAsync();
            _logger.LogInformation("ServiceBusEventListener - ExecuteAsync - End");
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "ServiceBusEventListener - ErrorHandler");
            return Task.CompletedTask;

        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            _logger.LogInformation("ServiceBusEventListener - MessageHandler - Start");
            Thread.Sleep(_bGServiceSettings.SleepBetweenMessageMs * 20);
            WorkOrderModel messageModel = null;
            try
            {
                messageModel = GetMessageFromBody(args?.Message);

                if (messageModel == null)
                {
                    _logger.LogInformation("ServiceBusEventListener - MessageHandler - args Message can't be null or empty");
                    return;
                }

                _logger.LogInformation($"ServiceBusEventListener - MessageHandler - ({messageModel?.JobNumber}). message: '{args.Message.Body}'");

                // can add validation as per your requirement 

                var workOrderCommand = new ServiceBusEventListenerDemoCommand(
                    messageModel.WorkOrderId,
                    messageModel.CorrelationId,
                    messageModel.TimeStamp,
                    messageModel.Status,
                    messageModel.ContractorSiteIdentifier,
                    messageModel.JobNumber);

                var commandHandler = (IServiceBusEventListenerDemoCommandHandler<ServiceBusEventListenerDemoCommand>)_serviceProvider.GetServices(typeof(IServiceBusEventListenerDemoCommandHandler<ServiceBusEventListenerDemoCommand>));

                await commandHandler.Execute(workOrderCommand);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ServiceBusEventListener - MessageHandler - ({messageModel?.JobNumber}). Exception");
                throw;
            }
            finally
            {
                _logger.LogInformation("ServiceBusEventListener - MessageHandler - End");
            }
        }

        private WorkOrderModel? GetMessageFromBody(ServiceBusReceivedMessage? message)
        {

            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var body = Encoding.UTF8.GetString(message.Body);
            return JsonSerializer.Deserialize<WorkOrderModel>(body);
        }
    }
}

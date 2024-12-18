namespace ServiceBusEventListenerDemo.Model.Configuration
{
    public class ServiceBusConfiguration
    {
        public required string ServiceBusConnection { get; set; }

        public string ServiceBusDemoTopicName { get; set; }

        public string ServiceBustDemoTopicSubscriber { get; set; }
    }
}

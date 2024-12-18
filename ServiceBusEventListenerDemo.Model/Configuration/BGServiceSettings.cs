namespace ServiceBusEventListenerDemo.Model.Configuration
{
    public class BGServiceSettings
    {
        public int MaxConcurrentCalls { get; set; }

        public int HealthCheckFrequencySeconds { get; set; }

        public int HealthCheckPort { get; set; }

        public int SleepBetweenMessageMs { get; set; }

    }
}

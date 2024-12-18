namespace ServiceBusEventListenerDemo.ApplicationServices.Commands
{
    public class ServiceBusEventListenerDemoCommand : ICommands
    {
        public ServiceBusEventListenerDemoCommand(Guid workOrderId,
            Guid correlationId,
            string timeStamp,
            string status,
            string contractorSiteIdentifier,
            string jobNumber)
        {
            WorkOrderId = workOrderId;
            CorrelationId = correlationId;
            TimeStamp = timeStamp;
            Status = status;
            ContractorSiteIdentifier = contractorSiteIdentifier;
            JobNumber = jobNumber;
        }

        public Guid WorkOrderId { get; set; }   
        public Guid CorrelationId { get; set; }
        public string TimeStamp { get; set; }
        public string Status { get; set; }
        public string ContractorSiteIdentifier { get; set; }
        public string JobNumber { get; set; }
    }
}

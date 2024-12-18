using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusEventListenerDemo.Model.Entities
{
    public class WorkOrderModel
    {
        public Guid WorkOrderId { get; set; }

        public Guid CorrelationId { get; set; }

        public string TimeStamp { get; set; }

        public string Status { get; set; }

        public string ContractorSiteIdentifier { get; set; }

        public string JobNumber { get; set; }
    }
}

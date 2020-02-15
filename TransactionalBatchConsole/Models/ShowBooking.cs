using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionalBatchConsole.Models
{
    public class ShowBooking
    {
        public string id { get; set; }
        public string PartitionKey { get; set; }
    }
}

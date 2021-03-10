using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosDBDeleteByQuery
{
    public class IdPkPair
    {
        public string DocumentId { get; set; }
        public string PartitionKey { get; set; }
    }
}

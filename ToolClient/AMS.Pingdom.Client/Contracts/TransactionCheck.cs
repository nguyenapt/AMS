using System.Collections.Generic;

namespace PingdomClient.Contracts
{
    public class TransactionCheck
    {
        public bool Active { get; set; }

        /// <summary>
        /// Check identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Creating time. Format is UNIX timestamp
        /// </summary>
        public int Created_At { get; set; }

        /// <summary>
        /// Modifying time. Format is UNIX timestamp
        /// </summary>
        public int Modified_At { get; set; }

        public int Interval { get; set; }

        /// <summary>
        /// Check name
        /// </summary>
        public string Name { get; set; }

        public string Region { get; set; }

        /// <summary>
        /// Current status of check
        /// </summary>
        public string Status { get; set; }

        public string[] Tags { get; set; }

        public string Type { get; set; }
    }

    public class CreateNewTransactionCheckResponse : PingdomResponse
    {
        public TransactionCheckExtended Check { get; set; }
    }

    public class GetDetailedTransactionCheckInformationResponse : PingdomResponse
    {
        public TransactionCheckExtended Check { get; set; }
    }

    public class GetTransactionCheckListResponse : PingdomResponse
    {
        public IEnumerable<TransactionCheck> Checks { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
    }

}
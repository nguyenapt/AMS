namespace PingdomClient.Contracts
{
    using System.Collections.Generic;

    public class TransactionCheckExtended : TransactionCheck
    {
        /// <summary>
        /// Identifier of contact who should receive alerts
        /// </summary>
        public IEnumerable<int> Contact_Ids { get; set; }

        public string Custom_Message { get; set; }

        /// <summary>
        /// Send notification when down n times
        /// </summary>
        public int Send_Notification_When_Down { get; set; }

        public string Security_Level { get; set; }

        public dynamic Steps { get; set; }

        public int[] Teams_Ids { get; set; }

        public int[] Integration_Ids { get; set; }

        public dynamic MetaData { get; set; }
    }
}
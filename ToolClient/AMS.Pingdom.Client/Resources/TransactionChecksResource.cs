namespace PingdomClient.Resources
{
    using Contracts;
    using System.Threading.Tasks;

    public sealed class TransactionChecksResource : Resource
    {
        internal TransactionChecksResource() { }

        /// <summary>
        /// Returns a list overview of all TMS checks. Ref: https://docs.pingdom.com/api/#operation/getAllChecks
        /// </summary>
        /// <returns></returns>
        public Task<GetTransactionCheckListResponse> GetTMSChecksList()
        {
            return Client.GetAsync<GetTransactionCheckListResponse>("tms/check/");
        }

        /// <summary>
        /// Returns a detailed description of a specified TMS check. Ref: https://docs.pingdom.com/api/#operation/getCheck
        /// </summary>
        /// <param name="checkId"></param>
        /// <returns></returns>
        public Task<GetDetailedTransactionCheckInformationResponse> GetDetailedCheckInformation(int checkId)
        {
            return Client.GetAsync<GetDetailedTransactionCheckInformationResponse>(string.Format("tms/check/{0}", checkId));
        }

        /// <summary>
        /// Creates a new TMS check with settings specified by provided parameters. Ref: https://docs.pingdom.com/api/#operation/addCheck
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public Task<CreateNewTransactionCheckResponse> CreateNewCheck(object check)
        {
            return Client.PostAsync<CreateNewTransactionCheckResponse>("tms/check/", check);
        }

        /// <summary>
        /// Modify settings for a TMS check. The provided settings will overwrite previous values.  Ref: https://docs.pingdom.com/api/#operation/modifyCheck
        /// Settings not provided will stay the same as before the update. To clear an existing value, 
        /// provide an empty value. Please note that you cannot change the type of a check once it has been created.
        /// </summary>
        /// <param name="checkId"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public Task<PingdomResponse> ModifyCheck(int checkId, object check)
        {
            return Client.PutAsync<PingdomResponse>(string.Format("tms/check/{0}", checkId), check);
        }

        /// <summary>
        /// Deletes a TMS check. THIS METHOD IS IRREVERSIBLE! You will lose all collected data. Be careful! Ref: https://docs.pingdom.com/api/#operation/deleteCheck
        /// </summary>
        /// <param name="checkId"></param>
        /// <returns></returns>
        public Task<PingdomResponse> DeleteCheck(int checkId)
        {
            return Client.DeleteAsync<PingdomResponse>(string.Format("tms/check/{0}", checkId));
        }

        //To add: https://docs.pingdom.com/api/#operation/getCheckReportStatus
        //To add: https://docs.pingdom.com/api/#operation/getCheckReportStatusAll
        //To add: https://docs.pingdom.com/api/#operation/getCheckReportPerformance
    }
}
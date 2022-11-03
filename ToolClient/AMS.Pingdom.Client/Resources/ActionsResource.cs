namespace PingdomClient.Resources
{
    using Contracts;
    using Extensions;
    using System.Threading.Tasks;

    public sealed class ActionsResource : Resource
    {
        /// <summary>
        /// Returns a list of actions (alerts) that have been generated for your account.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task<GetActionListResponse> GetActionsList(ActionArgs args = null)
        {
            var queryString = args != null ? args.ToQueryString() : string.Empty;
            var apiMethod = "actions" + queryString;

            return Client.GetAsync<GetActionListResponse>(apiMethod);
        }
    }
}
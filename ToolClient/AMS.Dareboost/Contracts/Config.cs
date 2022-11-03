namespace AMS.Dareboost.Contracts
{
    public class ConfigResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public Config[] config { get; set; }
    }
    public class Config
    {
        public string location { get; set; }
        public bool isPrivate { get; set; }
        public ConfigBrowser[] browsers { get; set; }
    }
    public class ConfigBrowser
    {
        public string name { get; set; }
        public bool isMobile { get; set; }

    }
}
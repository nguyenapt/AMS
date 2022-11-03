namespace AMS.PageSpeed.Contracts
{
    public class LoadingExperience
    {
        //public string Id { get; set; }
        public Metrics Metrics { get; set; }
        public string Overall_category { get; set; }
        //public string Initial_url { get; set; }
    }

    public class Metrics
    {
        public Metric CUMULATIVE_LAYOUT_SHIFT_SCORE { get; set; }
        public Metric FIRST_CONTENTFUL_PAINT_MS { get; set; }
        public Metric FIRST_INPUT_DELAY_MS { get; set; }
        public Metric LARGEST_CONTENTFUL_PAINT_MS { get; set; }
    }
    public class Metric
    {
        public double Percentile { get; set; }
        public Distribution[] Distributions { get; set; }
        public string Category { get; set; }
    }

    public class Distribution
    {
        public uint Min { get; set; }
        public uint Max { get; set; }
        public double Proportion { get; set; }
    }
}

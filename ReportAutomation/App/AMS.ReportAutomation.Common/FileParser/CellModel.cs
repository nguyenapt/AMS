namespace AMS.ReportAutomation.Common.FileParser
{
    public class CellModel
    {
        public string PropName { get; set; }

        public object Value { get; set; }

        public CellType Type { get; set; }

        public int ColIndex { get; set; }
    }

    public class CellHeader
    {
        public string Name { get; set; }

        public int ColIndex { get; set; }
    }
}

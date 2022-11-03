using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Analytics.v3.Data;

namespace AMS.GoogleAnalytic
{
    public class AnalyticDataPoint
    {
        public AnalyticDataPoint()
        {
            Rows = new List<IList<string>>();
        }
        public IList<GaData.ColumnHeadersData> ColumnHeaders { get; set; }
        public List<IList<string>> Rows { get; set; }
    }
}

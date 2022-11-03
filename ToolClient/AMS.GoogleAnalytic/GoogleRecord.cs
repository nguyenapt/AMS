using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.GoogleAnalytic
{
    public class GoogleRecord
    {
        public string RecordDate { get; set; }
        public int Sessions { get; set; }
        public int Users { get; set; }
        public int Pageviews { get; set; }
        public double BounceRate { get; set; }
        public int Visits { get; set; }
    }
}

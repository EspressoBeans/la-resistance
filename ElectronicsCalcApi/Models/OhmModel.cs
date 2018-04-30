using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicsCalcApi.Models
{
    public class OhmBandsModel
    {
        public string bandAColor { get; set; }
        public string bandBColor { get; set; }
        public string bandCColor { get; set; }
        public string bandDColor { get; set; }
    }

    public class OhmBandsListModel
    {
        public List<string> bandColors { get; set; }
    }

    public class OhmValuesModel
    {
        public decimal ohms { get; set; }
        public decimal? tolerance { get; set; }
        public decimal? low { get; set; }
        public decimal? high { get; set; }
    }
}
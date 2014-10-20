using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.GisMap
{
    public class CriminalIndexResultModel
    {
        public IList<string> Districts { get; set; }
        public string CriminalIndex { get; set; }

        public CriminalIndexResultModel()
        {
            this.Districts = new List<string>();
        }
    }
}

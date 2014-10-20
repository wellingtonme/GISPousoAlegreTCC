using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Points
{
    public class CriminalIndexModel
    {
        public int Id { get; set; }
        public string District { get; set; }
        public int CriminalIndex { get; set; }
        public string AreaLocation { get; set; }
    }
}

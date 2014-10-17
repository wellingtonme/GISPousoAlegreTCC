using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataBase
{
    public class SqlsModel
    {
        public long Id { get; set; }
        public string SqlCommand { get; set; }
        public string SqlName { get; set; }
    }
}

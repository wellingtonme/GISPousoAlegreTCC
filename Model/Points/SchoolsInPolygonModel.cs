﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Points
{
    public class SchoolsInPolygonModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool Contains { get; set; }
        public string PointLocation { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.GisMap
{
    public class PointModel
    {
        public Geometry geometry { get; set; }
        public Attributes attributes { get; set; }
        public Symbol symbol { get; set; }
        public InfoTemplate infoTemplate { get; set; }
    }

    public class Geometry
    {
        public string x { get; set; }
        public string y { get; set; }
        public SpatialReference spatialReference { get; set; }
    }

    public class SpatialReference
    {
        public int wkid { get; set; }
    }

    public class Attributes
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class Symbol
    {
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public string type { get; set; }
    }

    public class InfoTemplate
    {
        public string title { get; set; }
        public string content { get; set; }
    }
}

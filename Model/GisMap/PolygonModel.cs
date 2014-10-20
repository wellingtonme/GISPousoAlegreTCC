using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.GisMap
{
    public class PolygonModel
    {
        public GeometryPolygon geometry { get; set; }
        public SymbolPolygon symbol { get; set; }
        public AttributesPolygon attributes { set; get; }
        public InfoTemplate infoTemplate { get; set; }
    }

    public class GeometryPolygon
    {
        public IList<IList<IList<string>>> rings { get; set; }
        public SpatialReference spatialReference { get; set; }
    }

    public class AttributesPolygon
    {
        public string District { get; set; }
        public string CriminalIndex { get; set; }
    }

    public class SymbolPolygon
    {
        public IList<int> color { get; set; }
        public Outline outline { get; set; }
        public string type { get; set; }
        public string style { get; set; }
    }

    public class Outline
    {
        public IList<int> color { get; set; }
        public int width { get; set; }
        public string type { get; set; }
        public string style { get; set; }
    }
}

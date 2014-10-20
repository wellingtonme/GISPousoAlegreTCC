using Model.GisMap;
using Model.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Conversion
{
    public class ModelToPointMapService : IModelToPointMapService
    {
        public PointModel ModelToGraphicPoint(GenericPointModel point, string imageUrl, string templateTitle, string templateContent)
        {
            PointModel pointModel = null;

            try
            {
                IDictionary<string, string> coords = CoordinatesParser.GetPointCoordinates(point.PointLocation);
                pointModel = new PointModel();
                pointModel.geometry = new Geometry()
                {
                    x = coords["x"],
                    y = coords["y"],
                    spatialReference = new SpatialReference()
                    {
                        wkid = Constants.SPATIAL_REFERENCES_OF_POINT
                    },
                };

                pointModel.attributes = new Attributes()
                {
                    Name = point.Name,
                    Address = point.Address
                };

                pointModel.symbol = new Symbol()
                {
                    url = imageUrl,
                    height = Constants.ICONS_TO_LAYER_HEIGHT,
                    width = Constants.ICONS_TO_LAYER_WIDTH,
                    type = Constants.GRAPHIC_POINT_TYPE_PMS
                };

                pointModel.infoTemplate = new InfoTemplate()
                {
                    title = templateTitle,
                    content = templateContent
                };

            }
            catch (Exception ex)
            {
                throw ex;
                ///TODO Put log here
            }

            return pointModel;
        }

        public IList<PointModel> ConvertListInGraplhicsPoints(IList<GenericPointModel> points, GraphycsLayerType graphicsType)
        {
            IList<PointModel> result = null;

            try
            {
                if (points != null)
                {
                    if (points.Count > 0)
                    {
                        result = new List<PointModel>();
                        foreach (var point in points)
                        {
                            var url = (graphicsType == GraphycsLayerType.Schools) ? Constants.SCHOOL_POINT_ICON_URL : Constants.COPS_POINT_ICON_URL;
                            var title = (graphicsType == GraphycsLayerType.Schools) ? Constants.SCHOOL_INFOTEMPLATE_TITLE : Constants.COPS_INFOTEMPLATE_TITLE;
                            var content = (graphicsType == GraphycsLayerType.Schools) ? Constants.SCHOOL_INFOTEMPLATE_CONTENT : Constants.COPS_INFOTEMPLATE_CONTENT;
                            var item = ModelToGraphicPoint(point, url, title, content);
                            result.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public PolygonModel ModelToGraphicPolygon(CriminalIndexModel model)
        {
            PolygonModel polygon = null;

            try
            {
                IList<string> coords = CoordinatesParser.GetPolygonCoordinates(model.AreaLocation);
                polygon = new PolygonModel();
                polygon.geometry = new GeometryPolygon()
                {
                    spatialReference = new SpatialReference()
                    {
                        wkid = Constants.SPATIAL_REFERENCES_OF_POINT
                    }
                };
                IList<IList<IList<string>>> ringsOut = new List<IList<IList<string>>>();
                IList<IList<string>> ringsIn = new List<IList<string>>();

                for (int x = 0; x < coords.Count; x=x+2)
                {
                    IList<string> coord = new List<string>();
                    coord.Add(coords[x]);
                    coord.Add(coords[x + 1]);
                    ringsIn.Add(coord);
                }

                ringsOut.Add(ringsIn);
                polygon.geometry.rings = ringsOut;
                polygon.symbol = new SymbolPolygon()
                {
                    color = GetColor(model.CriminalIndex),
                    outline = new Outline()
                    {
                        color = GetColor(model.CriminalIndex),
                        width = Constants.POLYGON_WIDTH,
                        type = Constants.OUTLINE_TYPE,
                        style = Constants.OUTLINE_STYLE
                    },
                    type = Constants.POLYGON_TYPE,
                    style = Constants.POLYGON_STYLE
                };

                polygon.symbol.color[3] = Constants.COLOR_OUTLINE;
                polygon.symbol.outline.color[3] = Constants.COLOR_LINE;

                polygon.attributes = new AttributesPolygon()
                {
                    District = model.District,
                    CriminalIndex = GetIndexName(model.CriminalIndex)
                };

                polygon.infoTemplate = new InfoTemplate()
                {
                    title = "Indice de Criminalidade",
                    content = Constants.CRIMINAL_INDEX_CONTENT
                };
            }
            catch (Exception ex)
            {
                polygon = null;
            }

            return polygon;
        }

        private string GetIndexName(int criminalIndex)
        {
            string result = "";

            try
            {
                switch (criminalIndex)
                {
                    case 1:
                        result = "Muito Baixo";
                        break;
                    case 2:
                        result = "Baixo";
                        break;
                    case 3:
                        result = "Médio";
                        break;
                    case 4:
                        result = "Alto";
                        break;
                    case 5:
                        result = "Muito Alto";
                        break;
                }
            }
            catch (Exception ex)
            {
                result = "";
            }

            return result;
        }

        private IList<int> GetColor(int indexCriminal)
        {
            IList<int> color = null;

            try
            {
                switch (indexCriminal)
                {
                    case 1: 
                        color = Constants.CRIMINAL_INDICE_VERY_LOW;
                        break;
                    case 2:
                        color = Constants.CRIMINAL_INDICE_LOW;
                        break;
                    case 3:
                        color = Constants.CRIMINAL_INDICE_MEDIUM;
                        break;
                    case 4:
                        color = Constants.CRIMINAL_INDICE_HIGH;
                        break;
                    case 5:
                        color = Constants.CRIMINAL_INDICE_VERY_HIGH;
                        break;
                }
            }
            catch (Exception ex)
            {

            }

            return color;
        }

        public IList<PolygonModel> ConvertListToGraphicsPolygon(IList<CriminalIndexModel> criminalIndex)
        {
            IList<PolygonModel> result = null;

            try
            {
                result = new List<PolygonModel>();
                foreach (var item in criminalIndex)
                {
                    result.Add(ModelToGraphicPolygon(item));
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }
}

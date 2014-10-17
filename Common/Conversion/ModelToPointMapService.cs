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
    }
}

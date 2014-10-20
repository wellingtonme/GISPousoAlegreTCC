using Model.GisMap;
using Model.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Conversion
{
    public interface IModelToPointMapService
    {
        PointModel ModelToGraphicPoint(GenericPointModel point, string imageUrl, string templateTitle, string templateContent);

        IList<PointModel> ConvertListInGraplhicsPoints(IList<GenericPointModel> points, GraphycsLayerType graphicsType);

        PolygonModel ModelToGraphicPolygon(CriminalIndexModel model);

        IList<PolygonModel> ConvertListToGraphicsPolygon(IList<CriminalIndexModel> criminalIndex);
    }
}

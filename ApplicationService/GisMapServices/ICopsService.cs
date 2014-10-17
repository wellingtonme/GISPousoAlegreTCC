using Model.GisMap;
using Model.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.GisMapServices
{
    public interface ICopsService
    {
        IList<CopsModel> GetAllCops();

        IList<CopsInPolygonModel> GetAllCopsInSelectedArea(string coordinates);

        IList<PointModel> GetCopsGraphycsLayersPoints();
    }
}

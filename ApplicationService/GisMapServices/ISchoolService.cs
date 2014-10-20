using Model.GisMap;
using Model.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.GisMapServices
{
    public interface ISchoolService
    {
        IList<SchoolModel> GetAllSchools();

        IList<SchoolModel> GetAllSchoolsInSelectedArea(string coordinates);

        IList<PointModel> GetSchoolGraphycsLayersPoints();
    }
}

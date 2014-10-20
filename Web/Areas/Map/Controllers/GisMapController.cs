using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ApplicationService.GisMapServices;
using SharpArch.Domain;
using Model.Points;
using System.Collections;
using Common.Conversion;

namespace Web.Areas.Map.Controllers
{
    public class GisMapController : Controller
    {
        //
        // GET: /Map/GisMap/

        public ActionResult Index()
        {
            //try
            //{
            //    var schoolService = SafeServiceLocator<ISchoolService>.GetService();
            //    schoolService.GetAllSchools();
                
            //}
            //catch (Exception ex)
            //{

            //}
            return View();
        }

        public JsonResult GetGraphicsLayersSchools()
        {
            JsonResult result = new JsonResult();

            try
            {
                var schoolService = SafeServiceLocator<ISchoolService>.GetService();
                var graphicsPoints = schoolService.GetSchoolGraphycsLayersPoints();
                result.Data = graphicsPoints;
            }
            catch (Exception ex)
            {

            }

            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public JsonResult GetGraphycsLayerCops()
        {
            JsonResult result = new JsonResult();

            try
            {
                var copsService = SafeServiceLocator<ICopsService>.GetService();
                var graphicsPoints = copsService.GetCopsGraphycsLayersPoints();
                result.Data = graphicsPoints;
            }
            catch (Exception ex)
            {

            }

            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public JsonResult GetGraphicsCriminalIndex()
        {
            JsonResult result = new JsonResult();

            try
            {
                var criminalService = SafeServiceLocator<ICriminalIndexService>.GetService();
                var criminals = criminalService.GetCriminalIndexGraphics();
                if (criminalService != null)
                {
                    result.Data = criminals;
                }
            }
            catch (Exception ex)
            {

            }

            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public JsonResult GetObjectsInSelectedArea(string geometry)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                var coords = CoordinatesParser.ParseCoordinatesToPolygon(geometry);
                var schoolService = SafeServiceLocator<ISchoolService>.GetService();
                var copsService = SafeServiceLocator<ICopsService>.GetService();
                IList<SchoolModel> schoolsInArea = schoolService.GetAllSchoolsInSelectedArea(geometry);
                IList<CopsModel> copsInArea = copsService.GetAllCopsInSelectedArea(geometry);
                result.Data = new
                {
                    reqStatus = 200,
                    schools = schoolsInArea,
                    cops = copsInArea
                };
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        private IList<string[]> GetCoordinates(string coordinates)
        {
            //string result = "";
            IList<string[]> result = null;
            try
            {
                string[] splitCoords = coordinates.Split(',');
                
                for (int x = 0; x < splitCoords.Length; x = x+2)
                {
                    result.Add(new string[]{splitCoords[x], splitCoords[x + 1]});
                    //result = string.Concat(result,(string.Format("{0} {1}{2}", splitCoords[x], splitCoords[x + 1], ',')));
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }
    }
}

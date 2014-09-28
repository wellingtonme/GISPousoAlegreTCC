using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ApplicationService.GisMapServices;
using SharpArch.Domain;
using Model.School;

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

        public JsonResult GetSchoolInSelectedArea(string geometry)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                //var js = new JavaScriptSerializer();
                //var obj = js.Serialize(geometry);
                string coordinates = GetCoordinates(geometry);

                IList<SchoolsInPolygonModel> schools = SafeServiceLocator<ISchoolService>.GetService().GetAllSchoolsInSelectedArea(coordinates);
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        private string GetCoordinates(string coordinates)
        {
            string result = "";
            try
            {
                string[] splitCoords = coordinates.Split(',');
                
                for (int x = 0; x < splitCoords.Length; x = x+2)
                {
                    result = string.Concat(result,(string.Format("{0} {1}{2}", splitCoords[x], splitCoords[x + 1], ',')));
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }
}

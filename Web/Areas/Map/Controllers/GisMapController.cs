using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ApplicationService.GisMapServices;
using SharpArch.Domain;

namespace Web.Areas.Map.Controllers
{
    public class GisMapController : Controller
    {
        //
        // GET: /Map/GisMap/

        public ActionResult Index()
        {
            try
            {
                var schoolService = SafeServiceLocator<ISchoolService>.GetService();
                schoolService.GetAllSchools();
                
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public JsonResult GetSchoolInSelectedArea(object geometry)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                var js = new JavaScriptSerializer();
                var obj = js.Serialize(geometry);

                IList<string> coordinates = GetCoordinates(obj);
            }
            catch (Exception ex)
            {

            }
            //make a service to get school
            return result;
        }

        private IList<string> GetCoordinates(string coordinates)
        {
            IList<string> result = null;
            try
            {
                result = new List<string>();
                string[] splitCoords = coordinates.Split(',');
                splitCoords[0].Replace("[\\", "");
                splitCoords.Last().Replace("\\]", "");
                for (int x = 0; x < splitCoords.Length / 2; x = x+2)
                {
                    result.Add(string.Format("{0} {1}", splitCoords[x], splitCoords[x + 1]));
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }
}

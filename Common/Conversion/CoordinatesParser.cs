using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Conversion
{
    public static class CoordinatesParser
    {
        public static IList<string> GetPolygonCoordinates(string coordinatesToParse)
        {
            IList<string> result = null;

            try
            {
                result = new List<string>();
                string[] split = coordinatesToParse.Split('(');
                split[2] = split[2].Replace(")", "");
                string[] coords = split[2].Split(',');
                for (int x = 0; x < coords.Length; x++)
                {
                    string[] temp = coords[x].Split(' ');
                    result.Add(temp[0]);
                    result.Add(temp[1]);
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public static IDictionary<string, string> GetPointCoordinates(string coordinatesToParse)
        {
            IDictionary<string, string> result = null;

            try
            {
                result = new Dictionary<string, string>();
                string[] sprit = coordinatesToParse.Split('(');
                sprit[1] = sprit[1].Replace(")","");
                string[] coord = sprit[1].Split(' ');
                result["x"] = coord[0];
                result["y"] = coord[1];
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public static double[] ConvertWebMercatorToGeographic(string[] mercator)
        {
            double[] result = null;
            if (Math.Abs(double.Parse(mercator[0])) < 180 && Math.Abs(double.Parse(mercator[1])) < 90)
                return null;

            if ((Math.Abs(double.Parse(mercator[0])) > 20037508.3427892) || (Math.Abs(double.Parse(mercator[1])) > 20037508.3427892))
                return null;

            double x = double.Parse(mercator[0]);
            double y = double.Parse(mercator[1]);
            double num3 = x / 6378137.0;
            double num4 = num3 * 57.295779513082323;
            double num5 = Math.Floor((double)((num4 + 180.0) / 360.0));
            double num6 = num4 - (num5 * 360.0);
            double num7 = 1.5707963267948966 - (2.0 * Math.Atan(Math.Exp((-1.0 * y) / 6378137.0)));
            result[0] = num6;
            result[1] = num7 * 57.295779513082323;
            return result;
        }

        public static IList<double[]> ConverListWebMercatorToGeographic(IList<string[]> coordinates)
        {
            IList<double[]> result = null;
            try
            {
                foreach (var mercator in coordinates)
                {
                    result.Add(ConvertWebMercatorToGeographic(mercator));
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        public static string ParseCoordinatesToPolygon(string coordinates)
        {
            string result = "";

            try
            {
                string[] splited = coordinates.Split(',');
                for (int x = 0; x < splited.Length; x=x+2)
                {
                    var temp = string.Concat(splited[x] + " ", splited[x + 1]+",");
                    if ((x + 2) == splited.Length) temp = temp.Replace(",", "");
                    result = string.Concat(result, temp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}

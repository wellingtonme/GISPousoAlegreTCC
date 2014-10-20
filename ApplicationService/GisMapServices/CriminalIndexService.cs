using Common.Conversion;
using Data.DataBase;
using Model.GisMap;
using Model.Points;
using Npgsql;
using SharpArch.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.GisMapServices
{
    public class CriminalIndexService : ICriminalIndexService
    {
        NpgsqlConnection _conn = null;

        public IList<CriminalIndexModel> GetAllCriminalIndex()
        {
            IList<CriminalIndexModel> result = null;

            try
            {
                var dataBaseService = SafeServiceLocator<IDataBaseBasics>.GetService();
                var schoolSql = dataBaseService.GetSqlCommandByName("selectAllCriminalIndex");

                if (_conn == null) _conn = dataBaseService.GetConnection();
                
                _conn.Open();

                NpgsqlCommand command = new NpgsqlCommand(schoolSql.SqlCommand, _conn);
                var dr = command.ExecuteReader();

                if (!dr.HasRows) throw new ArgumentException("No Criminal Index found.");

                result = DataBaseResultConversion.FormatResultList<CriminalIndexModel>(dr);

                _conn.Close();
            }
            catch (Exception ex)
            {
                if (_conn != null) _conn.Close();
                result = null;
            }

            return result;
        }

        public IList<PolygonModel> GetCriminalIndexGraphics()
        {
            IList<PolygonModel> result = null;

            IModelToPointMapService mapService = SafeServiceLocator<IModelToPointMapService>.GetService();

            try
            {
                var criminals = GetAllCriminalIndex();
                if (criminals == null) return null;
                if (criminals.Count < 1) return null;
                result = mapService.ConvertListToGraphicsPolygon(criminals);
            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }
}

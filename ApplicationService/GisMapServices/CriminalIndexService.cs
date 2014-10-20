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

        public CriminalIndexResultModel GetAllCriminalIndexInSelectedArea(string coordinates)
        {
            CriminalIndexResultModel result = null;
            try
            {
                coordinates = CoordinatesParser.ParseCoordinatesToPolygon(coordinates);
                coordinates = string.Format("POLYGON(({0}))", coordinates);
                var dataBaseService = SafeServiceLocator<IDataBaseBasics>.GetService();
                var sql = dataBaseService.GetSqlCommandByName("selectCriminalIndexInArea");

                if (_conn == null) _conn = dataBaseService.GetConnection();
                _conn.Open();

                NpgsqlCommand command = new NpgsqlCommand(sql.SqlCommand, _conn);
                command.Parameters.Add(new NpgsqlParameter("polygon", coordinates));

                NpgsqlDataReader dr = command.ExecuteReader();

                var criminals = DataBaseResultConversion.FormatResultList<CriminalIndexModel>(dr);

                _conn.Close();

                if (criminals == null) return null;
                if (criminals.Count < 1) return null;

                result = FormatToCriminalIndexResult(criminals);

            }
            catch (Exception ex)
            {
                if (_conn != null) _conn.Close();
            }
            return result;
        }

        private CriminalIndexResultModel FormatToCriminalIndexResult(IList<CriminalIndexModel> criminals)
        {
            CriminalIndexResultModel result = null;

            try
            {
                if (criminals == null) return null;
                if (criminals.Count < 1) return null;

                int index = (int)Math.Round(criminals.Average(c => c.CriminalIndex));

                result = new CriminalIndexResultModel();
                foreach (var item in criminals)
                {
                    result.Districts.Add(item.District);
                }

                result.CriminalIndex = GetCriminalIndexName(index);

            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        private string GetCriminalIndexName(int criminalIndex)
        {
            string criminalName = "";

            switch (criminalIndex)
            {
                case 1:
                    criminalName = "Muito Baixo";
                    break;
                case 2:
                    criminalName = "Baixo";
                    break;
                case 3:
                    criminalName = "Media";
                    break;
                case 4:
                    criminalName = "Alto";
                    break;
                case 5:
                    criminalName = "Muito Alto";
                    break;
            }

            return criminalName;
        }
    }
}

using AutoMapper;
using Common;
using Common.Conversion;
using Data.DataBase;
using Model.GisMap;
using Model.Points;
using Npgsql;
using SharpArch.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.GisMapServices
{
    public class SchoolService : ISchoolService
    {
        NpgsqlConnection _conn = null;

        public IList<SchoolModel> GetAllSchools()
        {            
            IList<SchoolModel> result = null;
            try
            {
                var dataBaseService = SafeServiceLocator<IDataBaseBasics>.GetService();
                var schoolSql = dataBaseService.GetSqlCommandByName("selectAllSchools");

                if (_conn == null) _conn = dataBaseService.GetConnection();
                _conn.Open();

                NpgsqlCommand command = new NpgsqlCommand(schoolSql.SqlCommand, _conn);
                var dr = command.ExecuteReader();

                if (!dr.HasRows) throw new ArgumentException("No schools found");

                result = DataBaseResultConversion.FormatResultList<SchoolModel>(dr);

                _conn.Close();
            }
            catch (ArgumentException ex)
            {
                ///TODO Put log Here
                if(_conn != null) _conn.Close();
            }
            catch (Exception ex)
            {
                ///TODO Put log Here
                if (_conn != null) _conn.Close();
            }

            return result;
        }

        public IList<SchoolsInPolygonModel> GetAllSchoolsInSelectedArea(IList<string[]> coordinates)
        {
            IList<SchoolsInPolygonModel> result = null;
            try
            {
                IList<double[]> geographics = CoordinatesParser.ConverListWebMercatorToGeographic(coordinates);

                var dataBaseService = SafeServiceLocator<IDataBaseBasics>.GetService();
                var sql = dataBaseService.GetSqlCommandByName("selectSchoolInPolygon");

                if (_conn == null) _conn = dataBaseService.GetConnection();
                _conn.Open();

                NpgsqlCommand command = new NpgsqlCommand(sql.SqlCommand, _conn);
                command.Parameters.Add(new NpgsqlParameter("coordinates", coordinates));

                NpgsqlDataReader dr = command.ExecuteReader();

                result = DataBaseResultConversion.FormatResultList<SchoolsInPolygonModel>(dr);
            }
            catch (Exception ex)
            {
                if (_conn != null) _conn.Close();
            }
            return result;
        }

        public IList<PointModel> GetSchoolGraphycsLayersPoints()
        {
            IList<PointModel> result = null;

            IModelToPointMapService mapService = SafeServiceLocator<IModelToPointMapService>.GetService();

            try
            {
                IList<SchoolModel> schools = GetAllSchools();
                if (schools == null) return null;
                IList<GenericPointModel> schoolPoints = Mapper.Map<IList<GenericPointModel>>(schools);
                result = mapService.ConvertListInGraplhicsPoints(schoolPoints, GraphycsLayerType.Schools);
            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }
}

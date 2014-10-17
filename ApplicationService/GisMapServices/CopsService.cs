using AutoMapper;
using Common;
using Common.Conversion;
using Data.DataBase;
using Model.DataBase;
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
    public class CopsService : ICopsService
    {
        NpgsqlConnection _conn = null;
        public IList<CopsModel> GetAllCops()
        {
            IList<CopsModel> result = null;

            try
            {
                var dataBaseService = SafeServiceLocator<IDataBaseBasics>.GetService();
                SqlsModel sql = dataBaseService.GetSqlCommandByName("selectAllCops");

                if (_conn == null) _conn = dataBaseService.GetConnection();

                _conn.Open();

                NpgsqlCommand command = new NpgsqlCommand(sql.SqlCommand, _conn);
                var dr = command.ExecuteReader();

                if (!dr.HasRows) throw new ArgumentException("No Cops found");

                result = DataBaseResultConversion.FormatResultList<CopsModel>(dr);

                _conn.Close();

            }
            catch (NpgsqlException npgsqlEx)
            {
                ///TODO Put Log
            }
            catch (Exception ex)
            {
                ///TODO Put log
                result = null;
                if (_conn != null) _conn.Close();
            }

            return result;
        }

        public IList<CopsInPolygonModel> GetAllCopsInSelectedArea(string coordinates)
        {
            throw new NotImplementedException();
        }

        public IList<PointModel> GetCopsGraphycsLayersPoints()
        {
            IList<PointModel> result = null;

            try
            {
                IList<CopsModel> cops = GetAllCops();
                if (cops == null) return null;
                IModelToPointMapService mapService = SafeServiceLocator<IModelToPointMapService>.GetService();
                IList<GenericPointModel> points = Mapper.Map<IList<GenericPointModel>>(cops);
                result = mapService.ConvertListInGraplhicsPoints(points, GraphycsLayerType.Cops);
            }
            catch (NpgsqlException npgsqlEx)
            {
                if (_conn != null) _conn.Close();
            }
            catch (Exception ex)
            {
                if (_conn != null) _conn.Close();
                result = null;
            }

            return result;
        }
    }
}

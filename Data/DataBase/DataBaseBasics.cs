using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Configuration;
using Model.DataBase;
using Common.Conversion;

namespace Data.DataBase
{
    public class DataBaseBasics : IDataBaseBasics
    {
        public NpgsqlConnection GetConnection()
        {
            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Postgre"].ConnectionString);
            }
            catch (NpgsqlException ex)
            {
                ///TODO Put log here
            }
            catch (Exception ex)
            {
                ///TODO Put log here
            }
            return conn;
        }

        public SqlsModel GetSqlCommandByName(string sqlName)
        {
            SqlsModel sql = null;
            NpgsqlConnection conn = null;
            try
            {
                conn = GetConnection();

                if (conn == null) throw new ArgumentException("Connection not initialized");

                conn.Open();

                string query = @"SELECT * FROM application_sqls WHERE sql_name like :sqlName";
                NpgsqlCommand command = new NpgsqlCommand(query, conn);
                command.Parameters.Add(new NpgsqlParameter("sqlName", sqlName));
                NpgsqlDataReader dr = command.ExecuteReader();

                if (!dr.HasRows)
                {
                    conn.Close();
                    throw new ArgumentException("Sql not found.");
                }

                sql = DataBaseResultConversion.FormatResult<SqlsModel>(dr);

                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                if (conn != null) conn.Close();
                throw ex;
            }
            catch (ArgumentException ex)
            {
                //TODO log
                if (conn != null) conn.Close();
            }
            catch (Exception ex)
            {
                ///TODO log
                if (conn != null) conn.Close();
            }

            return sql;
        }
    }


}

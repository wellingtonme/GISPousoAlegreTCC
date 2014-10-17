using Model.DataBase;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataBase
{
    public interface IDataBaseBasics
    {
        NpgsqlConnection GetConnection();
        SqlsModel GetSqlCommandByName(string sqlName);
    }
}

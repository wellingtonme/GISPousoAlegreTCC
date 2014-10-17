using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Conversion
{
    public static class DataBaseResultConversion
    {
        public static IList<T> FormatResultList<T>(NpgsqlDataReader dr)
        {
            IList<T> resultList = new List<T>();
            try
            {
                IList<string> list = new List<string>();

                do
                {
                    while (dr.Read())
                    {
                        for (int x = 0; x < dr.FieldCount; x++)
                        {
                            list.Add(dr[x].ToString());
                        }
                    }
                } while (dr.NextResult());

                Type typeOfT = (Type)typeof(T);
                var props = typeOfT.GetProperties();

                if (props.Length != (list.Count / dr.RecordsAffected)) throw new Exception("The number of properties and the number of columns returned in sql does not match");
                
                int skip = 0;
                for (int x = 0; x < dr.RecordsAffected; x++)
                {
                    IList<string> tempList = list.Skip(skip).Take(props.Length).ToList<string>();
                    T tclass = GetNewObject<T>();
                    if (tclass == null) throw new Exception("Impossible to instance the generic class.");
                    for (int y = 0; y < props.Length; y++)
                    {
                        var attr = props[y];
                        Type t = attr.PropertyType;
                        var value = Convert.ChangeType(tempList.ElementAt(y), t);
                        tclass.GetType().GetProperty(props[y].Name).SetValue(tclass, value, null);
                    }
                    skip += props.Length;
                    resultList.Add(tclass);
                }
            }
            catch (Exception ex)
            {
                ///TODO put log Here
            }

            return resultList;
        }

        public static T FormatResult<T>(NpgsqlDataReader dr)
        {
            T result = default(T);
            try
            {
                IList<string> list = new List<string>();

                while (dr.Read())
                {
                    for (int x = 0; x < dr.FieldCount; x++)
                    {
                        list.Add(dr[x].ToString());
                    }
                }

                Type typeOfT = (Type)typeof(T);
                var props = typeOfT.GetProperties();

                if (props.Length != list.Count) throw new Exception("The number of properties and the number of columns returned in sql does not match");
                
                T tclass = GetNewObject<T>();
                if (tclass == null) throw new Exception("Impossible to initialize the generic class.");
                for (int y = 0; y < props.Length; y++)
                {
                    var attr = props[y];
                    Type t = attr.PropertyType;
                    var value = Convert.ChangeType(list.ElementAt(y), t);
                    tclass.GetType().GetProperty(props[y].Name).SetValue(tclass, value, null);
                }

                result = tclass;
            }
            catch (Exception ex)
            {
                ///TODO put log Here
            }

            return result;
        }

        private static T GetNewObject<T>()
        {
            try
            {
                return (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
            }
            catch
            {
                return default(T);
            }
        }
    }
}

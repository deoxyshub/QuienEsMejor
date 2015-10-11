using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace WhoIsBetter.DataAccess
{
    public static class DataAccessExtension
    {
        
        public static string GetString(this SqlDataReader reader, string columnName)
        {
            object columnValue = reader[columnName];

            if (!(columnValue is DBNull))
                return (string)columnValue;

            return null;
        }
        
        public static T GetValue<T>(this SqlDataReader reader, string columnName) 
        {
            try
            {
                return (T)reader[columnName];
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(string.Format("Specified cast is not valid. (Column: {0})", columnName), ex);
            }
            

        }

        public static Nullable<T> GetNullableValue<T>(this SqlDataReader reader, string columnName) where T : struct
        {
            object columnValue = reader[columnName];

            if (!(columnValue is DBNull))
                return (T)columnValue;

            return null;
        }
        
        
        public static DataTable ToDataTable<T, E>(this ICollection<T> collection, Func<T, E> expression)
        {
            if (collection == null) return null;

            var dt = new DataTable();

            foreach (T item in collection)
            {

                var dr = dt.NewRow();
                var entity = expression(item);
                var properties = entity.GetType().GetProperties();

                foreach (var prop in properties) {
                    var type = prop.PropertyType.IsNullable() ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType;

                    if (!dt.Columns.Contains(prop.Name)) dt.Columns.Add(prop.Name, type);

                    dr.SetField(prop.Name, prop.GetValue(entity, null));
                }

                dt.Rows.Add(dr);
            }

            return collection.Count.Equals(0) ? null : dt;

        }

        
        public static  ICollection<E> ReadEntityCollection<E>(this SqlDataReader dr, Func<SqlDataReader, E> co)
            where E : class
        {
            List<E> list = new List<E>();

            while (dr.Read())
            {
                list.Add(co(dr));
            }
            return list;
        }

        public static  E ReadEntity<E>(this SqlDataReader dr, Func<SqlDataReader, E> co)
            where E : class
        {
            E entity = null;
            if (dr.Read())
            {
                entity = co(dr);
            }
            return entity;
        }
        
        public static bool IsNullable(this Type t)
        {
            if (t.FullName == "System.String") return false;

            return (!t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }

    }
}

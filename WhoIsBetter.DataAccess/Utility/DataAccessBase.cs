using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Concurrent;

namespace WhoIsBetter.DataAccess
{
    public abstract class DataAccessBase
    {

        private string _connectionStringName;
        private string _connectionString;
        public DataAccessBase(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        private SqlCommand PrepareCommand(string storedProcedure, params DataParameter[] parameters)
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(_connectionStringName, storedProcedure, _connectionString);

            // Assign the provided values to these parameters based on parameter name
            AssignParameterValues(commandParameters, parameters);

            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(storedProcedure, connection);
            cmd.Parameters.AddRange(commandParameters);
            cmd.CommandType = CommandType.StoredProcedure;

            return cmd;
        }
        private void AssignParameterValues(SqlParameter[] sqlParameters, DataParameter[] parameters)
        {
            bool pFound;
            for (int p = 0; p < parameters.Length; p++)
            {
                pFound = false;
                for (int s = 0; s < sqlParameters.Length; s++)
                {
                    if (sqlParameters[s].ParameterName == parameters[p].Name)
                    {
                        pFound = true;
                        sqlParameters[s].Value = parameters[p].Value;
                    }

                }
                if (!pFound)
                    throw new Exception(string.Format("Parameter {0} not found", parameters[p].Name));
            }
        }


        protected int ExecuteNonQuery(string storedProcedure, params DataParameter[] parameters)
        {
            int rowsAffected = 0;
            SqlCommand cmd = null;
            using (cmd = PrepareCommand(storedProcedure, parameters))
            {
                cmd.Connection.Open();
                try
                {
                    rowsAffected = cmd.ExecuteNonQuery();
                    UpdateDataParameters(cmd.Parameters, parameters);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }
            return rowsAffected;

        }

        protected SqlCommand GetCommand(string storedProcedure, params DataParameter[] parameters)
        {
            SqlCommand cmd = PrepareCommand(storedProcedure, parameters);
            cmd.Connection.Open();
            return cmd;
        }
        /// <remarks>Remember Close connection after use the SqlDataReader. This reader don't return output parameters</remarks>
        protected SqlDataReader ExecuteReaderSingle(string storedProcedure, params DataParameter[] parameters)
        {
            SqlCommand cmd = PrepareCommand(storedProcedure, parameters);
            cmd.Connection.Open();

            return cmd.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <param name="parameters"></param>
        /// <remarks>Remember Close connection after use the SqlDataReader. This reader don't return output parameters</remarks>
        /// <returns></returns>
        protected SqlDataReader ExecuteReader(string storedProcedure, params DataParameter[] parameters)
        {
            SqlCommand cmd = PrepareCommand(storedProcedure, parameters);
            cmd.CommandTimeout = 60;
            cmd.Connection.Open();

            return cmd.ExecuteReader(CommandBehavior.Default | CommandBehavior.CloseConnection);
        }

        protected DataTable ExecuteDataTable(string storedProcedure, params DataParameter[] parameters)
        {
            DataTable dt = null;
            using (SqlCommand cmd = PrepareCommand(storedProcedure, parameters))
            {
                cmd.CommandTimeout = 60;
                cmd.Connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                cmd.Connection.Close();
            }

            return dt;

        }

        protected DataSet ExecuteDataSet(string storedProcedure, params DataParameter[] parameters)
        {
            DataSet ds = null;
            using (SqlCommand cmd = PrepareCommand(storedProcedure, parameters))
            {
                cmd.CommandTimeout = 60;
                cmd.Connection.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                cmd.Connection.Close();
            }

            return ds;

        }

        void Connection_StateChange(object sender, StateChangeEventArgs e)
        {
            if (e.CurrentState == ConnectionState.Closed)
            {

            }
        }
        protected T ExecuteScalar<T>(string storedProcedure, params DataParameter[] parameters)
        {
            return (T)this.ExecuteScalar(storedProcedure, parameters);
        }

        protected object ExecuteScalar(string storedProcedure, params DataParameter[] parameters)
        {
            object returnedValue = null;
            using (SqlCommand cmd = PrepareCommand(storedProcedure, parameters))
            {
                cmd.Connection.Open();
                returnedValue = cmd.ExecuteScalar();
                cmd.Connection.Close();
                UpdateDataParameters(cmd.Parameters, parameters);
            }
            return returnedValue;
        }
        private void UpdateDataParameters(SqlParameterCollection sqlParameters, DataParameter[] parameters)
        {
            for (int i = 0; i < sqlParameters.Count; i++)
            {
                if (sqlParameters[i].Direction != ParameterDirection.Input)
                {
                    for (int j = 0; j < parameters.Length; j++)
                    {
                        if (parameters[j].Name == sqlParameters[i].ParameterName)
                        {
                            parameters[j].Value = sqlParameters[i].Value;
                            break;
                        }
                    }
                }
            }
        }



        protected ICollection<E> ExecuteEntityCollection<E>(string storedProcedure, Func<SqlDataReader, E> co, params DataParameter[] parameters)
            where E : class
        {
            SqlCommand cmd = this.GetCommand(storedProcedure, parameters);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            ICollection<E> list = dr.ReadEntityCollection<E>(co);
            dr.Close();
            UpdateDataParameters(cmd.Parameters, parameters);
            return list;
        }
        protected E ExecuteEntity<E>(string storedProcedure, Func<SqlDataReader, E> co, params DataParameter[] parameters)
            where E : class
        {
            SqlDataReader dr = null;
            E entity;
            try
            {
                SqlCommand cmd = this.GetCommand(storedProcedure, parameters);
                dr = cmd.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.CloseConnection);
                entity = dr.ReadEntity<E>(co);

                UpdateDataParameters(cmd.Parameters, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }

            return entity;
        }

    }
    public class DataParameter
    {
        string _name;
        object _value;

        public DataParameter(string parameterName, object value = null)
        {
            _name = parameterName;
            _value = value;
        }

        public string Name
        {
            get { return _name; }
        }
        public object Value
        {
            get
            {

                return (_value == DBNull.Value) ? null : _value;
            }
            set
            {
                _value = (value == null) ? DBNull.Value : value;
            }
        }
    }

    /// <summary>
    /// SqlHelperParameterCache provides functions to leverage a static cache of procedure parameters, and the
    /// ability to discover parameters for stored procedures at run-time.
    /// </summary>
    public sealed class SqlHelperParameterCache
    {
        #region private methods, variables, and constructors

        //Since this class provides only static methods, make the default constructor private to prevent 
        //instances from being created with "new SqlHelperParameterCache()"
        private SqlHelperParameterCache() { }

        public static void Clear()
        {
            paramCache.Clear();
        }
        public static void RemoveCacheItem(string dbKey, string spName)
        {
            if (dbKey == null || dbKey.Length == 0) throw new ArgumentNullException("dbKey");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            string hashKey = dbKey + ":" + spName;
            SqlParameter[] dummy;
            paramCache.TryRemove(hashKey, out dummy);
        }

        private static ConcurrentDictionary<string, SqlParameter[]> paramCache = new System.Collections.Concurrent.ConcurrentDictionary<string, SqlParameter[]>();


        /// <summary>
        /// Resolve at run time the appropriate set of SqlParameters for a stored procedure
        /// </summary>
        /// <param name="connection">A valid SqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">Whether or not to include their return value parameter</param>
        /// <returns>The parameter array discovered.</returns>
        private static SqlParameter[] DiscoverSpParameterSet(SqlConnection connection, string spName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            SqlCommand cmd = new SqlCommand(spName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            connection.Open();
            SqlCommandBuilder.DeriveParameters(cmd);
            connection.Close();
            CorrectTypeNameAssignments(cmd);
            cmd.Parameters.RemoveAt(0); //Remove Return Value Parameter

            SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count];
            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // Init the parameters with a DBNull value
            for (int i = 0; i < discoveredParameters.Length; i++)
            {
                discoveredParameters[i].Value = DBNull.Value;
            }
            return discoveredParameters;
        }

        private static void CorrectTypeNameAssignments(SqlCommand cmd)
        {
            foreach (SqlParameter parameter in cmd.Parameters)
            {
                if (parameter.SqlDbType != SqlDbType.Structured)
                {
                    continue;
                }
                string name = parameter.TypeName;
                int index = name.IndexOf(".");
                if (index == -1)
                {
                    continue;
                }
                name = name.Substring(index + 1);
                if (name.Contains("."))
                {
                    parameter.TypeName = name;
                }
            }
        }

        /// <summary>
        /// Deep copy of cached SqlParameter array
        /// </summary>
        /// <param name="originalParameters"></param>
        /// <returns></returns>
        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        #endregion private methods, variables, and constructors

        #region Parameter Discovery Functions


        /// <summary>
        /// Retrieves the set of SqlParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a SqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>An array of SqlParameters</returns>
        public static SqlParameter[] GetSpParameterSet(string dbKey, string spName, string connectionString)
        {
            if (dbKey == null || dbKey.Length == 0) throw new ArgumentNullException("dbKey");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string hashKey = dbKey + ":" + spName;

                SqlParameter[] cachedParameters;

                if (!paramCache.TryGetValue(hashKey, out cachedParameters))
                {
                    SqlParameter[] spParameters = DiscoverSpParameterSet(connection, spName);
                    foreach (SqlParameter p in spParameters)
                    {
                        p.Value = null; //Use default value in storeprocedure
                    }
                    cachedParameters = spParameters;
                    paramCache.TryAdd(hashKey, spParameters);

                }

                return CloneParameters(cachedParameters);

            }
        }


        #endregion Parameter Discovery Functions

    }
}
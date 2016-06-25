using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyChangeControlLib.Types;
using System.Data;
using SpotifyChangeControlLib.DataObjects;

namespace SpotifyChangeControlLib.DatabaseObjects
{
    /// <summary>
    /// This class will be used to access the database layer 
    /// directly by using its functions. 
    /// </summary>
    internal static class SpotifyDatabase
    {
        private static string _ConnectionString;
        private static string _Database;
        private static string _DataSource;
        private static SqlConnection _oSqlConnection;
        private static int _BatchThreshold;

        //Make sure that certain one time processes have been done
        private static bool _InitCompleted = false;


         public  static void Init(string sConnectionString, int iBatchThreshold = 1000)
        {
            SpotifyDatabase._ConnectionString = sConnectionString;
            SpotifyDatabase._oSqlConnection = new SqlConnection(SpotifyDatabase._ConnectionString);
            SpotifyDatabase._DataSource = SpotifyDatabase._oSqlConnection.DataSource;
            SpotifyDatabase._Database = SpotifyDatabase._oSqlConnection.Database;
            SpotifyDatabase._BatchThreshold = iBatchThreshold;
            

        }

        private static void CheckInit()
        {
            if (!SpotifyDatabase._InitCompleted)
            {
                throw new Exception("The init function has not been called. This type 'SpotifyDatabase' must have it's init function called before being used!");
            }
        }

        internal static void ExecuteNonQuery(string sCommandText)
        {
            try
            {
                CheckInit();
                using (SqlCommand oSqlCommand = new SqlCommand(sCommandText, SpotifyDatabase._oSqlConnection))
                {
                    oSqlCommand.CommandType = System.Data.CommandType.Text;
                    if (_oSqlConnection.State != System.Data.ConnectionState.Open)
                    {
                        _oSqlConnection.Open();
                    }
                    oSqlCommand.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_oSqlConnection.State == System.Data.ConnectionState.Open)
                {
                    _oSqlConnection.Close();
                }
            }
        }

        internal static void BulkInsert(DataTable oDataTable, TableState oDatabaseTableInfo)
        {
            try
            {
                using (SqlBulkCopy oSqlBulkCopy = new SqlBulkCopy(SpotifyDatabase._oSqlConnection, SqlBulkCopyOptions.TableLock, null))
                {
                    if (_oSqlConnection.State != System.Data.ConnectionState.Open)
                    {
                        _oSqlConnection.Open();
                    }
                    oSqlBulkCopy.BatchSize = SpotifyDatabase._BatchThreshold;
                    oSqlBulkCopy.DestinationTableName = oDatabaseTableInfo.TableName;
                    oSqlBulkCopy.WriteToServer(oDataTable);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_oSqlConnection.State == System.Data.ConnectionState.Open)
                {
                    _oSqlConnection.Close();
                }
            }
        }

        internal static DataSet ExecuteStoredProcedure(string sProcedure, params SqlParameter[] oSqlParameters)
        {
           
            try
            {
                DataSet oDataSet = new DataSet();

                using (SqlCommand oSqlCommand = new SqlCommand(sProcedure, SpotifyDatabase._oSqlConnection))
                {

                    oSqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    if (oSqlParameters.Length > 0)
                    {
                        oSqlCommand.Parameters.AddRange(oSqlParameters);
                    }
                    using (SqlDataAdapter oSqlDataAdapter = new SqlDataAdapter(oSqlCommand))
                    {
                        if (_oSqlConnection.State != System.Data.ConnectionState.Open)
                        {
                            _oSqlConnection.Open();
                        }
                        oSqlDataAdapter.Fill(oDataSet);
                    }
                    return oDataSet;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_oSqlConnection.State == System.Data.ConnectionState.Open)
                {
                    _oSqlConnection.Close();
                }
            }
        }

        internal static DataTable ExecuteStoredProcedure(string sProcedure, string sDataTableName = "Table1", params SqlParameter[] oSqlParameters)
        {
            try
            {
                DataTable oDataTable = null;
                DataSet oDataSet = ExecuteStoredProcedure(sProcedure, oSqlParameters);
                if (oDataSet.Tables.Count > 0)
                {
                    oDataTable = oDataSet.Tables[0];
                    oDataTable.TableName = sDataTableName;
                }

                return oDataTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

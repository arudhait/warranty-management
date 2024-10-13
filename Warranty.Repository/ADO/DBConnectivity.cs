using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Warranty.Common.Utility;

namespace Warranty.Repository.ADO
{
    public class DBConnectivity
    {
        public SqlConnection con;
        public SqlDataAdapter Da;
        public SqlCommand Cmd;
        public SqlCommandBuilder Cb;
        public DBConnectivity()
        {

        }
        public void OpenConnection()
        {
            try
            {
                con = new SqlConnection();
                con.ConnectionString = AppCommon.ConnectionString;
                if ((con.State == ConnectionState.Open)) con.Close();
                con.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetDataFromSP(List<StoredProcModel> parm, string CommandText, ref int totalRecords)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (parm != null && parm.Count > 0)
                {
                    foreach (var item in parm)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }
                DataSet ds = new DataSet();
                DataSet ds2 = new DataSet();
                DataTable dt = new DataTable();
                OpenConnection();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = CommandText;
                Da = new SqlDataAdapter(cmd);
                Da.Fill(ds);
                if (ds.Tables.Count > 1)
                    totalRecords = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Da.Dispose();
                CloseConnection();
                cmd.Dispose();
            }
        }
        public DataTable GetDataFromSP(List<StoredProcModel> parm, string CommandText)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (parm != null && parm.Count > 0)
                {
                    foreach (var item in parm)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }
                DataSet ds = new DataSet();
                DataSet ds2 = new DataSet();
                DataTable dt = new DataTable();
                OpenConnection();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = CommandText;
                Da = new SqlDataAdapter(cmd);
                Da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Da.Dispose();
                CloseConnection();
                cmd.Dispose();
            }
        }
        public string GetDataFromFunction(List<StoredProcModel> parm, string functionName)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                string functionReturnValue = "";
                OpenConnection();
                string functionParameters = "";
                if (parm != null && parm.Count > 0)
                    functionParameters = string.Join(",", parm.Select(x => "'" + x.Value + "'").ToList());
                cmd = new SqlCommand($"SELECT DBO.{functionName}({functionParameters})", con);
                cmd.CommandType = CommandType.Text;
                functionReturnValue = cmd.ExecuteScalar().ToString();
                return functionReturnValue;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
                cmd.Dispose();
            }
        }

        public DataTable GetDataFromSP(List<StoredProcModel> parm, string CommandText, ref int totalRecords, ref DataTable table2)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (parm != null && parm.Count > 0)
                {
                    foreach (var item in parm)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }
                DataSet ds = new DataSet();
                DataSet ds2 = new DataSet();
                DataTable dt = new DataTable();
                OpenConnection();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = CommandText;
                Da = new SqlDataAdapter(cmd);
                Da.Fill(ds);
                if (ds.Tables.Count >= 2)
                    totalRecords = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                if (ds.Tables.Count >= 3)
                {
                    table2 = ds.Tables[2];
                }
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Da.Dispose();
                CloseConnection();
                cmd.Dispose();
            }
        }
        public DataTable GetDataFromSP(List<StoredProcModel> parm, string CommandText, ref DataTable table2)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (parm != null && parm.Count > 0)
                {
                    foreach (var item in parm)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }
                DataSet ds = new DataSet();
                DataSet ds2 = new DataSet();
                DataTable dt = new DataTable();
                OpenConnection();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = CommandText;
                Da = new SqlDataAdapter(cmd);
                Da.Fill(ds);
                if (ds.Tables.Count >= 2)
                    table2 = ds.Tables[1];
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Da.Dispose();
                CloseConnection();
                cmd.Dispose();
            }
        }
        public int ExecSP(List<StoredProcModel> parm, string CommandText)
        {
            SqlCommand cmd = new SqlCommand();
            int retval;
            try
            {
                if (parm != null && parm.Count > 0)
                {
                    foreach (var item in parm)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }

                OpenConnection();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = CommandText;
                retval = cmd.ExecuteNonQuery();
                cmd.Dispose();
                CloseConnection();
                return retval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
                cmd.Dispose();
            }
        }
        public void BulkInsertToDataBase(DataTable insertTable, string tableName)
        {
            OpenConnection();
            //creating object of SqlBulkCopy  
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            try
            {
                //assigning Destination table name  
                objbulk.DestinationTableName = tableName;
                var columnName = insertTable.Columns;
                //Mapping Table column
                for (int i = 0; i < columnName.Count; i++)
                {
                    objbulk.ColumnMappings.Add(columnName[i].ColumnName, columnName[i].ColumnName);
                }
                //inserting bulk Records into DataBase   
                objbulk.WriteToServer(insertTable);
            }
            catch (Exception ex)
            {
                string message = string.Empty;
                if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                {
                    string pattern = @"\d+";
                    Match match = Regex.Match(ex.Message.ToString(), pattern);
                    var index = Convert.ToInt32(match.Value) - 1;
                    FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                    var sortedColumns = fi.GetValue(objbulk);
                    var items = (Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);
                    FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                    var metadata = itemdata.GetValue(items[index]);
                    var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                    var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                    message = String.Format("Column: {0} contains data with a length greater than: {1}", column, length);
                }
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}

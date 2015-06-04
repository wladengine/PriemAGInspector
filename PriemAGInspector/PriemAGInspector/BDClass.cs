using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PriemAGInspector
{
    class BDClass
    {
        private SqlConnection _conn;
        
        public BDClass(string connectionString)
        {
            _conn = new SqlConnection(connectionString);
        }

        public DataTable GetDataTable(string query, Dictionary<string, object> _params)
        {
            DataTable tbl = new DataTable();
            try
            {
                _conn.Open();
                using (SqlCommand command = new SqlCommand(query, _conn))
                {
                    if (_params != null)
                    {
                        foreach (var prm in _params)
                            command.Parameters.Add(new SqlParameter(prm.Key, prm.Value));
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(tbl);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _conn.Close();
            }
            return tbl;
        }
        public object GetValue(string query, Dictionary<string, object> _params)
        {
            try
            {
                _conn.Open();
                using (SqlCommand command = new SqlCommand(query, _conn))
                {
                    if (_params != null)
                    {
                        foreach (var prm in _params)
                            command.Parameters.Add(new SqlParameter(prm.Key, prm.Value));
                    }

                    return command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                _conn.Close();
            }
        }
        public int ExecuteQuery(string query, Dictionary<string, object> _params)
        {
            try
            {
                _conn.Open();
                using (SqlCommand command = new SqlCommand(query, _conn))
                {
                    if (_params != null)
                    {
                        foreach (var prm in _params)
                            command.Parameters.Add(new SqlParameter(prm.Key, prm.Value));
                    }

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
            finally
            {
                _conn.Close();
            }
        }
    }
}

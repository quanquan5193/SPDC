using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace SPDC.Common
{
    public class SystemParameterProvider
    {
        private static SystemParameterProvider instance;

        private readonly string _connectionString;
        private static List<SystemParameterInfo> _lstParams;
        private readonly string CacheKey = "SystemParameterCached";

        private SystemParameterProvider(string connectionName)
        {
            _connectionString = connectionName;
            _lstParams = new List<SystemParameterInfo>();

            LoadData();
        }

        public static void Initialize(string connectionString)
        {
            if (instance == null)
            {
                instance = new SystemParameterProvider(connectionString);
            }
        }

        public static SystemParameterProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new Exception("Please Initialize first");
                }

                return instance;
            }
        }

        public int GetValueInt(string key)
        {
            var info = _lstParams.FirstOrDefault(i => i.Key == key);

            if (info != null)
            {
                return info.ValueInt;
            }

            return 0;
        }

        public double GetValueDouble(string key)
        {
            var info = _lstParams.FirstOrDefault(i => i.Key == key);

            if (info != null)
            {
                return info.ValueDouble;
            }

            return 0.0;
        }

        public string GetValueString(string key)
        {
            var info = _lstParams.FirstOrDefault(i => i.Key == key);

            if (info != null)
            {
                return info.ValueString;
            }

            return string.Empty;
        }

        private void LoadData()
        {
            var cached = HttpContext.Current.Cache.Get(CacheKey);

            if (cached != null)
            {
                _lstParams = (List<SystemParameterInfo>)cached;
            }
            else
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM dbo.CommonData";
                    cmd.CommandType = System.Data.CommandType.Text;
                    var reader = cmd.ExecuteReader();

                    try
                    {
                        while (reader.Read())
                        {
                            var sp = new SystemParameterInfo();

                            sp.Key = reader["Key"].ToString();
                            sp.ValueDouble = (double)reader["ValueDouble"];
                            sp.ValueString = reader["ValueString"] != null ? reader["ValueString"].ToString() : string.Empty;
                            sp.ValueInt = (int)reader["ValueInt"];

                            _lstParams.Add(sp);
                        }

                        HttpContext.Current.Cache.Add(CacheKey, _lstParams, null, DateTime.Now.AddMinutes(60), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }

                }
            }
        }
    }
}

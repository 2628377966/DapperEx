using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

using Dapper;
using MySql.Data.MySqlClient;
using System.Data.OleDb;


namespace DBHelperDapper
{
    public class DBHelper
    {
        public static int DeleteModel<T>(ref T model)
        {
            return DeleteModel<T>(typeof(T).Name, ref model);
        }

        public static int DeleteModel<T>(ref T model, params string[] KeyColumn)
        {
            return DeleteModel<T>(typeof(T).Name, ref model, KeyColumn);
        }

        public static int DeleteModel<T>(string TableName, ref T model)
        {
            return DeleteModel<T>(TableName, ref model, new string[] { "ID" });
        }

        public static int DeleteModel<T>(string TableName, ref T model, params string[] KeyColumn)
        {
            int num = -1;
            string sql = "";
            StringBuilder builder = new StringBuilder("delete " + TableName);
            builder.Append(" where ");
            bool flag = false;
            foreach (string str2 in KeyColumn)
            {
                flag = true;
                builder.Append(str2 + "=?" + str2);
                builder.Append(" and ");
            }
            if (flag)
            {
                sql = builder.Remove(builder.Length - 5, 5).ToString();
            }
            else
            {
                sql = builder.ToString();
            }
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams((T)model);
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                num = connection.Execute(sql, param, null, null, null);
                connection.Close();
            }
            return num;
        }

        public static int Execute(string sqlCommandText, object Params)
        {
            int num = -1;
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                num = connection.Execute(sqlCommandText, Params, null, null, null);
                connection.Close();
            }
            return num;
        }

        public static T ExecuteScaler<T>(string sqlCommandText, object Params)
        {
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(Params);
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                T local = connection.Query<T>(sqlCommandText, param, null, true, null, null).First<T>();
                connection.Close();
                return local;
            }
        }

        public static List<T> GetAllModelList<T>()
        {
            return GetModelListByModelTable<T>(null, null);
        }

        public static T GetModel<T>(object KeyColumn) where T : new()
        {
            return GetModel<T>((default(T) == null) ? Activator.CreateInstance<T>() : default(T), KeyColumn);
        }

        public static T GetModel<T>(T model, object KeyColumn) where T : new()
        {
            return GetModel<T>(typeof(T).Name, model, KeyColumn);
        }

        public static T GetModel<T>(string TableName, object KeyColumn) where T : new()
        {
            return GetModel<T>(TableName, (default(T) == null) ? Activator.CreateInstance<T>() : default(T), KeyColumn);
        }

        public static T GetModel<T>(string TableName, T model, object KeyColumn) where T : new()
        {
            T local = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
            StringBuilder builder = new StringBuilder("select ");
            builder.Append(" * ");
            builder.Append(" from " + TableName);
            if (KeyColumn != null)
            {
                Type type = KeyColumn.GetType();
                builder.Append(" where ");
                foreach (PropertyInfo info in type.GetProperties())
                {
                    string name = info.Name;
                    builder.Append(name.TrimStart(new char[] { '?' }) + "=?" + name + " and ");
                }
                builder.Remove(builder.Length - 5, 5);
            }
            List<T> modelList = GetModelList<T>(builder.ToString(), KeyColumn);
            if ((modelList != null) && (modelList.Count > 0))
            {
                return modelList[0];
            }
            return default(T);
        }

        public static List<T> GetModelList<T>(string sql)
        {
            List<T> list = new List<T>();
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                list = connection.Query<T>(sql, null, null, true, null, null).AsList<T>();
                connection.Close();
            }
            return list;
        }

        public static List<T> GetModelList<T>(string sql, object Param)
        {
            List<T> list = new List<T>();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(Param);
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                list = connection.Query<T>(sql, param, null, true, null, null).AsList<T>();
                connection.Close();
            }
            return list;
        }

        public static List<T> GetModelList<U, V, T>(string sql, System.Func<U, V, T> map, object Param)
        {
            List<T> list = new List<T>();
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                list = connection.Query<U, V, T>(sql, map, Param, null, true, "Id", null, null).AsList<T>();
                connection.Close();
            }
            return list;
        }

        public static List<T> GetModelList<T>(string sql, object Param, CommandType Type)
        {
            List<T> list = new List<T>();
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams(Param);
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                list = connection.Query<T>(sql, param, null, true, null, new CommandType?(Type)).AsList<T>();
                connection.Close();
            }
            return list;
        }

        public static List<T> GetModelList<T>(string TableName, string Where, object Param)
        {
            StringBuilder builder = new StringBuilder("select * from ");
            builder.Append(TableName);
            if ((Where != null) && (Where != ""))
            {
                builder.Append(" where ");
                builder.Append(Where);
            }
            return GetModelList<T>(builder.ToString(), Param);
        }

        public static List<T> GetModelList<T>(string TableName, string Where, object Param, CommandType Type)
        {
            StringBuilder builder = new StringBuilder("select * from ");
            builder.Append(TableName);
            if ((Where != null) && (Where != ""))
            {
                builder.Append(" where ");
                builder.Append(Where);
            }
            return GetModelList<T>(builder.ToString(), Param, Type);
        }

        public static List<T> GetModelListByModelTable<T>(string Where, object Param)
        {
            return GetModelList<T>(typeof(T).Name, Where, Param);
        }

        public static List<T> GetModelListPage<T>(string sqlCommandText, string OrderBy, int startIndex, int endIndex, object Params)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM ( ");
            builder.Append(" SELECT ROW_NUMBER() OVER (");
            if (string.IsNullOrEmpty(OrderBy.Trim()))
            {
                throw new Exception("排序字段不能为空");
            }
            builder.Append("order by T." + OrderBy);
            builder.Append(")AS Row, T.*  from (");
            builder.Append(sqlCommandText);
            builder.Append(") as T ) TT");
            builder.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return GetModelList<T>(builder.ToString(), Params);
        }

        public static List<T> GetModelListPageByTBName<T>(string Where, string OrderBy, int startIndex, int endIndex, object Params = null)
        {
            return GetModelListPageByTBName<T>(typeof(T).Name, Where, OrderBy, startIndex, endIndex, Params);
        }

        public static List<T> GetModelListPageByTBName<T>(string TableName, string Where, string OrderBy, int startIndex, int endIndex, object Params = null)
        {
            StringBuilder builder = new StringBuilder("select * from " + TableName);
            if ((Where != null) && (Where != ""))
            {
                builder.Append(" where ");
                builder.Append(Where);
            }
            builder.Append(" order by ");
            builder.Append(OrderBy);
            builder.Append(" limit ");
            builder.Append(startIndex);
            builder.Append(",");
            builder.Append((int)(endIndex - startIndex));
            return GetModelList<T>(builder.ToString(), Params);
        }

        public static int GetRecordCount(string sqlCommandText, object Params)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select count(1) FROM (");
            builder.Append(sqlCommandText);
            builder.Append(") as TB");
            return ExecuteScaler<int>(builder.ToString(), Params);
        }

        public static int GetRecordCountByTBName<T>(string Where, object Params)
        {
            return GetRecordCountByTBName(typeof(T).Name, Where, Params);
        }

        public static int GetRecordCountByTBName(string TableName, string Where, object Params)
        {
            StringBuilder builder = new StringBuilder("select * from " + TableName);
            if ((Where != null) && (Where != ""))
            {
                builder.Append(" where ");
                builder.Append(Where);
            }
            return GetRecordCount(builder.ToString(), Params);
        }

        public static int InsertModel<T>(ref T model) where T : class
        {
            return InsertModel<T>(typeof(T).Name, ref model);
        }

        public static int InsertModel<T>(string[] AutoUpdateColumnName, ref T model) where T : class
        {
            return InsertModel<T>(typeof(T).Name, AutoUpdateColumnName, ref model);
        }

        public static int InsertModel<T>(string TableName, ref T model)
        {
            return InsertModel<T>(TableName, new string[] { "id" }, ref model);
        }

        public static int InsertModel<T>(string TableName, string[] AutoUpdateColumnName, ref T model)
        {
            int num = -1;
            string str = "";
            object obj2 = BMACache.Get("InsertSql");
            PropertyInfo[] properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            MethodInfo[] methods = model.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
            if ((obj2 == null) || !((Dictionary<MethodInfo[], string>)obj2).Keys.Contains<MethodInfo[]>(methods))
            {
                Dictionary<MethodInfo[], string> dictionary;
                StringBuilder builder = new StringBuilder("insert into " + TableName);
                StringBuilder builder2 = new StringBuilder();
                foreach (PropertyInfo info in properties)
                {
                    IEnumerable<Attribute> customAttributes = info.GetCustomAttributes(typeof(ModelAttribute));
                    if (((customAttributes.Count<Attribute>() > 0) && (customAttributes.First<Attribute>().GetType() == typeof(ModelAttribute))) && (((ModelAttribute)customAttributes.First<Attribute>()).Name == ModelAttributeType.TableColumn))
                    {
                        string str2 = info.Name.ToLower();
                        if ((AutoUpdateColumnName == null) || !AutoUpdateColumnName.Contains<string>(str2))
                        {
                            builder2.Append(str2);
                            builder2.Append(",");
                        }
                    }
                }
                builder.Append("(");
                builder.Append(builder2.ToString().TrimEnd(new char[] { ',' }));
                builder.Append(") values(?");
                builder.Append(builder2.ToString().TrimEnd(new char[] { ',' }).Replace(",", ",?"));
                builder.Append(")");
                if ((AutoUpdateColumnName != null) && (AutoUpdateColumnName.Length > 0))
                {
                    builder.Append(";select ?NewInsertID=SCOPE_IDENTITY()");//;select @@IDENTITY
                    //builder.Append(";select @@IDENTITY");
                }
                str = builder.ToString();
                if (obj2 == null)
                {
                    dictionary = new Dictionary<MethodInfo[], string>();
                }
                else
                {
                    dictionary = (Dictionary<MethodInfo[], string>)obj2;
                }
                dictionary.Add(methods, str);
                BMACache.Insert("InsertSql", dictionary);
            }
            else
            {
                str = ((Dictionary<MethodInfo[], string>)obj2)[methods];
            }
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams((T)model);
            if ((AutoUpdateColumnName != null) && (AutoUpdateColumnName.Length > 0))
            {
                param.Add("?NewInsertID", null, DbType.Int32, ParameterDirection.Output, null);
            }
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                num = connection.Execute(str, param, null, null, null);
                //num = connection.Query<int>(str, param, null, true, null, null).First<int>();
                //num = connection.Query(str, param, null, null, null);
                connection.Close();
            }
            if (((AutoUpdateColumnName != null) && (AutoUpdateColumnName.Length > 0)) && (num > 0))
            {
                num = param.Get<int>("?NewInsertID");
            }
            return num;
        }

        public static int InsertModelUnAutoUpdate<T>(ref T model)
        {
            return InsertModelUnAutoUpdate<T>(typeof(T).Name, ref model);
        }

        public static int InsertModelUnAutoUpdate<T>(string TableName, ref T model)
        {
            return InsertModel<T>(TableName, null, ref model);
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, System.Func<TFirst, TSecond, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = new int?(), CommandType? commandType = new CommandType?())
        {
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                return connection.Query<TFirst, TSecond, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, null);
            }
        }

        public static int UpdateModel<T>(ref T model)
        {
            return UpdateModel<T>(typeof(T).Name, ref model);
        }

        public static int UpdateModel<T>(string TableName, ref T model)
        {
            return UpdateModel<T>(TableName, ref model, new string[] { "id" }, new string[] { "ID" });
        }

        public static int UpdateModel<T>(ref T model, string[] AutoUpdate, params string[] KeyColumn)
        {
            return UpdateModel<T>(typeof(T).Name, ref model, AutoUpdate, KeyColumn);
        }

        public static int UpdateModel<T>(string TableName, ref T model, string[] AutoUpdate, params string[] KeyColumn)
        {
            int num = -1;
            string str = "";
            object obj2 = BMACache.Get("UpdateSql");
            PropertyInfo[] properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            MethodInfo[] methods = model.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
            if ((obj2 == null) || !((Dictionary<MethodInfo[], string>)obj2).Keys.Contains<MethodInfo[]>(methods))
            {
                Dictionary<MethodInfo[], string> dictionary;
                StringBuilder builder = new StringBuilder("update " + TableName + " set ");
                StringBuilder builder2 = new StringBuilder();
                foreach (PropertyInfo info in properties)
                {
                    IEnumerable<Attribute> customAttributes = info.GetCustomAttributes(typeof(ModelAttribute));
                    if (((customAttributes.Count<Attribute>() > 0) && (customAttributes.First<Attribute>().GetType() == typeof(ModelAttribute))) && (((ModelAttribute)customAttributes.First<Attribute>()).Name == ModelAttributeType.TableColumn))
                    {
                        string str2 = info.Name.ToLower();
                        if ((AutoUpdate == null) || !AutoUpdate.Contains<string>(str2))
                        {
                            builder2.Append(str2 + "=?" + str2 + ",");
                        }
                    }
                }
                builder.Append(builder2.ToString().TrimEnd(new char[] { ',' }));
                builder.Append(" where ");
                bool flag = false;
                foreach (string str3 in KeyColumn)
                {
                    flag = true;
                    builder.Append(str3 + "=?" + str3);
                    builder.Append(" and ");
                }
                if (flag)
                {
                    str = builder.Remove(builder.Length - 5, 5).ToString();
                }
                else
                {
                    str = builder.ToString();
                }
                if (obj2 == null)
                {
                    dictionary = new Dictionary<MethodInfo[], string>();
                }
                else
                {
                    dictionary = (Dictionary<MethodInfo[], string>)obj2;
                }
                dictionary.Add(methods, str);
                BMACache.Insert("UpdateSql", dictionary);
            }
            else
            {
                str = ((Dictionary<MethodInfo[], string>)obj2)[methods];
            }
            DynamicParameters param = new DynamicParameters();
            param.AddDynamicParams((T)model);
            using (IDbConnection connection = GetMySqlOpenConnection(false))
            {
                num = connection.Execute(str, param, null, null, null);
                connection.Close();
            }
            return num;
        }



        private static MySqlConnection GetMySqlOpenConnection(bool mars = false)
        {
            string sqlConnectionString = DBHelper.sqlConnectionString;
            if (mars)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(sqlConnectionString)
                {
                    MultipleActiveResultSets = true
                };
                sqlConnectionString = builder.ConnectionString;
            }
            MySqlConnection connection = new MySqlConnection(sqlConnectionString);
            connection.Open();
            return connection;
        }

        private static IDbConnection GetOpenConnection(bool mars = false)
        {
            string sqlConnectionString = DBHelper.sqlConnectionString;
            if (mars)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(sqlConnectionString)
                {
                    MultipleActiveResultSets = true
                };
                sqlConnectionString = builder.ConnectionString;
            }
            OleDbConnection connection = new OleDbConnection(sqlConnectionString);
            connection.Open();
            return connection;
        }

        private static string sqlConnectionString
        {
            get
            {
                return PubConstant.ConnectionString;
            }
        }
    
    }
}


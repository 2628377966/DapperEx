using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DapperEx
{
    /// <summary>
    /// 初始化数据库连接对象
    /// </summary>
    public class DbBase : IDisposable
    {
        private string paramPrefix = "@";
        private string providerName = "System.Data.SqlClient";
        private IDbConnection dbConnecttion;
        private DbProviderFactory dbFactory;
        private DBType _dbType = DBType.SqlServer;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public IDbConnection DbConnecttion
        {
            get
            {
                return dbConnecttion;
            }
        }

        /// <summary>
        /// 开始数据库事务
        /// </summary>
        public IDbTransaction DbTransaction
        {
            get
            {
                return dbConnecttion.BeginTransaction();
            }
        }

        /// <summary>
        /// 参数前缀
        /// </summary>
        public string ParamPrefix
        {
            get
            {
                return paramPrefix;
            }
        }

        /// <summary>
        /// 数据库提供者名
        /// </summary>
        public string ProviderName
        {
            get
            {
                return providerName;
            }
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DBType DbType
        {
            get
            {
                return _dbType;
            }
        }

        /// <summary>
        /// 初始化DbBase
        /// </summary>
        /// <param name="connectionStringName">connectionString名</param>
        public DbBase(string connectionStringName)
        {
            ConnectionStringSettings Css = ConfigurationManager.ConnectionStrings[connectionStringName];
            var connStr = Css.ConnectionString;
            if (!string.IsNullOrEmpty(Css.ProviderName))
            {
                providerName = Css.ProviderName;
            }
            else
            {
                throw new Exception("ConnectionStrings中没有配置提供程序ProviderName！");
            }
            dbFactory = DbProviderFactories.GetFactory(providerName);
            dbConnecttion = dbFactory.CreateConnection();
            dbConnecttion.ConnectionString = connStr;
            dbConnecttion.Open();
            SetParamPrefix();
        }
        /// <summary>
        /// 初始化DbBase
        /// </summary>
        /// <param name="connectionStringName"></param>
        public DbBase(string strConnection, string providerName)
        {
            this.providerName = providerName;
            dbFactory = DbProviderFactories.GetFactory(providerName);
            dbConnecttion = dbFactory.CreateConnection();
            dbConnecttion.ConnectionString = strConnection;
            dbConnecttion.Open();
            SetParamPrefix();
        }


        private void SetParamPrefix()
        {
            string dbtype = (dbFactory == null ? dbConnecttion.GetType() : dbFactory.GetType()).Name;

            // 使用类型名判断
            if (dbtype.StartsWith("MySql")) _dbType = DBType.MySql;
            else if (dbtype.StartsWith("SqlCe")) _dbType = DBType.SqlServerCE;
            else if (dbtype.StartsWith("Npgsql")) _dbType = DBType.PostgreSQL;
            else if (dbtype.StartsWith("Oracle")) _dbType = DBType.Oracle;
            else if (dbtype.StartsWith("SQLite")) _dbType = DBType.SQLite;
            else if (dbtype.StartsWith("System.Data.SqlClient.")) _dbType = DBType.SqlServer;
            // else try with provider name
            else if (providerName.IndexOf("MySql", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.MySql;
            else if (providerName.IndexOf("SqlServerCe", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.SqlServerCE;
            else if (providerName.IndexOf("Npgsql", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.PostgreSQL;
            else if (providerName.IndexOf("Oracle", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.Oracle;
            else if (providerName.IndexOf("SQLite", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.SQLite;

            if (_dbType == DBType.MySql && dbConnecttion != null && dbConnecttion.ConnectionString != null)// && dbConnecttion.ConnectionString.IndexOf("Allow User Variables=true") >= 0)
                paramPrefix = "?";
            if (_dbType == DBType.Oracle)
                paramPrefix = ":";
        }

        public void Dispose()
        {
            if (dbConnecttion != null)
            {
                try
                {
                    dbConnecttion.Dispose();
                }
                catch { }
            }
        }
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DBType
    {
        SqlServer,
        SqlServerCE,
        MySql,
        PostgreSQL,
        Oracle,
        SQLite
    }
}
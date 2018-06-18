using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Db2Code.Library
{
    public class Mssql 
    {
        private string server = "";
        private string db = "master";
        private bool isAuth = false;
        private string username = "";
        private string password = "";

        public List<Poco.Type> Types { get; set; } = new List<Poco.Type>();

        public Mssql(string server, string username, string password)
        {
            this.server = server;
            this.db = "master";
            this.isAuth = false;
            this.username = username;
            this.password = password;

            MssqlHelper.DefaultConnectionString = $"Server={server};Database={db};User Id={username};Password={password};";

            this.GetTypes();
        }

        public Mssql(string server)
        {
            this.server = server;
            this.db = "master";
            this.isAuth = true;
            this.username = "";
            this.password = "";

            MssqlHelper.DefaultConnectionString = $"Server={server};Database={db};Trusted_Connection=True;";

            this.GetTypes();
        }

        private void GetTypes()
        {
            this.Types = new List<Poco.Type>();
            try
            {
                var query = $"SELECT system_type_id, name FROM sys.types ORDER BY system_type_id";

                var dt = MssqlHelper.ExecuteQuery(query);
                foreach (DataRow dr in dt.Rows)
                {
                    var type = new Poco.Type();
                    type.Id = int.Parse(dr["system_type_id"].ToString());
                    type.Name = dr["name"].ToString();

                    Types.Add(type);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Poco.DataBase> GetDatabases()
        {
            var lstDataBases = new List<Poco.DataBase>();
            try
            {
                var query = $"SELECT database_id, name FROM sys.databases";

                var dt = MssqlHelper.ExecuteQuery(query);
                foreach (DataRow dr in dt.Rows)
                {
                    var db = new Poco.DataBase();
                    db.Id = int.Parse(dr["database_id"].ToString());
                    db.Name = dr["name"].ToString();

                    lstDataBases.Add(db);
                }

                return lstDataBases;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Poco.Table> GetTables(Poco.DataBase database)
        {
            var lstTables = new List<Poco.Table>();
            try
            {
                this.db = database.Name;

                if (isAuth)
                    MssqlHelper.DefaultConnectionString = $"Server={server};Database={db};Trusted_Connection=True;";
                else
                    MssqlHelper.DefaultConnectionString = $"Server={server};Database={db};User Id={username};Password={password};";

                var query = $"SELECT object_id, name FROM sys.Tables ORDER BY name";

                var dt = MssqlHelper.ExecuteQuery(query);
                foreach (DataRow dr in dt.Rows)
                {
                    var tb = new Poco.Table();
                    tb.Id = int.Parse(dr["object_id"].ToString());
                    tb.Name = dr["name"].ToString();
                    tb.DataBase = database;

                    lstTables.Add(tb);
                }

                return lstTables;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Poco.Column> GetColumns(int tableId)
        {
            var lstColumns = new List<Poco.Column>();
            try
            {
                var query = $"SELECT T0.column_id, T0.name, T0.system_type_id, " +
                            $"(SELECT CASE WHEN COUNT(1) > 0 THEN 1 ELSE 0 END FROM sys.index_columns TT where TT.object_id = T0.object_id and TT.column_id = T0.column_id)  AS pk " +
                            $"FROM sys.columns T0 " +
                            $"WHERE object_id = {tableId} " +
                            $" BY column_id";

                var dt = MssqlHelper.ExecuteQuery(query);
                foreach (DataRow dr in dt.Rows)
                {
                    var col = new Poco.Column();
                    col.Id = int.Parse(dr["column_id"].ToString());
                    col.Name = dr["name"].ToString();
                    col.Type = Types.FirstOrDefault(x => x.Id == int.Parse(dr["system_type_id"].ToString()));
                    col.IsPrimaryKey = bool.Parse(dr["pk"].ToString());

                    lstColumns.Add(col);
                }

                return lstColumns;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

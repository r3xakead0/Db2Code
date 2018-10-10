using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Db2Code.Library
{
    public class Mssql 
    {
        private string server = "";
        private bool isAuth = false;
        private string username = "";
        private string password = "";

        public string Database { get; set; } = "master";
        public List<Poco.Type> Types { get; set; } = new List<Poco.Type>();

        public Mssql(string server, string username, string password)
        {
            this.server = server;
            this.Database = "master";
            this.isAuth = false;
            this.username = username;
            this.password = password;

            MssqlHelper.DefaultConnectionString = $"Server={server};Database={Database};User Id={username};Password={password};";

            this.GetTypes();
        }

        public Mssql(string server)
        {
            this.server = server;
            this.Database = "master";
            this.isAuth = true;
            this.username = "";
            this.password = "";

            MssqlHelper.DefaultConnectionString = $"Server={server};Database={Database};Trusted_Connection=True;";

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
                this.Database = database.Name;

                if (isAuth)
                    MssqlHelper.DefaultConnectionString = $"Server={server};Database={Database};Trusted_Connection=True;";
                else
                    MssqlHelper.DefaultConnectionString = $"Server={server};Database={Database};User Id={username};Password={password};";

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
                var query = "SELECT T0.column_id, T0.name, T0.system_type_id, " +
                            "(SELECT CASE WHEN COUNT(1) > 0 THEN 1 ELSE 0 END FROM sys.index_columns TT where TT.object_id = T0.object_id and TT.column_id = T0.column_id) AS pk " +
                            "FROM sys.columns T0 " +
                            $"WHERE object_id = {tableId} " +
                            "ORDER BY column_id";

                var dt = MssqlHelper.ExecuteQuery(query);
                foreach (DataRow dr in dt.Rows)
                {
                    var col = new Poco.Column();
                    col.Id = int.Parse(dr["column_id"].ToString());
                    col.Name = dr["name"].ToString();
                    col.Type = Types.FirstOrDefault(x => x.Id == int.Parse(dr["system_type_id"].ToString()));
                    col.IsPrimaryKey = (dr["pk"].ToString() == "1");

                    lstColumns.Add(col);
                }

                return lstColumns;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void StoredProcedure(Poco.Table table, string PathRoot)
        {
            try
            {

                string nameTable = StringExtension.ToPascalCase(table.Name);

                string pathStoredProcedure = Path.Combine(PathRoot, "StoredProcedure");
                string fileStoredProcedure = $"{nameTable}.sql";

                if (Directory.Exists(pathStoredProcedure) == false)
                    Directory.CreateDirectory(pathStoredProcedure);

                string pathFileStoredProcedure = Path.Combine(pathStoredProcedure, fileStoredProcedure);
                if (File.Exists(pathFileStoredProcedure) == true)
                    File.Delete(pathFileStoredProcedure);

                using (StreamWriter objReader = new StreamWriter(pathFileStoredProcedure))
                {

                    #region Cabecera
                    objReader.WriteLine("set ANSI_NULLS ON");
                    objReader.WriteLine("set QUOTED_IDENTIFIER ON");
                    objReader.WriteLine("GO");
                    objReader.WriteLine("");
                    #endregion

                    List<Poco.Column> lstColumns = this.GetColumns(table.Id);
                    List<Poco.Column> lstColumnsWithoutPk = lstColumns.Where(x => x.IsPrimaryKey == false).ToList();
                    Poco.Column objPrimaryKey = lstColumns.FirstOrDefault(x => x.IsPrimaryKey == true);

                    #region Insert 

                    objReader.WriteLine($"CREATE PROCEDURE Sp{nameTable}Insertar");

                    string head = $"\tINSERT INTO {nameTable} (";
                    string foot = "\tVALUES (";
                    string ret = "";

                    for (int i = 0; i < lstColumns.Count; i++)
                    {
                        string nameColumn = lstColumns[i].Name;
                        string typeColumn = lstColumns[i].Type.Name.ToUpper();
                        string comma = (i == lstColumns.Count - 1) ? "" : ",";

                        if (lstColumns[i].IsPrimaryKey)
                        {
                            objReader.WriteLine($"@{nameColumn} AS {typeColumn} OUTPUT{comma}");

                            ret = $"\tSET @{nameColumn} = @@IDENTITY";
                        }
                        else
                        {
                            objReader.WriteLine($"@{nameColumn} AS {typeColumn}{comma}");

                            string separator = (i == lstColumns.Count - 1) ? ")" : ",";
                            head += $"{nameColumn}{separator}";
                            foot += $"@{nameColumn}{separator}";
                        }
                    }

                    objReader.WriteLine("AS");
                    objReader.WriteLine("BEGIN");

                    objReader.WriteLine(head);
                    objReader.WriteLine(foot);
                    objReader.WriteLine(ret);

                    objReader.WriteLine("END");
                    objReader.WriteLine("GO");

                    objReader.WriteLine("");

                    #endregion

                    #region Update

                    objReader.WriteLine($"CREATE PROCEDURE Sp{nameTable}Actualizar");

                    for (int i = 0; i < lstColumns.Count; i++)
                    {
                        string nameColumn = lstColumns[i].Name;
                        string typeColumn = lstColumns[i].Type.Name.ToUpper();
                        string comma = (i == lstColumns.Count - 1) ? "" : ",";

                        objReader.WriteLine($"@{nameColumn} AS {typeColumn}{comma}");
                    }

                    objReader.WriteLine("AS");
                    objReader.WriteLine("BEGIN");
                    objReader.WriteLine($"\tUPDATE {nameTable}");

                    for (int i = 0; i < lstColumnsWithoutPk.Count; i++)
                    {
                        string nameColumn = lstColumnsWithoutPk[i].Name.Trim();
                        string comma = (i == lstColumnsWithoutPk.Count - 1) ? "" : ",";
                        if (i == 0)
                            objReader.WriteLine($"\tSET {nameColumn} = @{nameColumn}{comma}");
                        else
                            objReader.WriteLine($"\t{nameColumn} = @{nameColumn}{comma}");
                    }

                    if (objPrimaryKey != null)
                    {
                        string nameColumn = objPrimaryKey.Name.Trim();
                        objReader.WriteLine($"\tWHERE {nameColumn} = @{nameColumn}");
                    }

                    objReader.WriteLine("END");
                    objReader.WriteLine("GO");
                    objReader.WriteLine("");

                    #endregion

                    #region Delete

                    objReader.WriteLine($"CREATE PROCEDURE Sp{nameTable}Eliminar");

                    if (objPrimaryKey != null)
                    {
                        string nameColumnPK = objPrimaryKey.Name.Trim();
                        string typeColumnPK = objPrimaryKey.Type.Name.ToUpper();
                        objReader.WriteLine($"@{nameColumnPK} AS {typeColumnPK}");
                    }

                    objReader.WriteLine("AS");
                    objReader.WriteLine("BEGIN");
                    objReader.WriteLine($"\tDELETE FROM {nameTable}");

                    if (objPrimaryKey != null)
                    {
                        string nameColumnPK = objPrimaryKey.Name.Trim();
                        objReader.WriteLine($"\tWHERE {nameColumnPK} = @{nameColumnPK}");
                    }

                    objReader.WriteLine("END");
                    objReader.WriteLine("GO");
                    objReader.WriteLine("");

                    #endregion

                    #region List
                    objReader.WriteLine($"CREATE PROCEDURE Sp{nameTable}Listar");
                    objReader.WriteLine("AS");
                    objReader.WriteLine("BEGIN");
                    for (int i = 0; i < lstColumns.Count; i++)
                    {
                        string nameColumn = lstColumns[i].Name.Trim();
                        string comma = (i == lstColumns.Count - 1) ? "" : ",";
                        if (i == 0)
                            objReader.WriteLine($"\tSELECT {nameColumn}{comma}");
                        else
                            objReader.WriteLine($"\t{nameColumn}{comma}");
                    }

                    objReader.WriteLine($"\tFROM {nameTable} WITH(NOLOCK)");
                    objReader.WriteLine("END");
                    objReader.WriteLine("GO");
                    objReader.WriteLine("");

                    #endregion

                    #region Get

                    objReader.WriteLine($"CREATE PROCEDURE Sp{nameTable}Obtener");

                    if (objPrimaryKey != null)
                    {
                        string nameColumnPK = objPrimaryKey.Name.Trim();
                        string typeColumnPK = objPrimaryKey.Type.Name.ToUpper();
                        objReader.WriteLine($"@{nameColumnPK} AS {typeColumnPK}");
                    }

                    objReader.WriteLine("AS");
                    objReader.WriteLine("BEGIN");

                    for (int i = 0; i < lstColumns.Count; i++)
                    {
                        string nameColumn = lstColumns[i].Name.Trim();
                        string comma = (i == lstColumns.Count - 1) ? "" : ",";
                        if (i == 0)
                            objReader.WriteLine($"\tSELECT TOP 1 {nameColumn}{comma}");
                        else
                            objReader.WriteLine($"\t{nameColumn}{comma}");
                    }

                    objReader.WriteLine($"\tFROM {nameTable} WITH(NOLOCK)");

                    if (objPrimaryKey != null)
                    {
                        string nameColumnPK = objPrimaryKey.Name.Trim();
                        objReader.WriteLine($"\tWHERE {nameColumnPK} = @{nameColumnPK}");
                    }

                    objReader.WriteLine("END");
                    objReader.WriteLine("GO");
                    objReader.WriteLine("");

                    #endregion

                    #region First

                    objReader.WriteLine($"CREATE PROCEDURE Sp{nameTable}Primero");
                    objReader.WriteLine("AS");
                    objReader.WriteLine("BEGIN");

                    objReader.WriteLine("\tSELECT TOP 1 ");
                    for (int i = 0; i < lstColumns.Count; i++)
                    {
                        string nameColumn = lstColumns[i].Name.Trim();
                        string comma = (i == lstColumns.Count - 1) ? "" : ",";
                        objReader.WriteLine($"\t{nameColumn}{comma}");
                    }

                    objReader.WriteLine($"\tFROM {nameTable} WITH(NOLOCK)");

                    if (objPrimaryKey != null)
                    {
                        string nameColumnPK = objPrimaryKey.Name.Trim();
                        objReader.WriteLine($"\tORDER BY  {nameColumnPK} ASC");
                    }

                    objReader.WriteLine("END");
                    objReader.WriteLine("GO");
                    objReader.WriteLine("");

                    #endregion 

                    #region Last

                    objReader.WriteLine($"CREATE PROCEDURE Sp{nameTable}Ultimo");
                    objReader.WriteLine("AS");
                    objReader.WriteLine("BEGIN");

                    objReader.WriteLine("\tSELECT TOP 1 ");
                    for (int i = 0; i < lstColumns.Count; i++)
                    {
                        string nameColumn = lstColumns[i].Name.Trim();
                        string comma = (i == lstColumns.Count - 1) ? "" : ",";
                        objReader.WriteLine($"\t{nameColumn}{comma}");
                    }

                    objReader.WriteLine($"\tFROM {nameTable} WITH(NOLOCK)");

                    if (objPrimaryKey != null)
                    {
                        string nameColumnPK = objPrimaryKey.Name.Trim();
                        objReader.WriteLine($"\tORDER BY  {nameColumnPK} DESC");
                    }

                    objReader.WriteLine("END");
                    objReader.WriteLine("GO");
                    objReader.WriteLine("");

                    #endregion

                    #region Previous 

                    objReader.WriteLine($"CREATE PROCEDURE Sp{nameTable}Anterior");

                    if (objPrimaryKey != null)
                    {
                        string nameColumnPK = objPrimaryKey.Name.Trim();
                        string typeColumnPK = objPrimaryKey.Type.Name.ToUpper();
                        objReader.WriteLine($"@{nameColumnPK} AS {typeColumnPK}");
                    }

                    objReader.WriteLine("AS");
                    objReader.WriteLine("BEGIN");

                    objReader.WriteLine("\tSELECT TOP 1 ");
                    for (int i = 0; i < lstColumns.Count; i++)
                    {
                        string nameColumn = lstColumns[i].Name.Trim();
                        string comma = (i == lstColumns.Count - 1) ? "" : ",";
                        objReader.WriteLine($"\t{nameColumn}{comma}");
                    }


                    objReader.WriteLine($"\tFROM {nameTable} WITH(NOLOCK)");

                    if (objPrimaryKey != null)
                    {
                        string nameColumnPK = objPrimaryKey.Name.Trim();
                        objReader.WriteLine($"\tWHERE {nameColumnPK} < @{nameColumnPK}");
                        objReader.WriteLine($"\tORDER BY  {nameColumnPK} DESC");
                    }

                    objReader.WriteLine("END");
                    objReader.WriteLine("GO");
                    objReader.WriteLine("");

                    #endregion

                    #region Next

                    objReader.WriteLine($"CREATE PROCEDURE Sp{nameTable}Siguiente");

                    if (objPrimaryKey != null)
                    {
                        string nameColumnPK = objPrimaryKey.Name.Trim();
                        string typeColumnPK = objPrimaryKey.Type.Name.ToUpper();
                        objReader.WriteLine($"@{nameColumnPK} AS {typeColumnPK}");
                    }

                    objReader.WriteLine("AS");
                    objReader.WriteLine("BEGIN");

                    objReader.WriteLine("\tSELECT TOP 1 ");
                    for (int i = 0; i < lstColumns.Count; i++)
                    {
                        string nameColumn = lstColumns[i].Name.Trim();
                        string comma = (i == lstColumns.Count - 1) ? "" : ",";
                        objReader.WriteLine($"\t{nameColumn}{comma}");
                    }


                    objReader.WriteLine($"\tFROM {nameTable} WITH(NOLOCK)");

                    if (objPrimaryKey != null)
                    {
                        string nameColumnPK = objPrimaryKey.Name.Trim();
                        objReader.WriteLine($"\tWHERE {nameColumnPK} > @{nameColumnPK}");
                        objReader.WriteLine($"\tORDER BY  {nameColumnPK} ASC");
                    }

                    objReader.WriteLine("END");
                    objReader.WriteLine("GO");
                    objReader.WriteLine("");

                    #endregion

                    objReader.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

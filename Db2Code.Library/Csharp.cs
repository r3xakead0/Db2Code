using Db2Code.Library.Poco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Db2Code.Library
{
    public class Csharp : IGenerate
    {
        private string PathRoot = "";
        private Mssql Database = null;

        public Csharp(string path, Mssql database)
        {
            this.PathRoot = path;
            this.Database = database;
        }

        public void BusinessEntity(Table table)
        {
            try
            {
                string nameDB = StringExtension.ToPascalCase(Database.Database);
                string nameTable = StringExtension.ToPascalCase(table.Name);

                string pathBusinessEntity = Path.Combine(this.PathRoot, "BusinessEntity");
                string fileBusinessEntity = $"{nameTable}.cs";

                if (Directory.Exists(pathBusinessEntity) == false)
                    Directory.CreateDirectory(pathBusinessEntity);

                string pathFilepathBusinessEntity = Path.Combine(pathBusinessEntity, fileBusinessEntity);
                if (File.Exists(pathFilepathBusinessEntity) == true)
                    File.Delete(pathFilepathBusinessEntity);

                using (StreamWriter objReader = new StreamWriter(pathFilepathBusinessEntity))
                {

                    objReader.WriteLine("using System;");
                    objReader.WriteLine("");

                    objReader.WriteLine($"namespace {nameDB}.BusinessEntity");
                    objReader.WriteLine("{");

                    objReader.WriteLine($"\tpublic class {nameTable}");
                    objReader.WriteLine("\t{");

                    var lstColumns = Database.GetColumns(table.Id);
                    foreach (var objColumn in lstColumns)
                    {
                        string nameColumn = objColumn.Name;
                        string typeColumn = this.TranslateType(objColumn.Type.Name);
                        objReader.WriteLine($"\t\tpublic {typeColumn} {nameColumn}" + "{ get; set; }");
                    }   

                    objReader.WriteLine("\t}");

                    objReader.WriteLine("}");

                    objReader.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void BusinessLogic(Table table)
        {
            try
            {
                string nameDB = StringExtension.ToPascalCase(Database.Database);
                string nameTable = StringExtension.ToPascalCase(table.Name);

                string pathBusinessLogic = Path.Combine(this.PathRoot, "BusinessLogic");
                string fileBusinessLogic = $"{nameTable}.cs";

                if (Directory.Exists(pathBusinessLogic) == false)
                    Directory.CreateDirectory(pathBusinessLogic);

                string pathFilePathBusinessLogic = Path.Combine(pathBusinessLogic, fileBusinessLogic);
                if (File.Exists(pathFilePathBusinessLogic) == true)
                    File.Delete(pathFilePathBusinessLogic);
              
                using (StreamWriter objReader = new StreamWriter(pathFilePathBusinessLogic))
                {

                    List<Column> lstColumns = Database.GetColumns(table.Id);
                    Column objColumnPK = lstColumns.FirstOrDefault(x => x.IsPrimaryKey == true);

                    objReader.WriteLine($"using BE = {nameDB}.BusinessEntity;");
                    objReader.WriteLine("using System.Collections.Generic;");
                    objReader.WriteLine("using System.Data.SqlClient;");
                    objReader.WriteLine("using System.Data;");
                    objReader.WriteLine("using System;");
                    objReader.WriteLine("");

                    objReader.WriteLine($"namespace {nameDB}.BusinessLogic");
                    objReader.WriteLine("{");

                    objReader.WriteLine($"\tpublic class {nameTable}");
                    objReader.WriteLine("\t{");

                    objReader.WriteLine("\t\tprivate string connectionString = \"\";");
                    objReader.WriteLine("");

                    #region Constructor

                    objReader.WriteLine($"\t\tpublic {nameTable}(string connectionString)");
                    objReader.WriteLine("\t\t{");
                    objReader.WriteLine("\t\t\ttry {");
                    objReader.WriteLine("\t\t\t\tthis.connectionString = connectionString;");
                    objReader.WriteLine("\t\t\t}");
                    objReader.WriteLine("\t\t\tcatch (Exception ex) {");
                    objReader.WriteLine("\t\t\t\tthrow ex;");
                    objReader.WriteLine("\t\t\t}");
                    objReader.WriteLine("\t\t}");
                    objReader.WriteLine("");

                    #endregion

                    #region Insert

                    objReader.WriteLine($"\t\tpublic int Insertar(ref BE.{nameTable} be{nameTable})");
                    objReader.WriteLine("\t\t{");
                    objReader.WriteLine("\t\t\tint rowsAffected = 0;");
                    objReader.WriteLine("\t\t\t\ttry {");
                    objReader.WriteLine($"\t\t\t\tstring sp = \"Sp{nameTable}Insertar\";");
                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\t\tvar cnn = new SqlConnection(this.connectionString);");
                    objReader.WriteLine("\t\t\t\tvar cmd = new SqlCommand(sp, cnn);");
                    objReader.WriteLine("\t\t\t\tcmd.CommandType = CommandType.StoredProcedure;");
                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\t\tcnn.Open();");
                    objReader.WriteLine("");

                    foreach (var objColumn in lstColumns)
                    {
                        string nameColumn = StringExtension.ToPascalCase(objColumn.Name);

                        string parameter = $"new SqlParameter(\"@{nameColumn}\", be{nameTable}.{nameColumn})";
                        objReader.WriteLine($"\t\t\t\tcmd.Parameters.Add({parameter});");

                        if (objColumn.IsPrimaryKey == true)
                            objReader.WriteLine($"\t\t\t\tcmd.Parameters[\"@{nameColumn}\"].Direction = ParameterDirection.Output;"); 
                    }

                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\t\trowsAffected = cmd.ExecuteNonQuery();");
                    objReader.WriteLine("");

                    if (objColumnPK != null)
                    {
                        string nameColumn = StringExtension.ToPascalCase(objColumnPK.Name);
                        string typeColumn = this.TranslateType(objColumnPK.Type.Name);

                        if (typeColumn.ToUpper().Equals("STRING"))
                            objReader.WriteLine($"\t\t\t\tbe{nameTable}.{nameColumn} = cmd.Parameters[\"@{nameColumn}\"].Value.ToString();");
                        else
                            objReader.WriteLine($"\t\t\t\tbe{nameTable}.{nameColumn} = {typeColumn}.Parse(cmd.Parameters[\"@{nameColumn}\"].Value.ToString());");
                    }

                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\t\treturn rowsAffected;");
                    objReader.WriteLine("\t\t\t}");
                    objReader.WriteLine("\t\t\tcatch (Exception ex) {");
                    objReader.WriteLine("\t\t\t\tthrow ex;");
                    objReader.WriteLine("\t\t\t}");
                    objReader.WriteLine("\t\t}");
                    objReader.WriteLine("");

                    #endregion

                    #region Update

                    objReader.WriteLine($"\tpublic int Actualizar(BE.{nameTable} be{nameTable})");
                    objReader.WriteLine("\t{");
                    objReader.WriteLine("\tint rowsAffected = 0;");
                    objReader.WriteLine("\t\ttry {");
                    objReader.WriteLine($"\t\t\tstring sp = \"Sp{nameTable}Actualizar\";");
                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\tvar cnn = new SqlConnection(this.connectionString);");
                    objReader.WriteLine("\t\t\tvar cmd = new SqlCommand(sp, cnn);");
                    objReader.WriteLine("\t\t\tcmd.CommandType = CommandType.StoredProcedure;");
                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\tcnn.Open();");
                    objReader.WriteLine("");

                    foreach (var objColumn in lstColumns)
                    {
                        string nameColumn = StringExtension.ToPascalCase(objColumn.Name);

                        string parameter = $"new SqlParameter(\"@{nameColumn}\", be{nameTable}.{nameColumn})";
                        objReader.WriteLine($"\t\t\tcmd.Parameters.Add({parameter});");
                    }

                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\trowsAffected = cmd.ExecuteNonQuery();");
                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\treturn rowsAffected;");
                    objReader.WriteLine("\t\t}");
                    objReader.WriteLine("\t\tcatch (Exception ex) {");
                    objReader.WriteLine("\t\t\tthrow ex;");
                    objReader.WriteLine("\t\t}");
                    objReader.WriteLine("\t}");
                    objReader.WriteLine("");

                    #endregion

                    #region Delete

                    objReader.WriteLine($"\tpublic int Eliminar(BE.{nameTable} be{nameTable})");
                    objReader.WriteLine("\t{");
                    objReader.WriteLine("\tint rowsAffected = 0;");
                    objReader.WriteLine("\t\ttry {");
                    objReader.WriteLine($"\t\t\tstring sp = \"Sp{nameTable}Eliminar\";");
                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\tvar cnn = new SqlConnection(this.connectionString);");
                    objReader.WriteLine("\t\t\tvar cmd = new SqlCommand(sp, cnn);");
                    objReader.WriteLine("\t\t\tcmd.CommandType = CommandType.StoredProcedure;");
                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\tcnn.Open();");
                    objReader.WriteLine("");

                    if (objColumnPK != null)
                    {
                        string nameColumn = StringExtension.ToPascalCase(objColumnPK.Name);

                        string parameter = $"new SqlParameter(\"@{nameColumn}\", be{nameTable}.{nameColumn})";
                        objReader.WriteLine($"\t\t\tcmd.Parameters.Add({parameter});");
                    }

                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\trowsAffected = cmd.ExecuteNonQuery();");
                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\treturn rowsAffected;");
                    objReader.WriteLine("\t\t}");
                    objReader.WriteLine("\t\tcatch (Exception ex) {");
                    objReader.WriteLine("\t\t\tthrow ex;");
                    objReader.WriteLine("\t\t}");
                    objReader.WriteLine("\t}");
                    objReader.WriteLine("");

                    #endregion

                    #region List

                    objReader.WriteLine($"\tpublic List<BE.{nameTable}> Listar()");
                    objReader.WriteLine("\t{");
                    objReader.WriteLine($"\tvar lst{nameTable} = new List<BE.{nameTable}>();");
                    objReader.WriteLine("\t\ttry {");
                    objReader.WriteLine($"\t\t\tstring sp = \"Sp{nameTable}Listar\";");
                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\tvar cnn = new SqlConnection(this.connectionString);");
                    objReader.WriteLine("\t\t\tvar cmd = new SqlCommand(sp, cnn);");
                    objReader.WriteLine("\t\t\tcmd.CommandType = CommandType.StoredProcedure;");
                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\tcnn.Open();");
                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\tSqlDataReader reader = cmd.ExecuteReader();");
                    objReader.WriteLine("\t\t\twhile (reader.Read())");
                    objReader.WriteLine("\t\t\t{");
                    objReader.WriteLine($"\t\t\t\tvar be{nameTable} = new BE.{nameTable}();");
                    foreach (var objColumn in lstColumns)
                    {
                        string nameColumn = StringExtension.ToPascalCase(objColumn.Name);
                        string typeColumn = this.TranslateType(objColumn.Type.Name);

                        if (typeColumn.ToUpper().Equals("STRING"))
                            objReader.WriteLine($"\t\t\t\tbe{nameTable}.{nameColumn} = reader[\"{nameColumn}\"].ToString();");
                        else
                            objReader.WriteLine($"\t\t\t\tbe{nameTable}.{nameColumn} = {typeColumn}.Parse(reader[\"{nameColumn}\"].ToString());");
                    }
                    objReader.WriteLine($"\t\t\t\tlst{nameTable}.Add(be{nameTable});");
                    objReader.WriteLine("\t\t\t}");
                    objReader.WriteLine($"\t\t\treturn lst{nameTable};");
                    objReader.WriteLine("\t\t}");
                    objReader.WriteLine("\t\tcatch (Exception ex) {");
                    objReader.WriteLine("\t\t\tthrow ex;");
                    objReader.WriteLine("\t\t}");
                    objReader.WriteLine("\t}");
                    objReader.WriteLine("");

                    #endregion

                    #region Get

                    if (objColumnPK != null)
                    {
                        string nameColumnPK = StringExtension.ToPascalCase(objColumnPK.Name);
                        string typeColumnPK = this.TranslateType(objColumnPK.Type.Name);

                        objReader.WriteLine($"\tpublic BE.{nameTable} Obtener({typeColumnPK} {nameColumnPK})");
                        objReader.WriteLine("\t{");
                        objReader.WriteLine($"\tBE.{nameTable} be{nameTable} = null;");
                        objReader.WriteLine("\t\ttry {");
                        objReader.WriteLine($"\t\t\tstring sp = \"Sp{nameTable}Obtener\";");
                        objReader.WriteLine("");
                        objReader.WriteLine("\t\t\tvar cnn = new SqlConnection(this.connectionString);");
                        objReader.WriteLine("\t\t\tvar cmd = new SqlCommand(sp, cnn);");
                        objReader.WriteLine("\t\t\tcmd.CommandType = CommandType.StoredProcedure;");
                        objReader.WriteLine("");
                        objReader.WriteLine("\t\t\tcnn.Open();");
                        objReader.WriteLine("");
                        objReader.WriteLine($"\t\t\tcmd.Parameters.Add(new SqlParameter(\"@{nameColumnPK}\", {nameColumnPK}));");
                        objReader.WriteLine("");
                        objReader.WriteLine("\t\t\tSqlDataReader reader = cmd.ExecuteReader();");
                        objReader.WriteLine("\t\t\tif (reader.Read())");
                        objReader.WriteLine("\t\t\t{");
                        objReader.WriteLine($"\t\t\t\tbe{nameTable} = new BE.{nameTable}();");
                        foreach (var objColumn in lstColumns)
                        {
                            string nameColumn = StringExtension.ToPascalCase(objColumn.Name);
                            string typeColumn = this.TranslateType(objColumn.Type.Name);

                            if (typeColumn.ToUpper().Equals("STRING"))
                                objReader.WriteLine($"\t\t\t\tbe{nameTable}.{nameColumn} = reader[\"{nameColumn}\"].ToString();");
                            else
                                objReader.WriteLine($"\t\t\t\tbe{nameTable}.{nameColumn} = {typeColumn}.Parse(reader[\"{nameColumn}\"].ToString());");
                        }
                        objReader.WriteLine("\t\t\t}");
                        objReader.WriteLine($"\t\t\treturn be{nameTable};");
                        objReader.WriteLine("\t\t}");
                        objReader.WriteLine("\t\tcatch (Exception ex) {");
                        objReader.WriteLine("\t\t\tthrow ex;");
                        objReader.WriteLine("\t\t}");
                        objReader.WriteLine("\t}");
                        objReader.WriteLine("");
                    }

                    #endregion

                    #region First

                    objReader.WriteLine($"\tpublic BE.{nameTable} Primero()");
                    objReader.WriteLine("\t{");
                    objReader.WriteLine($"\tBE.{nameTable} be{nameTable} = null;");
                    objReader.WriteLine("\t\ttry {");
                    objReader.WriteLine($"\t\t\tstring sp = \"Sp{nameTable}Primero\";");
                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\tvar cnn = new SqlConnection(this.connectionString);");
                    objReader.WriteLine("\t\t\tvar cmd = new SqlCommand(sp, cnn);");
                    objReader.WriteLine("\t\t\tcmd.CommandType = CommandType.StoredProcedure;");
                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\tcnn.Open();");
                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\tSqlDataReader reader = cmd.ExecuteReader();");
                    objReader.WriteLine("\t\t\tif (reader.Read())");
                    objReader.WriteLine("\t\t\t{");
                    objReader.WriteLine($"\t\t\t\tbe{nameTable} = new BE.{nameTable}();");
                    foreach (var objColumn in lstColumns)
                    {
                        string nameColumn = StringExtension.ToPascalCase(objColumn.Name);
                        string typeColumn = this.TranslateType(objColumn.Type.Name);

                        if (typeColumn.ToUpper().Equals("STRING"))
                            objReader.WriteLine($"\t\t\t\tbe{nameTable}.{nameColumn} = reader[\"{nameColumn}\"].ToString();");
                        else
                            objReader.WriteLine($"\t\t\t\tbe{nameTable}.{nameColumn} = {typeColumn}.Parse(reader[\"{nameColumn}\"].ToString());");
                    }
                    objReader.WriteLine("\t\t\t}");
                    objReader.WriteLine($"\t\t\treturn be{nameTable};");
                    objReader.WriteLine("\t\t}");
                    objReader.WriteLine("\t\tcatch (Exception ex) {");
                    objReader.WriteLine("\t\t\tthrow ex;");
                    objReader.WriteLine("\t\t}");
                    objReader.WriteLine("\t}");
                    objReader.WriteLine("");

                    #endregion

                    #region Last

                    objReader.WriteLine($"\tpublic BE.{nameTable} Ultimo()");
                    objReader.WriteLine("\t{");
                    objReader.WriteLine($"\tBE.{nameTable} be{nameTable} = null;");
                    objReader.WriteLine("\t\ttry {");
                    objReader.WriteLine($"\t\t\tstring sp = \"Sp{nameTable}Ultimo\";");
                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\tvar cnn = new SqlConnection(this.connectionString);");
                    objReader.WriteLine("\t\t\tvar cmd = new SqlCommand(sp, cnn);");
                    objReader.WriteLine("\t\t\tcmd.CommandType = CommandType.StoredProcedure;");
                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\tcnn.Open();");
                    objReader.WriteLine("");
                    objReader.WriteLine("\t\t\tSqlDataReader reader = cmd.ExecuteReader();");
                    objReader.WriteLine("\t\t\tif (reader.Read())");
                    objReader.WriteLine("\t\t\t{");
                    objReader.WriteLine($"\t\t\t\tbe{nameTable} = new BE.{nameTable}();");
                    foreach (var objColumn in lstColumns)
                    {
                        string nameColumn = StringExtension.ToPascalCase(objColumn.Name);
                        string typeColumn = this.TranslateType(objColumn.Type.Name);

                        if (typeColumn.ToUpper().Equals("STRING"))
                            objReader.WriteLine($"\t\t\t\tbe{nameTable}.{nameColumn} = reader[\"{nameColumn}\"].ToString();");
                        else
                            objReader.WriteLine($"\t\t\t\tbe{nameTable}.{nameColumn} = {typeColumn}.Parse(reader[\"{nameColumn}\"].ToString());");
                    }
                    objReader.WriteLine("\t\t\t}");
                    objReader.WriteLine($"\t\t\treturn be{nameTable};");
                    objReader.WriteLine("\t\t}");
                    objReader.WriteLine("\t\tcatch (Exception ex) {");
                    objReader.WriteLine("\t\t\tthrow ex;");
                    objReader.WriteLine("\t\t}");
                    objReader.WriteLine("\t}");
                    objReader.WriteLine("");

                    #endregion

                    #region Previous

                    if (objColumnPK != null)
                    {
                        string nameColumnPK = StringExtension.ToPascalCase(objColumnPK.Name);
                        string typeColumnPK = this.TranslateType(objColumnPK.Type.Name);

                        objReader.WriteLine($"\tpublic BE.{nameTable} Anterior({typeColumnPK} {nameColumnPK})");
                        objReader.WriteLine("\t{");
                        objReader.WriteLine($"\tBE.{nameTable} be{nameTable} = null;");
                        objReader.WriteLine("\t\ttry {");
                        objReader.WriteLine($"\t\t\tstring sp = \"Sp{nameTable}Anterior\";");
                        objReader.WriteLine("");
                        objReader.WriteLine("\t\t\tvar cnn = new SqlConnection(this.connectionString);");
                        objReader.WriteLine("\t\t\tvar cmd = new SqlCommand(sp, cnn);");
                        objReader.WriteLine("\t\t\tcmd.CommandType = CommandType.StoredProcedure;");
                        objReader.WriteLine("");
                        objReader.WriteLine("\t\t\tcnn.Open();");
                        objReader.WriteLine("");
                        objReader.WriteLine($"\t\t\tcmd.Parameters.Add(new SqlParameter(\"@{nameColumnPK}\", {nameColumnPK}));");
                        objReader.WriteLine("");
                        objReader.WriteLine("\t\t\tSqlDataReader reader = cmd.ExecuteReader();");
                        objReader.WriteLine("\t\t\tif (reader.Read())");
                        objReader.WriteLine("\t\t\t{");
                        objReader.WriteLine($"\t\t\t\tbe{nameTable} = new BE.{nameTable}();");
                        foreach (var objColumn in lstColumns)
                        {
                            string nameColumn = StringExtension.ToPascalCase(objColumn.Name);
                            string typeColumn = this.TranslateType(objColumn.Type.Name);

                            if (typeColumn.ToUpper().Equals("STRING"))
                                objReader.WriteLine($"\t\t\t\tbe{nameTable}.{nameColumn} = reader[\"{nameColumn}\"].ToString();");
                            else
                                objReader.WriteLine($"\t\t\t\tbe{nameTable}.{nameColumn} = {typeColumn}.Parse(reader[\"{nameColumn}\"].ToString());");
                        }
                        objReader.WriteLine("\t\t\t}");
                        objReader.WriteLine($"\t\t\treturn be{nameTable};");
                        objReader.WriteLine("\t\t}");
                        objReader.WriteLine("\t\tcatch (Exception ex) {");
                        objReader.WriteLine("\t\t\tthrow ex;");
                        objReader.WriteLine("\t\t}");
                        objReader.WriteLine("\t}");
                        objReader.WriteLine("");
                    }

                    #endregion

                    #region Next

                    if (objColumnPK != null)
                    {
                        string nameColumnPK = StringExtension.ToPascalCase(objColumnPK.Name);
                        string typeColumnPK = this.TranslateType(objColumnPK.Type.Name);

                        objReader.WriteLine($"\tpublic BE.{nameTable} Siguiente({typeColumnPK} {nameColumnPK})");
                        objReader.WriteLine("\t{");
                        objReader.WriteLine($"\tBE.{nameTable} be{nameTable} = null;");
                        objReader.WriteLine("\t\ttry {");
                        objReader.WriteLine($"\t\t\tstring sp = \"Sp{nameTable}Siguiente\";");
                        objReader.WriteLine("");
                        objReader.WriteLine("\t\t\tvar cnn = new SqlConnection(this.connectionString);");
                        objReader.WriteLine("\t\t\tvar cmd = new SqlCommand(sp, cnn);");
                        objReader.WriteLine("\t\t\tcmd.CommandType = CommandType.StoredProcedure;");
                        objReader.WriteLine("");
                        objReader.WriteLine("\t\t\tcnn.Open();");
                        objReader.WriteLine("");
                        objReader.WriteLine($"\t\t\tcmd.Parameters.Add(new SqlParameter(\"@{nameColumnPK}\", {nameColumnPK}));");
                        objReader.WriteLine("");
                        objReader.WriteLine("\t\t\tSqlDataReader reader = cmd.ExecuteReader();");
                        objReader.WriteLine("\t\t\tif (reader.Read())");
                        objReader.WriteLine("\t\t\t{");
                        objReader.WriteLine($"\t\t\t\tbe{nameTable} = new BE.{nameTable}();");
                        foreach (var objColumn in lstColumns)
                        {
                            string nameColumn = StringExtension.ToPascalCase(objColumn.Name);
                            string typeColumn = this.TranslateType(objColumn.Type.Name);

                            if (typeColumn.ToUpper().Equals("STRING"))
                                objReader.WriteLine($"\t\t\t\tbe{nameTable}.{nameColumn} = reader[\"{nameColumn}\"].ToString();");
                            else
                                objReader.WriteLine($"\t\t\t\tbe{nameTable}.{nameColumn} = {typeColumn}.Parse(reader[\"{nameColumn}\"].ToString());");
                        }
                        objReader.WriteLine("\t\t\t}");
                        objReader.WriteLine($"\t\t\treturn be{nameTable};");
                        objReader.WriteLine("\t\t}");
                        objReader.WriteLine("\t\tcatch (Exception ex) {");
                        objReader.WriteLine("\t\t\tthrow ex;");
                        objReader.WriteLine("\t\t}");
                        objReader.WriteLine("\t}");
                        objReader.WriteLine("");
                    }

                    #endregion

                    objReader.WriteLine("\t}");

                    objReader.WriteLine("}");

                    objReader.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WindowsForm(Table table)
        {
            try
            {
                string nameDB = StringExtension.ToPascalCase(Database.Database);
                string nameTable = StringExtension.ToPascalCase(table.Name);

                string pathBusinessEntity = Path.Combine(this.PathRoot, "WindowsForm");
                string fileBusinessEntity = $"{nameTable}.cs";

                if (Directory.Exists(pathBusinessEntity) == false)
                    Directory.CreateDirectory(pathBusinessEntity);

                string pathFilepathBusinessEntity = Path.Combine(pathBusinessEntity, fileBusinessEntity);
                if (File.Exists(pathFilepathBusinessEntity) == true)
                    File.Delete(pathFilepathBusinessEntity);

                using (StreamWriter objReader = new StreamWriter(pathFilepathBusinessEntity))
                {

                    objReader.WriteLine("using System;");
                    objReader.WriteLine("");

                    objReader.WriteLine($"namespace {nameDB}.WindowsForm");
                    objReader.WriteLine("{");

                    objReader.WriteLine($"\tpublic class {nameTable}");
                    objReader.WriteLine("\t{");

                    objReader.WriteLine("\t}");

                    objReader.WriteLine("}");

                    objReader.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string TranslateType(string typeDB)
        {
            string typeCsharp = "";
            try
            {
                switch (typeDB)
                {
                    case "text":
                    case "varchar":
                    case "char":
                    case "ntext":
                    case "nvarchar":
                    case "nchar":
                        typeCsharp = "string";
                        break;
                    case "tinyint":
                    case "smallint":
                    case "int":
                    case "bigint":
                        typeCsharp = "int";
                        break;
                    case "decimal":
                    case "numeric":
                    case "money":
                    case "smallmoney":
                    case "float":
                    case "real":
                        typeCsharp = "double";
                        break;
                    case "bit":
                        typeCsharp = "bool";
                        break;
                    case "smalldatetime":
                    case "datetime":
                        typeCsharp = "DateTime";
                        break;
                    case "binary":
                    case "varbinary":
                    case "image":
                        typeCsharp = "string";
                        break;
                    default:
                        typeCsharp = "string";
                        break;
                }


                return typeCsharp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


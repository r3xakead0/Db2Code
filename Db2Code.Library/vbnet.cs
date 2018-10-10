using System;
using Db2Code.Library.Poco;

namespace Db2Code.Library
{
    public class Vbnet : IGenerate
    {
        private string PathRoot = "";
        private Mssql Database = null;

        public Vbnet(string path, Mssql database)
        {
            this.PathRoot = path;
            this.Database = database;
        }

        public void BusinessEntity(Table table)
        {
            try
            {
                
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

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

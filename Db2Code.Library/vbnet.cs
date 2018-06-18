using System;
using System.Collections.Generic;
using Db2Code.Library.Poco;

namespace Db2Code.Library
{
    public class Vbnet : IGenerateClass
    {

        public Vbnet()
        {

        }

        public bool BusinessEntity(Table table)
        {
            bool flg = false;
            try
            {
                string nameTable = StringExtension.ToPascalCase(table.Name);

                return flg;
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }

        public bool BusinessLogic(Table table)
        {
            bool flg = false;
            try
            {

                return flg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

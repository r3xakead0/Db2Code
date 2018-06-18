using Db2Code.Library.Poco;

namespace Db2Code.Library
{
    public interface IGenerateClass
    {
        bool BusinessEntity(Table table);
        bool BusinessLogic(Table table);
    }
}

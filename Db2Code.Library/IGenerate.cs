using Db2Code.Library.Poco;

namespace Db2Code.Library
{
    public interface IGenerate
    {
        void BusinessEntity(Table table);
        void BusinessLogic(Table table);
        void WindowsForm(Table table);
    }
}

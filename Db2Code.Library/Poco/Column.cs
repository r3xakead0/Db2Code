namespace Db2Code.Library.Poco
{
    public class Column
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public bool IsPrimaryKey { get; set; } = false;
    }
}

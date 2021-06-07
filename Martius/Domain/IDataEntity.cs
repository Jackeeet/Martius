namespace Martius.Domain
{
    public interface IDataEntity
    {
        int Id { get; }
        string ToSqlString();
    }
}
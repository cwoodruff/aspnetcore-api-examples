namespace Chinook.Domain.DbInfo;

public class DbInfo(string connectionStrings) : IDbInfo
{
    public string ConnectionStrings { get; } = connectionStrings;
}
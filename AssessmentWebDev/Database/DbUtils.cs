using System.Data;
using MySql.Data.MySqlClient;

namespace AssessmentWebDev.Database;

public class DbUtils
{
    private static readonly string ConnectionString =
        Startup.StaticConfiguration.GetConnectionString("stenden-cafe.MySQL");
    public IDbConnection GetDbConnection()
    {
        return new MySqlConnection(ConnectionString);
    }
    
    
}
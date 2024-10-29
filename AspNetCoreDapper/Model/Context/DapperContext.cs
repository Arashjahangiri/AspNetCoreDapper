using System.Data;
using System.Data.SqlClient;

namespace AspNetCoreDapper.Model.Context
{
    public class DapperContext
    {
        private readonly IConfiguration configuration;
        private readonly string _connectionString;
        public DapperContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}

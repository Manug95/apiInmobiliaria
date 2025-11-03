using api_inmobiliaria.Repositories.EntityFramework;

namespace api_inmobiliaria.Repositories
{
    public class RepositoryBaseMySql
    {
        protected readonly IConfiguration _configuration;
        protected readonly string connectionString;

        public RepositoryBaseMySql(IConfiguration config)
        {
            _configuration = config;
            connectionString = config["ConnectionStrings:MySql"]!;
        }
    }
}
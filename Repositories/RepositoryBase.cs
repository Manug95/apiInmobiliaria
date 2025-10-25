using api_inmobiliaria.Repositories.EntityFramework;

namespace api_inmobiliaria.Repositories
{
    public class RepositoryBase
    {
        protected readonly IConfiguration? _configuration;
        protected readonly BDContext? _dbContext;
        protected readonly string? connectionString;

        public RepositoryBase(IConfiguration config)
        {
            _configuration = config;
            connectionString = config["ConnectionStrings:MySql"]!;
        }

        public RepositoryBase(BDContext context)
        {
            _dbContext = context;
        }
    }
}
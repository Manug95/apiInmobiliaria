using api_inmobiliaria.Repositories.EntityFramework;

namespace api_inmobiliaria.Repositories
{
    public class RepositoryBaseEF
    {
        protected readonly BDContext _dbContext;

        public RepositoryBaseEF(BDContext context)
        {
            _dbContext = context;
        }
    }
}
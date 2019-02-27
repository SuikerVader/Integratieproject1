namespace Integratieproject1.DAL.Repositorys
{
    public class UsersRepository
    {
        private CityOfIdeasDbContext ctx = null;
        public UsersRepository(CityOfIdeasDbContext dbContext)
        {
            ctx = dbContext;
            //CityOfIdeasDbInitializer.Initialize(ctx, false);
        }
        
    }
}
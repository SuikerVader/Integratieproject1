namespace Integratieproject1.DAL.Repositorys
{
    public class UsersRepository
    {
        private CityOfIdeasDbContext ctx = null;
        public UsersRepository()
        {
            ctx = new CityOfIdeasDbContext();
            CityOfIdeasDbInitializer.Initialize(ctx, false);
        }
    }
}
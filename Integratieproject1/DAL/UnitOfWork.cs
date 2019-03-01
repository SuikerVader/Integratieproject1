namespace Integratieproject1.DAL
{
    public class UnitOfWork
    {
        public CityOfIdeasDbContext ctx;
        public UnitOfWork()
        {
            ctx = new CityOfIdeasDbContext();
            CityOfIdeasDbInitializer.Initialize(ctx,true);
        }

    }
}
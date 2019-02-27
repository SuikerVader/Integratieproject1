namespace Integratieproject1.DAL.Repositorys
{
    public class IoTRepository
    {
        private CityOfIdeasDbContext ctx = null;
        public IoTRepository()
        {
            ctx = new CityOfIdeasDbContext();
            CityOfIdeasDbInitializer.Initialize(ctx, false);
        }
    }
}
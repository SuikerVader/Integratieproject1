namespace Integratieproject1.DAL.Repositorys
{
    public class SurveysRepository
    {
        private CityOfIdeasDbContext ctx = null;
        public SurveysRepository()
        {
            ctx = new CityOfIdeasDbContext();
            CityOfIdeasDbInitializer.Initialize(ctx, false);
        }
    }
}
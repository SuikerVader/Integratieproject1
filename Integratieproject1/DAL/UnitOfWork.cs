using System;

namespace Integratieproject1.DAL
{
    public class UnitOfWork
    {
        public CityOfIdeasDbContext ctx;
        public UnitOfWork()
        {
            ctx = new CityOfIdeasDbContext();
            Console.WriteLine("uow fixed!");
            CityOfIdeasDbInitializer.Initialize(ctx,true);
        }

    }
}
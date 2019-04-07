using System;

namespace Integratieproject1.DAL
{
    public class UnitOfWork
    {
        public UnitOfWork()
        {
            Ctx = new CityOfIdeasDbContext(true);
            /*Console.WriteLine("uow fixed!");
            CityOfIdeasDbInitializer.Initialize(ctx,true);*/
        }

        public UnitOfWork(CityOfIdeasDbContext ctx)
        {
            this.Ctx = ctx;
        }
        
        internal CityOfIdeasDbContext Ctx { get; }

        public void CommitChanges()
        {
            Ctx.CommitChanges();
        }

    }
}
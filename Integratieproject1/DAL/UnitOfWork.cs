using System;

namespace Integratieproject1.DAL
{
    public class UnitOfWork
    {
        public UnitOfWork()
        {
            ctx = new CityOfIdeasDbContext(true);
            /*Console.WriteLine("uow fixed!");
            CityOfIdeasDbInitializer.Initialize(ctx,true);*/
        }

        public UnitOfWork(CityOfIdeasDbContext ctx)
        {
            this.ctx = ctx;
        }
        internal CityOfIdeasDbContext ctx { get; }

        public void CommitChanges()
        {
            ctx.CommitChanges();
        }

    }
}
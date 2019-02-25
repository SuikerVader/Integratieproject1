using Integratieproject1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integratieproject1.Models.Ideations;

namespace Integratieproject1.DAL.Repositorys
{
  public class IdeationRepository
  {
    private CityOfIdeasDbContext ctx = null;
    public IdeationRepository()
    {
      ctx = new CityOfIdeasDbContext();
      CityOfIdeasDbInitializer.Initialize(ctx, false);
    }
    public IEnumerable<Ideation> GetIdeations()
    {
      return ctx.Ideations.AsEnumerable();
    }
    public Ideation GetIdeation(int ideationId)
    {
      return ctx.Ideations.Find(ideationId);
    }
    public Ideation CreateIdeation(Ideation ideation)
    {
      ctx.Ideations.Add(ideation);
      ctx.SaveChanges();
      return ideation;
    }
  }
 
}

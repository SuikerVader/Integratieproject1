using Integratieproject1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.DAL.Repositorys
{
  public class IdeationRepository
  {
    private CityOfIdeasDbContext ctx = null;
    public IdeationRepository()
    {
      ctx = new CityOfIdeasDbContext();
      CityOfIdeasDbInitializer.Initialize(ctx, true);
    }
    public IEnumerable<Ideation> GetIdeations()
    {
      throw new NotImplementedException();
    }
    public Ideation CreateIdeation(Ideation ideation)
    {
      throw new NotImplementedException();
    }
  }
 
}

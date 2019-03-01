using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.DAL
{
  public class CityOfIdeasDbInitializer
  {
    private static bool hasRunDuringAppExecution = false;
    public static void Initialize(CityOfIdeasDbContext context
    , bool dropCreateDatabase = false)
    {
      if (!hasRunDuringAppExecution)
      {
        // Delete database if requested
        if (dropCreateDatabase)
          context.Database.EnsureDeleted();
        // Create database and initial data if needed
        if (context.Database.EnsureCreated())
          //Console.WriteLine("initializer test1");
        // Seed(context);
        hasRunDuringAppExecution = true;
      }
      
    }
  }
}

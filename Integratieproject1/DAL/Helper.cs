using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Integratieproject1.DAL
{
    public static class Helper
    {
        internal static void PrintDbContextTrackedEntitiesStates(DbContext context, string title = "CURRENT ENTITIES STATES")
        {
            System.Diagnostics.Debug.WriteLine("=== BEGIN: "+title+" ===");
            foreach (var entry in context.ChangeTracker.Entries().ToList())
                System.Diagnostics.Debug.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State.ToString()} ");
            System.Diagnostics.Debug.WriteLine("=== END ===");
        }
    }
}
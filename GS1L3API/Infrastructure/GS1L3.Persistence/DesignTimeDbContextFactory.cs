using GS1L3.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GS1L3.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<GS1L3DbContext>
    {
        public GS1L3DbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<GS1L3DbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseSqlServer(Configuration.ConnectionString);
            return new(dbContextOptionsBuilder.Options);
        }
    }
}

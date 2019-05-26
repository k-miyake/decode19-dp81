using Microsoft.EntityFrameworkCore;

namespace chengefeed_dotnet
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tweet> Tweets { get; set; }
    }
}

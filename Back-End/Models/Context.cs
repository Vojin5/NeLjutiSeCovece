using Microsoft.EntityFrameworkCore;

namespace Back_End.Models
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchHistory> PlayedMatches { get; set; }
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
    }
}

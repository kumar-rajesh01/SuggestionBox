using Microsoft.EntityFrameworkCore;
using SuggestionBox.Models;

namespace SuggestionBox.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Suggestion> Suggestions { get; set; }
    }
}

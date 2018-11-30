using JiraWorklogsApp.DAL.Persistance.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace JiraWorklogsApp.DAL.Persistance
{
    public partial class AppDbContext : DbContext
    {
        private IOptions<DatabaseSettings> DatabaseSettings { get; }

        public AppDbContext(IOptions<DatabaseSettings> databaseSettings)
        {
            DatabaseSettings = databaseSettings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(DatabaseSettings.Value.ConnectionString);
        }
    }
}

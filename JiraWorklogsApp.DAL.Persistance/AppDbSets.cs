using JiraWorklogsApp.DAL.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace JiraWorklogsApp.DAL.Persistance
{
    public partial class AppDbContext
    {
        public DbSet<JiraConnection> JiraConnections { get; set; }
    }
}

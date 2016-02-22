using System.Data.Entity;
using Referee.Entities;

namespace Referee.Web.DataContexts
{
    public class RefereeDbContext : DbContext
    {
        public RefereeDbContext() : base("DefaultConnection")
        {
        }

        public virtual DbSet<Goal> Goals { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<Table> Tables { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Player> Players { get; set; }
    }
}
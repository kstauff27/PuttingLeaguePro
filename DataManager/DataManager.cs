using System.Data.Entity;
using Models;

namespace DataManager
{
    public class PuttingLeagueDb : DbContext
    {
        public PuttingLeagueDb() 
            : base("PuttingLeagueDb")
        {
        }

        public DbSet<DefaultPointValue> DefaultPointValues { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PointValue> PointValues { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamPlayer> TeamPlayers { get; set; }
        public DbSet<RoundScore> RoundScores { get; set; }
    }

    public class PuttingLeagueInitializer : DropCreateDatabaseIfModelChanges<PuttingLeagueDb>
    {
        protected override void Seed(PuttingLeagueDb context)
        {
            DefaultPointValue pv1 = new DefaultPointValue() { Points = 4 };
            DefaultPointValue pv2 = new DefaultPointValue() { Points = 6 };
            DefaultPointValue pv3 = new DefaultPointValue() { Points = 8 };
            DefaultPointValue pv4 = new DefaultPointValue() { Points = 12 };
            DefaultPointValue pv5 = new DefaultPointValue() { Points = 16 };
            DefaultPointValue pv6 = new DefaultPointValue() { Points = 1 };

            context.DefaultPointValues.Add(pv1);
            context.DefaultPointValues.Add(pv2);
            context.DefaultPointValues.Add(pv3);
            context.DefaultPointValues.Add(pv4);
            context.DefaultPointValues.Add(pv5);
            context.DefaultPointValues.Add(pv6);

            context.SaveChanges();
        }
    }
}

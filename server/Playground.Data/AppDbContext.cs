using System.Linq;
using Playground.Data.Entities;
using Playground.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Playground.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Change> Changes { get; set; }

        public DbSet<Diff> Diffs { get; set; }

        public DbSet<Conflict> Conflicts { get; set; }

        public DbSet<Paragraph> Paragraphs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder
                .Model
                .GetEntityTypes()
                .Where(x => x.BaseType == null)
                .ToList()
                .ForEach(x =>
                {
                    modelBuilder
                        .Entity(x.Name)
                        .ToTable(x.Name.Split('.').Last());
                });
        }
    }
}

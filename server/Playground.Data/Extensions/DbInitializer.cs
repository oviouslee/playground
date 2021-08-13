using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Playground.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Playground.Data.Extensions
{
    public static class DbInitializer
    {
        public static async Task Initialize(this AppDbContext db)
        {
            Console.WriteLine("Initializing database");
            await db.Database.MigrateAsync();
            Console.WriteLine("Database initialized");

            if(!await db.Changes.AnyAsync())
            {
                Console.WriteLine("Seeding Changes");

                var changes = new List<Change>
                {
                    new Change
                    {
                        isApproved = false,

                        Diffs = new List<Diff>
                        {

                            new Diff
                            {
                                Type = "Paragraph",
                                Previous = "Test A",
                                Proposed = "Test B",
                                ChangeId = 1
                            },
                            new Diff
                            {
                                Type = "Paragraph",
                                Previous = "Test A",
                                Proposed = "Test C",
                                ChangeId = 1
                            }
                        }
                    }
                };

                await db.Changes.AddRangeAsync(changes);
                await db.SaveChangesAsync();
            }
        }
    }
}
